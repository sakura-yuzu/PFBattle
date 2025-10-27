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

class SelectTargetEnemyPanel : ToggleGroupInherit
{
	public Transform parentPanel;

	public List<Creature> enemies;

	public void setEnemies(List<Creature> _enemies)
	{
		enemies = _enemies;

	}

	public async UniTask Prepare()
	{
		toggles = new List<ToggleInherit>();
		var buttonPrefab = await Addressables.LoadAssetAsync<GameObject>("Assets/Buttle/Prefab/UI/SelectTargetToggle.prefab").Task;
		GameObject instance;
		foreach (Creature enemy in enemies)
		{
			instance = Instantiate(buttonPrefab, parentPanel);
			ToggleInherit toggle = instance.GetComponent<ToggleInherit>();
			toggle.SetObject(enemy);
			toggle.group = this;
			toggles.Add(toggle);
		}
	}
}