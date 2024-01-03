using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
class AttackTechniquePanel : ButtonPanel
{
	public SkillDatabase skillDatabase;
	private SkillManager skillManager;
	public Transform scrollArea;

	void Awake(){
		skillManager = new SkillManager(skillDatabase);
		Prepare();
	}

	private async void Prepare(){
		var buttonPrefab = await Addressables.LoadAssetAsync<GameObject>("SystemButton").Task;
		foreach(Skill skill in skillManager.getAll())
		{
			buttons.Add(Instantiate(buttonPrefab, scrollArea).GetComponent<Button>());
		}
	}
}