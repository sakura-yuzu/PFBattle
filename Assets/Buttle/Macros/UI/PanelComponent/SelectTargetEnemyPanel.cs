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

	public void setEnemies(List<Creature> _enemies){
		enemies = _enemies;

	}

	public async UniTask Prepare(){
		toggles = new List<Toggle>();
		var buttonPrefab = await Addressables.LoadAssetAsync<GameObject>("Assets/Buttle/Prefab/UI/SelectTargetToggle.prefab").Task;
		GameObject instance;
		// Button button;
		BaseButton systemButton;
		Debug.Log("enemies.Count: " + enemies.Count);
		foreach(Creature enemy in enemies)
		{
			Debug.Log("enemy.displayName: " + enemy.displayName);
			instance = Instantiate(buttonPrefab, parentPanel);
			// button = instance.GetComponent<Button>();
			systemButton = instance.GetComponent<BaseButton>();
			systemButton.setText(enemy.displayName);
			systemButton.value = enemy.displayName; // Set the value for the button
			Toggle toggle = instance.GetComponent<Toggle>();
			toggle.group = this;
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
		// 	act.targetEnemy = enemies[pushed];
		// 	return act;
    // }
}