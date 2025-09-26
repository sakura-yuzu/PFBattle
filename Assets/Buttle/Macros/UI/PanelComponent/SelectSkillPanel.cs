using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Threading;
using System;
using Cysharp.Threading.Tasks;
using TMPro;
class SelectSkillPanel : ToggleGroupInherit
{
	public Transform parentPanel;
	public GameObject selectTargetEnemyPanel;
	public GameObject selectTargetAllyPanel;

	public List<SkillSetting> skills;

	// async void Awake(){
	// 	// await Prepare();
	// }

	public void setSkills(List<SkillSetting> _skills)
	{
		skills = _skills;
	}

	public async UniTask Prepare()
	{
		var buttonPrefab = await Addressables.LoadAssetAsync<GameObject>("Assets/Buttle/Prefab/UI/SelectSkillToggle.prefab").Task;
		GameObject instance;
		// Button button;
		BaseButton systemButton;
		foreach (SkillSetting skill in skills)
		{
			instance = Instantiate(buttonPrefab, parentPanel);
			// button = instance.GetComponent<Button>();
			systemButton = instance.GetComponent<BaseButton>();
			systemButton.setText(skill.skillName);
			Toggle toggle = instance.GetComponent<Toggle>();
			toggle.group = this;

			Debug.Log($"skill.targetType: {skill.targetType}");
			if (skill.targetType == SkillSetting.TargetType.EnemyOne ||
				skill.targetType == SkillSetting.TargetType.EnemyAll)
			{
				toggle.onValueChanged.AddListener((bool isOn) =>
				{
					Debug.Log($"50: isOn: {isOn}");
					if (isOn)
					{
						selectTargetEnemyPanel.SetActive(true);
						selectTargetAllyPanel.SetActive(false);
						if (skill.targetType == SkillSetting.TargetType.EnemyAll)
						{
							foreach (var enemyToggle in selectTargetEnemyPanel.GetComponentsInChildren<Toggle>())
							{
								if (enemyToggle.transition == Selectable.Transition.SpriteSwap)
								{
									var image = enemyToggle.targetGraphic as Image;
									if (image != null)
									{
										image.sprite = enemyToggle.spriteState.highlightedSprite;
									}
								}
							}
						}
					}
				});
			}
			else
			{
				toggle.onValueChanged.AddListener((bool isOn) =>
				{
					Debug.Log($"62: isOn: {isOn}");
					if (isOn)
					{
						selectTargetEnemyPanel.SetActive(false);
						selectTargetAllyPanel.SetActive(true);
					}
				});
			}

			toggles.Add(toggle);

			// buttons.Add(button);
		}
	}

	// public override async UniTask<Action> AwaitAnyButtonClickedAsync(CancellationToken cancellationToken)
	// {
	// 	var pushed = await UniTask.WhenAny(buttons
	// 									.Select(button => button.OnClickAsync(cancellationToken)));
	// 	// await UniTask.Delay(TimeSpan.FromSeconds(5f));
	// 	Action act =  new Action();
	// 	act.targetEnemy = skills[pushed];
	// 	return act;
	// }
}