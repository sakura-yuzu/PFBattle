using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
class SelectActionPanel : ButtonPanel
{

	public Canvas skillPanel;
	public Canvas itemPanel;
	public Canvas selectTargetAllyPanel;
	public Canvas selectTargetEnemyPanel;

	private SkillPanel skillPanelComponent;
	private SelectTargetAllyPanel selectTargetAllyPanelComponent;
	private SelectTargetEnemyPanel selectTargetEnemyPanelComponent;
	private ItemPanel itemPanelComponent;
	public Enemy[] enemies;
	public Character[] allies;

	public async void Prepare()
	{
		selectTargetEnemyPanelComponent = selectTargetEnemyPanel.GetComponent<SelectTargetEnemyPanel>();
		selectTargetEnemyPanelComponent.setEnemies(enemies);
		selectTargetEnemyPanel.gameObject.SetActive(true);

		skillPanelComponent = skillPanel.GetComponent<SkillPanel>();
		skillPanelComponent.gameObject.SetActive(true);

		skillPanelComponent.allySelectPanel = selectTargetAllyPanel;
		selectTargetAllyPanelComponent = selectTargetAllyPanel.GetComponent<SelectTargetAllyPanel>();
		selectTargetAllyPanelComponent.setAllies(allies);
		selectTargetAllyPanelComponent.gameObject.SetActive(true);

		skillPanelComponent.enemySelectPanel = selectTargetEnemyPanel;

		itemPanelComponent = itemPanel.GetComponent<ItemPanel>();
		itemPanelComponent.allySelectPanel = selectTargetAllyPanel;
		itemPanelComponent.gameObject.SetActive(true);
	}

	public override async UniTask<Action> AwaitAnyButtonClickedAsync(CancellationToken cancellationToken)
	{
		gameManagerCancellationToken = cancellationToken;
		Action action = null;
		int actionGroup = await UniTask.WhenAny(buttons
										.Select(button => button.OnClickAsync(cancellationToken)));
		switch (actionGroup)
		{
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
		// FIXME
		return new Action();
	}

	private async UniTask<Action> SelectAttackTarget(CancellationToken cancellationToken)
	{
		selectTargetEnemyPanel.enabled = true;
		Action act = await selectTargetEnemyPanelComponent.AwaitAnyButtonClickOrCancelAsync(cancellationToken);
		selectTargetEnemyPanel.enabled = false;
		return act;
	}

	private async UniTask<Action> SelectSkill(CancellationToken cancellationToken)
	{
		skillPanel.enabled = true;
		Action act = await skillPanelComponent.AwaitAnyButtonClickOrCancelAsync(cancellationToken);
		skillPanel.enabled = false;
		return act;
	}
	private async UniTask<Action> SelectItem(CancellationToken cancellationToken)
	{
		itemPanel.enabled = true;
		Action act = await itemPanelComponent.AwaitAnyButtonClickOrCancelAsync(cancellationToken);
		itemPanel.enabled = false;
		return act;
	}
	private async UniTask<Action> SelectDefenceTarget(CancellationToken cancellationToken)
	{
		selectTargetAllyPanel.enabled = true;
		Action act = await selectTargetAllyPanelComponent.AwaitAnyButtonClickOrCancelAsync(cancellationToken);
		selectTargetAllyPanel.enabled = false;
		return act;
	}
}