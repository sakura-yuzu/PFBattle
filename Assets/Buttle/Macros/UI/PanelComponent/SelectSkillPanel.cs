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
		foreach (SkillSetting skill in skills)
		{
			instance = Instantiate(buttonPrefab, parentPanel);
			ToggleInherit toggle = instance.GetComponent<ToggleInherit>();
			toggle.SetObject(skill);
			toggle.group = this;
			toggle.onValueChanged.AddListener((bool isOn) =>
			{
				displayNextPanel(toggle, skill, isOn);
			});
			toggles.Add(toggle);
		}
	}

	private void displayNextPanel(ToggleInherit toggle, SkillSetting skill, bool isOn)
	{
		if (isOn)
		{
			// まだちょっと無駄がある気がする
			if (skill.targetType == SkillSetting.TargetType.EnemyAll
			|| skill.targetType == SkillSetting.TargetType.EnemyOne)
			{
				selectTargetEnemyPanel.SetActive(true);
				selectTargetAllyPanel.SetActive(false);
				if (skill.targetType == SkillSetting.TargetType.EnemyAll)
				{
					selectTargetEnemyPanel.GetComponent<SelectTargetEnemyPanel>().selectAll();
				}
			}
			else if (skill.targetType == SkillSetting.TargetType.AllyAll ||
			skill.targetType == SkillSetting.TargetType.AllyOne)
			{
				selectTargetEnemyPanel.SetActive(false);
				selectTargetAllyPanel.SetActive(true);
				if (skill.targetType == SkillSetting.TargetType.AllyAll)
				{
					selectTargetAllyPanel.GetComponent<SelectTargetAllyPanel>().selectAll();
				}
			}
		}
	}
}