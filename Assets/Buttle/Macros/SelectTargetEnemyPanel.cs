using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
class SelectTargetEnemyPanel : ButtonPanel
{
	public Transform parentPanel;

	private Enemy[] enemies;

	void Awake(){
		// Prepare();
	}

	public void setEnemies(Enemy[] _enemies){
		enemies = _enemies;
	}

	private async UniTask Prepare(){
		var buttonPrefab = await Addressables.LoadAssetAsync<GameObject>("SystemButton").Task;
		foreach(Enemy enemy in enemies)
		{
			buttons.Add(Instantiate(buttonPrefab, parentPanel).GetComponent<Button>());
		}
	}
		public async UniTask<Action> AwaitAnyButtonClikedAsync(CancellationToken cancellationToken)
    {
			await Prepare();
			var pushed = await UniTask.WhenAny(buttons
    									.Select(button => button.OnClickAsync(cancellationToken)));
			Debug.Log(pushed);
			Action act =  new Action();
			act.targetEnemy = enemies[pushed];
			return act;
    }
}