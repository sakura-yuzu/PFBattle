using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;

class ButtleManager : MonoBehaviour
{

	public Enemy enemy;
	private List<Creature> enemies = new List<Creature>();
	private List<Creature> allies = new List<Creature>();
	public SelectedAllyList selectedCharacters;

	public GameObject selectActionPanel;
	public GameObject selectSkillPanel;
	public GameObject selectItemPanel;
	public GameObject selectTargetEnemyPanel;
	public GameObject selectTargetAllyPanel;

	private Vector3[] enemyPosition;

	private bool BattleContinue = true;

	async void Awake()
	{
		enemyPosition = new Vector3[]{
			new Vector3(-6,2,2),
			new Vector3(-2,2,2),
			new Vector3(2,2,2),
			new Vector3(6,2,2)
		};
		// await Prepare();
		// Buttle();
	}

	private async UniTask Prepare()
	{
		// logPanelComponent = logPanel.GetComponent<LogPanelComponent>();
		// TODO: エネミーの数が現在固定
		for(int i=0;i<4;i++){
			var enemyPrefab = await Addressables.LoadAssetAsync<GameObject>(enemy.prefabAddress).Task;
			Vector3 position = enemyPosition[i];
			GameObject enemyObject = Instantiate(enemyPrefab);
			Transform transform = enemyObject.transform;
			transform.position = position;
			Creature enemyComponent = enemyObject.GetComponent<Creature>();
			enemies.Add(enemyComponent);
		}

		foreach(Character character in selectedCharacters.selectedCharacterList){
			var characterPrefab = await Addressables.LoadAssetAsync<GameObject>(character.prefabAddress).Task;
			GameObject ally = Instantiate(characterPrefab);
			Creature allyComponent = ally.GetComponent<Creature>();
			allies.Add(allyComponent);
		}
	}

	private async UniTaskVoid Buttle()
	{
		do{

		}while(BattleContinue);
		SceneManager.LoadScene("ResultScene");
	}

	public void receiveAction(){
		Debug.Log("レシーブ");
		string action = selectActionPanel.GetComponent<ToggleGroupInherit>().getValue();
		string skill = selectSkillPanel.GetComponent<ToggleGroupInherit>()?.getValue();
		string item = selectItemPanel.GetComponent<ToggleGroupInherit>()?.getValue();
		string enemy = selectTargetEnemyPanel.GetComponent<ToggleGroupInherit>()?.getValue();
		string ally = selectTargetAllyPanel.GetComponent<ToggleGroupInherit>()?.getValue();
		Debug.Log("action: " + action);
		Debug.Log("skill: " + skill);
		Debug.Log("item: " + item);
		Debug.Log("enemy: " + enemy);
		Debug.Log("ally: " + ally);
	}

}