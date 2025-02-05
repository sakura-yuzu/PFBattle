using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using System.Linq;
using System.IO;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;

class BattleManager : MonoBehaviour
{
	private List<Creature> enemies;
	private List<Character> allies;
	public BattleData battleData;
	public SelectActionPanel selectActionPanel;
	public GameObject selectSkillPanel;
	public GameObject selectItemPanel;
	public GameObject selectTargetEnemyPanel;
	public GameObject selectTargetAllyPanel;
	public GameObject characterStatusPanel;

	private List<Vector3> enemyPositionList;

	private bool BattleContinue = true;
	private CancellationToken cancellationToken;

	public List<Vector3> LoadFromJSON(string json)
	{
		var data = JsonUtility.FromJson<EnemyPositionData>(json);
		return data.enemyPositions;
	}

	async void Awake()
	{
		enemies = new List<Creature>();
		allies = new List<Character>();
		await Prepare();
		Battle();
	}

	private async UniTask Prepare()
	{
		// ステージ生成
		Instantiate(battleData.stage.field, new Vector3(0, 0, 0), Quaternion.identity);

		// エネミーの位置を読み込み
		string json = File.ReadAllText(battleData.stage.filePath);
		enemyPositionList = LoadFromJSON(json);

		int i = 0;
		// エネミー生成
		foreach (CreatureSetting enemySetting in battleData.stage.enemyList)
		{
			var enemyPrefab = await Addressables.LoadAssetAsync<GameObject>(enemySetting.prefabAddress).Task;
			GameObject enemyObject = Instantiate(enemyPrefab);
			Vector3 position = enemyPositionList[i];
			enemyObject.transform.position = position;
			Creature enemyComponent = enemyObject.GetComponent<Creature>();
			enemies.Add(enemyComponent);
			i++;
		}

		foreach (CreatureSetting character in battleData.selectedCharacterList)
		{
			var characterPrefab = await Addressables.LoadAssetAsync<GameObject>(character.prefabAddress).Task;
			GameObject ally = Instantiate(characterPrefab);
			Character allyComponent = ally.GetComponent<Character>();
			allies.Add(allyComponent);
		}
	}

	private async UniTaskVoid Battle()
	{
		cancellationToken = this.GetCancellationTokenOnDestroy();
		List<Action> actions;
		do
		{
			actions = new List<Action>();
			foreach (Character ally in allies)
			{
				// IEnumerable<string> skillNameList = actioner.skills.Select(x => x.skillName);
				// selectSkillPanel.GetComponent<ToggleGroupInherit>().regenerateButtons(skillNameList);
				Action action = await selectActionPanel.selectAction(cancellationToken);
				action.setActioner(ally);
				actions.Add(action);
			}

			actions.Sort(delegate (Action x, Action y)
			{
				return x.actioner.speed - y.actioner.speed;
			});

			foreach (Action action in actions)
			{
				action.execute();
			}

			// BattleContinue = false;
		} while (BattleContinue);
		SceneExit();
	}

	// シーンを終了し、ResultSceneに遷移
	private void SceneExit()
	{
		SceneManager.LoadScene("ResultScene");
	}

	public class EnemyPositionData
	{
		public List<Vector3> enemyPositions;
	}
}