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
class SelectTargetEnemyPanel : ButtonPanel
{
	public Transform parentPanel;

	private List<EnemyClass> enemies;

	void Awake(){
		// Debug.Log("SelectTargetEnemyPanel::Awake");
		Prepare();
	}

	public void setEnemies(List<EnemyClass> _enemies){
		enemies = _enemies;
	}

	private async UniTask Prepare(){
		var buttonPrefab = await Addressables.LoadAssetAsync<GameObject>("SystemButton").Task;
		GameObject instance;
		Button button;
		SystemButton systemButton;
		foreach(EnemyClass enemy in enemies)
		{
			instance = Instantiate(buttonPrefab, parentPanel);
			button = instance.GetComponent<Button>();
			systemButton = instance.GetComponent<SystemButton>();
			systemButton.setText(enemy.name);
			buttons.Add(button);
		}
	}
		public override async UniTask<Action> AwaitAnyButtonClickedAsync(CancellationToken cancellationToken)
    {
			var pushed = await UniTask.WhenAny(buttons
    									.Select(button => button.OnClickAsync(cancellationToken)));
			// await UniTask.Delay(TimeSpan.FromSeconds(5f));
			Action act =  new Action();
			act.targetEnemy = enemies[pushed];
			return act;
    }
}