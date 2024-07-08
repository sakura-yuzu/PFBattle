using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;

class BattleManager : MonoBehaviour
{
	private List<Creature> enemies = new List<Creature>();
	private List<Creature> allies = new List<Creature>();
	public BattleData battleData;
	public SelectActionPanel selectActionPanel;

	public GameObject characterStatusPanel;

	// private Vector3[] enemyPosition;

	private bool BattleContinue = true;
	private CancellationToken cancellationToken;

	async void Awake()
	{
		// enemyPosition = new Vector3[]{
		// 	new Vector3(-6,2,2),
		// 	new Vector3(-2,2,2),
		// 	new Vector3(2,2,2),
		// 	new Vector3(6,2,2)
		// };
		// await Prepare();
		Battle();
	}

	private async UniTask Prepare()
	{
		// logPanelComponent = logPanel.GetComponent<LogPanelComponent>();
		// TODO: エネミーの数が現在固定
		// for (int i = 0; i < 4; i++)
		// {
		// 	var enemyPrefab = await Addressables.LoadAssetAsync<GameObject>(enemy.prefabAddress).Task;
		// 	Vector3 position = enemyPosition[i];
		// 	GameObject enemyObject = Instantiate(enemyPrefab);
		// 	Transform transform = enemyObject.transform;
		// 	transform.position = position;
		// 	Creature enemyComponent = enemyObject.GetComponent<Creature>();
		// 	enemies.Add(enemyComponent);
		// }

		// foreach (CreatureSetting character in battleData.selectedCharacterList)
		// {
		// 	var characterPrefab = await Addressables.LoadAssetAsync<GameObject>(character.prefabAddress).Task;
		// 	GameObject ally = Instantiate(characterPrefab);
		// 	Creature allyComponent = ally.GetComponent<Creature>();
		// 	allies.Add(allyComponent);
		// }
	}

	private async UniTaskVoid Battle()
	{
		cancellationToken = this.GetCancellationTokenOnDestroy();
		do
		{
			// Creature actioner = allies[0]; // TODO
			Action action = await selectActionPanel.selectAction(cancellationToken);
			Debug.Log("目印");
			// string className = "Liz";
			// Type type = Type.GetType(className);

			// // インスタンス作ったり
			// // Character character = (Character)Activator.CreateInstance(type);

			// // メソッド呼んだり
			// MethodInfo method = type.GetMethod("Fire");
			// method.Invoke(liz);
			// // await actioner.execute(action);
			// // BattleContinue = false;
		} while (BattleContinue);
		SceneManager.LoadScene("ResultScene");
	}

	// public void receiveAction(){
	// 	Action action = selectActionPanel.getAction();
	// }


	public void Kill(Creature creature)
	{

	}
}