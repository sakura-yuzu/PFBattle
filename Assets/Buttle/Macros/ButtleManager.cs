using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;

class ButtleManager : MonoBehaviour
{

	public Enemy enemy;
	private List<EnemyComponent> enemies = new List<EnemyComponent>();
	private List<AllyComponent> allies = new List<AllyComponent>();
	public SelectedAllyList selectedCharacters;

	public Transform allyListArea;
	public List<CharacterSelectButton> characterSelectButtonList;

	private Vector3[] enemyPosition;

	private CancellationToken cancellationToken;
	public Canvas selectTargetAllyPanel;
	public Canvas selectTargetEnemyPanel;

	private List<GameObject> allyActionPanelList;
	private SelectTargetAllyPanel selectTargetAllyPanelComponent;
	private SelectTargetEnemyPanelComponent selectTargetEnemyPanelComponent;

	public GameObject logPanel;
	private LogPanelComponent logPanelComponent;

	private bool BattleContinue = true;

	async void Awake()
	{
		enemyPosition = new Vector3[]{
			new Vector3(-6,2,2),
			new Vector3(-2,2,2),
			new Vector3(2,2,2),
			new Vector3(6,2,2)
		};
		await Prepare();
		Buttle();
	}

	private async UniTask Prepare()
	{
		logPanelComponent = logPanel.GetComponent<LogPanelComponent>();
		// TODO: エネミーの数が現在固定
		for(int i=0;i<4;i++){
			var enemyPrefab = await Addressables.LoadAssetAsync<GameObject>(enemy.prefabAddress).Task;
			Vector3 position = enemyPosition[i];
			GameObject enemyObject = Instantiate(enemyPrefab);
			Transform transform = enemyObject.transform;
			transform.position = position;
			EnemyComponent enemyComponent = enemyObject.GetComponent<EnemyComponent>();
			enemyComponent.setData(enemy);
			enemies.Add(enemyComponent);
		}

		selectTargetEnemyPanelComponent = selectTargetEnemyPanel.GetComponent<SelectTargetEnemyPanelComponent>();
		selectTargetEnemyPanelComponent.setEnemies(enemies);
		selectTargetEnemyPanel.gameObject.SetActive(true);

		foreach(Character character in selectedCharacters.selectedCharacterList){
			var characterPrefab = await Addressables.LoadAssetAsync<GameObject>(character.prefabAddress).Task;
			GameObject ally = Instantiate(characterPrefab);
			AllyComponent allyComponent = ally.GetComponent<AllyComponent>();
			allyComponent.setData(character);
			allies.Add(allyComponent);
		}

		selectTargetAllyPanelComponent = selectTargetAllyPanel.GetComponent<SelectTargetAllyPanel>();
		selectTargetAllyPanelComponent.setAllies(allies);
		selectTargetAllyPanelComponent.gameObject.SetActive(true);

		allyActionPanelList = new List<GameObject>();
		var allyButtonPrefab = await Addressables.LoadAssetAsync<GameObject>("AllyButton").Task;
		var actionPanelPrefab = await Addressables.LoadAssetAsync<GameObject>("Assets/Buttle/Prefab/AllyActionPanel.prefab").Task;

		foreach(AllyComponent allyComponent in allies){
			// ボタン作成
			GameObject allyButton = Instantiate(allyButtonPrefab, allyListArea, false);
			// サイズを指定
			RectTransform sd = allyButton.GetComponent<RectTransform>();
			sd.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 220);
			sd.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 480);
			// 独自コンポーネント追加
			CharacterSelectButton characterButton = allyButton.GetComponent<CharacterSelectButton>();
			characterButton.character = allyComponent;
			characterButton.Prepare(); // TODO: これキモくね？
			characterSelectButtonList.Add(characterButton);
			allyComponent.setCharacterButton(characterButton);

			GameObject allyActionPanel = Instantiate(actionPanelPrefab);
			AllyActionPanelComponent allyActionPanelComponent = allyActionPanel.GetComponent<AllyActionPanelComponent>();
			allyActionPanelComponent.character = allyComponent;
			allyActionPanelComponent.selectTargetAllyPanel = selectTargetAllyPanel;
			allyActionPanelComponent.selectTargetAllyPanelComponent = selectTargetAllyPanelComponent;
			allyActionPanelComponent.selectTargetEnemyPanel = selectTargetEnemyPanel;
			allyActionPanelComponent.selectTargetEnemyPanelComponent = selectTargetEnemyPanelComponent;
			await allyActionPanelComponent.Prepare();
			allyActionPanelList.Add(allyActionPanel);
		}
	}

	private async UniTaskVoid Buttle()
	{
		cancellationToken = this.GetCancellationTokenOnDestroy();
		do{
			// ログパネルを非表示にする
			// allyListArea.gameObject.SetActive(false);
			// ユーザーの入力を待つ
			List<Action> actions = await PlayerAction(cancellationToken);
			// 残っているエネミーの攻撃を計算する
			List<Action> enemyActions = await EnemyAction();
			enemyActions.ForEach(act => actions.Add(act));
			// ログパネルを表示する
			// allyListArea.gameObject.SetActive(true);
			// 与ダメを計算する
			BattleContinue = await Calculate(actions, cancellationToken);
			// ユーザーの版
		}while(BattleContinue);
		SceneManager.LoadScene("ResultScene");
	}

	private async UniTask<List<Action>> PlayerAction(CancellationToken cancellationToken)
	{
		List<Action> actions = new List<Action>();

		foreach(GameObject allyActionPanel in allyActionPanelList){
			allyActionPanel.GetComponent<Canvas>().enabled = true;
			Action action = await allyActionPanel.GetComponent<AllyActionPanelComponent>().AwaitAnyButtonClickedAsync(cancellationToken);
			allyActionPanel.GetComponent<Canvas>().enabled = false;
			actions.Add(action);
		}

		return actions;
	}

	private async UniTask<List<Action>> EnemyAction()
	{
		List<Action> actions = new List<Action>();
		foreach(EnemyComponent enemy in enemies){
			actions.Add(enemy.Attack(allies));
		}
		return actions;
	}

	private async UniTask<bool> Calculate(List<Action> actions, CancellationToken cancellationToken)
	{
		// 素早い順に行動する
		actions.Sort(delegate(Action x, Action y)
		{
				return x.actioner.speed.CompareTo(y.actioner.speed);
		});

		foreach(Action action in actions){
			if(action.actioner == null){
				continue;
			}

			switch(action.actionType){
				case Action.Types.Attack:
					logPanelComponent.addText(action.actioner.displayName + "の攻撃！");
					if(action.actioner is AllyComponent){
						EnemyComponent targetEnemy = action.targetEnemy;
						if(!enemies.Contains(targetEnemy)){
							targetEnemy = enemies[0];
						}
						int hp = await targetEnemy.Damaged(100);
						logPanelComponent.addText(targetEnemy.displayName + "に" + 100 + "のダメージ！");

						if(hp <= 0){
							logPanelComponent.addText(targetEnemy.displayName + "を倒した！");
							await targetEnemy.Death();
							enemies.Remove(targetEnemy);
						}
					} else {
						AllyComponent targetAlly = action.targetAlly;
						if(!allies.Contains(targetAlly)){
							targetAlly = allies[0];
						}
						int hp = await targetAlly.Damaged(100);
						logPanelComponent.addText(targetAlly.displayName + "に" + 100 + "のダメージ！");
						targetAlly.characterButton.updateHp(hp);

						// if(hp <= 0){
						// 	logPanelComponent.addText(targetAlly.displayName + "は倒れた！");
						// 	await targetAlly.Death();
						// 	allies.Remove(targetAlly);
						// }
					}
					break;
				case Action.Types.Skill:
					action.skill.execute();
					break;
				default:
					break;
			}
		}
		return enemies.Count != 0 && allies.Count != 0;
	}

}