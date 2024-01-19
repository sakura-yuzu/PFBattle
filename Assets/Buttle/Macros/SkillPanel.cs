using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
class SkillPanel : ButtonPanel
{
	public SkillDatabase skillDatabase;
	private SkillManager skillManager;
	public Transform scrollArea;

	public Canvas allySelectPanel;
	public Canvas enemySelectPanel;

	void Awake()
	{
		skillManager = new SkillManager(skillDatabase);
		Prepare();
	}

	private async void Prepare()
	{
		var buttonPrefab = await Addressables.LoadAssetAsync<GameObject>("SystemButton").Task;
		foreach (Skill skill in skillManager.getAll())
		{
			var button = Instantiate(buttonPrefab, scrollArea);
			button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = skill.skillName;
			buttons.Add(button.GetComponent<Button>());
		}
	}

	public override async UniTask<Action> AwaitAnyButtonClickedAsync(CancellationToken cancellationToken)
	{
		var pushed = await UniTask.WhenAny(buttons
										.Select(button => button.OnClickAsync(cancellationToken)));
		Debug.Log(pushed);
		Action act = new Action();
		Skill selectedSkill = skillManager.getAll()[pushed];

		if (selectedSkill.skillType == Skill.SkillType.Enchant)
		{
			allySelectPanel.enabled = true;
			act = await allySelectPanel.GetComponent<SelectTargetAllyPanel>().AwaitAnyButtonClickOrCancelAsync(cancellationToken);
			allySelectPanel.enabled = false;
		}
		else
		{
			enemySelectPanel.enabled = true;
			act = await enemySelectPanel.GetComponent<SelectTargetEnemyPanel>().AwaitAnyButtonClickOrCancelAsync(cancellationToken);
			enemySelectPanel.enabled = false;
		}
		act.skill = selectedSkill;
		return act;
	}
}