using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;

class ButtleManager : MonoBehaviour
{

	public Enemy[] enemies;
	public Character[] allies;

	public Transform allyListArea;

	private Vector3[] enemyPosition;

	private CancellationToken cancellationToken;
	public GameObject skillPanel;
	public Canvas selectActionPanel;
	public Canvas selectTargetEnemyPanel;

	private List<Button> allyButtons;

	private bool ButtleEnd = false;

	private CancellationTokenSource m_cancellationTokenSource;

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
		allyButtons = new List<Button>();
		var allyPrefab = await Addressables.LoadAssetAsync<GameObject>("AllyButton").Task;
		var enemyPrefab = await Addressables.LoadAssetAsync<GameObject>("Assets/Buttle/Prefab/Bird.prefab").Task;

		foreach (Character character in allies)
		{
			GameObject ally = Instantiate(allyPrefab, allyListArea, false);
			allyButtons.Add(ally.GetComponent<Button>());
			// サイズを指定
			RectTransform sd = ally.GetComponent<RectTransform>();
			sd.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);
			// 独自コンポーネント追加
			CharacterSelectButton characterButton = ally.GetComponent<CharacterSelectButton>();
			characterButton.character = character;
			characterButton.Prepare(); // TODO: これキモくね？
		}

		foreach (Vector3 position in enemyPosition)
		{
			GameObject enemy = Instantiate(enemyPrefab);
			Transform transform = enemy.transform;
			transform.position = position;
		}
	}

    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Z ) )
        {
            Buttle().Forget();
        }
        else if ( Input.GetKeyDown( KeyCode.Escape ) )
        {
            Debug.Log( "キャンセル" );

            m_cancellationTokenSource?.Cancel();
            m_cancellationTokenSource = null;
        }
    }

	private async UniTaskVoid Buttle()
	{
		m_cancellationTokenSource = new();
		cancellationToken = m_cancellationTokenSource.Token;
		// 与ダメを計算する
		// 残っているエネミーの攻撃を計算する
		// ユーザーの版
		
		// do{
		// ユーザーの入力を待つ
		await PlayerAction();
		// await EnemyAttack();
		// }while(ButtleEnd);
	}

	private async UniTask<Action> PlayerAction()
	{
		// TODO 何を返すか決めてない
		var who = await UniTask.WhenAny(allyButtons.ToArray()
											.Select(button => button.OnClickAsync(cancellationToken)));
		return await SelectAction(cancellationToken);
	}

	private async UniTask<Action> SelectAction(CancellationToken cancellationToken){
		selectActionPanel.enabled = true;
		SelectActionPanel selectActionPanelComponent = selectActionPanel.GetComponent<SelectActionPanel>();
		int actionGroup =  await selectActionPanelComponent.AwaitAnyButtonClikedAsync(cancellationToken);
		Action action = new Action();
		switch(actionGroup){
			case 0:
				action = await SelectAttackTarget(cancellationToken);
				Debug.Log("目印");
				Debug.Log(action);
				// var target = await SelectAttackTarget(cancellationToken);
				// return calculateAttackEffect();
				break;
			case 1:
				action = await SelectSkill(cancellationToken);
				// var target = await SelectSkillTarget(cancellationToken);
				// return calculateSkillEffect();
				break;
			case 2:
				// var act = await SelectItem(cancellationToken);
				// var target = await SelectItemTarget(cancellationToken);
				// return calculateItemEffect();
				break;
			case 3:
				// var target = await SelectDefenceTarget(cancellationToken);
				// return calculateDefenceEffect();
				break;
		}
		selectActionPanel.enabled = false;
		return action;
	}

	private async UniTask<Action> SelectAttackTarget(CancellationToken cancellationToken){
		selectTargetEnemyPanel.enabled = true;
		SelectTargetEnemyPanel selectTargetEnemyPanelComponent = selectTargetEnemyPanel.GetComponent<SelectTargetEnemyPanel>();
		selectTargetEnemyPanelComponent.setEnemies(enemies);
		Action act = await selectTargetEnemyPanelComponent.AwaitAnyButtonClikedAsync(cancellationToken);
		selectTargetEnemyPanel.enabled = false;
		return act;
	}

	private async UniTask<Action> SelectSkill(CancellationToken cancellationToken)
	{
		skillPanel.SetActive(true);
		SkillPanel skillPanelComponent = skillPanel.GetComponent<SkillPanel>();
		return await skillPanelComponent.AwaitAnyButtonClikedAsync(cancellationToken);
	}
	private async UniTask<Action> SelectItem(CancellationToken cancellationToken)
	{
		skillPanel.SetActive(true);
		SkillPanel skillPanelComponent = skillPanel.GetComponent<SkillPanel>();
		return await skillPanelComponent.AwaitAnyButtonClikedAsync(cancellationToken);
	}
	private async UniTask<Action> SelectDefenceTarget(CancellationToken cancellationToken)
	{
		skillPanel.SetActive(true);
		SkillPanel skillPanelComponent = skillPanel.GetComponent<SkillPanel>();
		return await skillPanelComponent.AwaitAnyButtonClikedAsync(cancellationToken);
	}

}