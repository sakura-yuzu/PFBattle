using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;

class ButtleManager : MonoBehaviour
{

	public Enemy[] enemies;
	public Character[] allies;

	public Transform allyListArea;

	private Vector3[] enemyPosition;

	private CancellationToken cancellationToken;
	public Canvas selectActionPanel;
	private SelectActionPanel selectActionPanelComponent;

	private bool ButtleEnd = false;

	async void Awake()
	{
		enemyPosition = new Vector3[]{
			new Vector3(-6,2,2),
			new Vector3(-2,2,2),
			new Vector3(2,2,2),
			new Vector3(6,2,2)
		};
		await Prepare();
		Buttle();
	}

	private async UniTask Prepare()
	{
		var allyPrefab = await Addressables.LoadAssetAsync<GameObject>("AllyButton").Task;
		var enemyPrefab = await Addressables.LoadAssetAsync<GameObject>("Assets/Buttle/Prefab/Bird.prefab").Task;

		foreach (Character character in allies)
		{
			GameObject ally = Instantiate(allyPrefab, allyListArea, false);
			// サイズを指定
			RectTransform sd = ally.GetComponent<RectTransform>();
			sd.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 220);
			sd.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 480);
			// 独自コンポーネント追加
			CharacterSelectButton characterButton = ally.GetComponent<CharacterSelectButton>();
			characterButton.character = character;
			characterButton.Prepare(); // TODO: これキモくね？
		}

		foreach (Vector3 position in enemyPosition)
		{
			GameObject enemy = Instantiate(enemyPrefab);
			Transform transform = enemy.transform;
			transform.position = position;
		}

		selectActionPanelComponent = selectActionPanel.GetComponent<SelectActionPanel>();
		selectActionPanelComponent.enemies = enemies;
		selectActionPanelComponent.allies = allies;
		selectActionPanelComponent.Prepare();
	}

	private async UniTaskVoid Buttle()
	{
		cancellationToken = this.GetCancellationTokenOnDestroy();

		// do{
		// ユーザーの入力を待つ
		await PlayerAction();
		// 与ダメを計算する
		// 残っているエネミーの攻撃を計算する
		// await EnemyAttack();
		// ユーザーの版
		// }while(ButtleEnd);
	}

	private async UniTask<List<Action>> PlayerAction()
	{
		List<Action> actions = new List<Action>();

		for (int i = 0; i < allies.Length; i++)
		{
			var who = allies[i];
			Action action = await SelectAction(cancellationToken);
			action.actioner = who;
			actions.Add(action);
		}

		return actions;
	}

	private async UniTask<Action> SelectAction(CancellationToken cancellationToken)
	{
		selectActionPanel.enabled = true;
		Action action = await selectActionPanelComponent.AwaitAnyButtonClickedAsync(cancellationToken);
// FIXME who渡した方がよさそうな気がする
		selectActionPanel.enabled = false;
		return action;
	}

}