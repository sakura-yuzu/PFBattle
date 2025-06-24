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

	public List<Skill> skills;

	// async void Awake(){
	// 	// await Prepare();
	// }

	public void setSkills(List<Skill> _skills){
		skills = _skills;
	}

	public async UniTask Prepare(){
		var buttonPrefab = await Addressables.LoadAssetAsync<GameObject>("Assets/Buttle/Prefab/UI/SystemButton.prefab").Task;
		GameObject instance;
		// Button button;
		BaseButton systemButton;
		Debug.Log("skills.Count: " + skills.Count);
		foreach(Skill skill in skills)
		{
			instance = Instantiate(buttonPrefab, parentPanel);
			// button = instance.GetComponent<Button>();
			systemButton = instance.GetComponent<BaseButton>();
			systemButton.setText(skill.setting.skillName);
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