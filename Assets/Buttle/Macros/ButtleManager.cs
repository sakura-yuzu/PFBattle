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
	public Canvas skillPanel;
	public Canvas itemPanel;
	public Canvas selectActionPanel;
	public Canvas selectTargetAllyPanel;
	public Canvas selectTargetEnemyPanel;

	private SkillPanel skillPanelComponent;
	private SelectTargetAllyPanel selectTargetAllyPanelComponent;
	private SelectTargetEnemyPanel selectTargetEnemyPanelComponent;
	private ItemPanel itemPanelComponent;
	private SelectActionPanel selectActionPanelComponent;

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
			sd.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 220);
			sd.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 480);
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

		
		selectTargetEnemyPanelComponent = selectTargetEnemyPanel.GetComponent<SelectTargetEnemyPanel>();
		selectTargetEnemyPanelComponent.setEnemies(enemies);
		selectTargetEnemyPanel.gameObject.SetActive(true);
		
		skillPanelComponent = skillPanel.GetComponent<SkillPanel>();

		skillPanelComponent.allySelectPanel = selectTargetAllyPanel;
		selectTargetAllyPanelComponent = selectTargetAllyPanel.GetComponent<SelectTargetAllyPanel>();
		selectTargetAllyPanelComponent.setAllies(allies);
		selectTargetAllyPanelComponent.gameObject.SetActive(true);
		
		skillPanelComponent.enemySelectPanel = selectTargetEnemyPanel;

		itemPanelComponent = itemPanel.GetComponent<ItemPanel>();
		itemPanelComponent.allySelectPanel = selectTargetAllyPanel;
		
		selectActionPanelComponent = selectActionPanel.GetComponent<SelectActionPanel>();
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
		
		// do{
		// ユーザーの入力を待つ
		await PlayerAction();
		// 与ダメを計算する
		// 残っているエネミーの攻撃を計算する
		// await EnemyAttack();
		// ユーザーの版
		// }while(ButtleEnd);
	}

	private async UniTask<List<Action>> PlayerAction()
	{
		List<Action> actions = new List<Action>();

		for(int i = 0; i < allies.Length; i++){
			var who = allies[i];
			actions.Add(await SelectAction(cancellationToken));
		}

		return actions;
	}

	private async UniTask<Action> SelectAction(CancellationToken cancellationToken){
		selectActionPanel.enabled = true;
		int actionGroup =  await selectActionPanelComponent.AwaitAnyButtonClikedAsync(cancellationToken);
		Action action = new Action();
		switch(actionGroup){
			case 0:
				action = await SelectAttackTarget(cancellationToken);
				// var target = await SelectAttackTarget(cancellationToken);
				// return calculateAttackEffect();
				break;
			case 1:
				action = await SelectSkill(cancellationToken);
				// var target = await SelectSkillTarget(cancellationToken);
				// return calculateSkillEffect();
				break;
			case 2:
				var act = await SelectItem(cancellationToken);
				// var target = await SelectItemTarget(cancellationToken);
				// return calculateItemEffect();
				break;
			case 3:
				var target = await SelectDefenceTarget(cancellationToken);
				// return calculateDefenceEffect();
				break;
		}
		selectActionPanel.enabled = false;
		return action;
	}

	private async UniTask<Action> SelectAttackTarget(CancellationToken cancellationToken){
		selectTargetEnemyPanel.enabled = true;
		Action act = await selectTargetEnemyPanelComponent.AwaitAnyButtonClikedAsync(cancellationToken);
		selectTargetEnemyPanel.enabled = false;
		return act;
	}

	private async UniTask<Action> SelectSkill(CancellationToken cancellationToken)
	{
		skillPanel.enabled = true;
		Action act = await skillPanelComponent.AwaitAnyButtonClikedAsync(cancellationToken);
		skillPanel.enabled = false;
		return act;
	}
	private async UniTask<Action> SelectItem(CancellationToken cancellationToken)
	{
		itemPanel.enabled = true;
		Action act = await itemPanelComponent.AwaitAnyButtonClikedAsync(cancellationToken);
		itemPanel.enabled = false;
		return act;
	}
	private async UniTask<Action> SelectDefenceTarget(CancellationToken cancellationToken)
	{
		selectTargetAllyPanel.enabled = true;
		Action act = await selectTargetAllyPanelComponent.AwaitAnyButtonClikedAsync(cancellationToken);
		selectTargetAllyPanel.enabled = false;
		return act;
	}

}