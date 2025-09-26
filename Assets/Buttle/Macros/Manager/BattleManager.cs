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
using TMPro;

class BattleManager : MonoBehaviour
{
	private List<Creature> enemies;
	private List<Creature> allies;
	public BattleData battleData;

	// TODO: なんでアクションパネルとその他で型が違うんだか覚えてない
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
		allies = new List<Creature>();
		await Prepare();
		await Battle();
		SceneExit();
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
			Creature enemyComponent = await instantiateCharacter(enemySetting, enemyPositionList[i++], selectTargetEnemyPanel);
			enemies.Add(enemyComponent);
		}

		foreach (CreatureSetting characterSetting in battleData.selectedCharacterList)
		{
			Creature allyComponent = await instantiateCharacter(characterSetting, new Vector3(0, 0, 0), selectTargetAllyPanel);
			allies.Add(allyComponent);
		}
		selectTargetAllyPanel.GetComponent<SelectTargetAllyPanel>().setAllies(allies);
		await selectTargetAllyPanel.GetComponent<SelectTargetAllyPanel>().Prepare();
		selectTargetEnemyPanel.GetComponent<SelectTargetEnemyPanel>().setEnemies(enemies);
		await selectTargetEnemyPanel.GetComponent<SelectTargetEnemyPanel>().Prepare();
	}

	private async UniTask<Creature> instantiateCharacter(CreatureSetting creatureSetting, Vector3 position, GameObject parent)
	{
		var characterPrefab = await Addressables.LoadAssetAsync<GameObject>(creatureSetting.prefabAddress).Task;
		GameObject characterObject = Instantiate(characterPrefab);
		characterObject.transform.position = position;
		Creature characterComponent = characterObject.GetComponent<Creature>();

		// 同名のキャラクターが存在する場合、接尾辞を付けて区別する
		string baseName = creatureSetting.displayName;
		string uniqueName = baseName;
		int suffix = 1;

		while (enemies.Any(e => e.displayName == uniqueName) || allies.Any(a => a.displayName == uniqueName))
		{
			uniqueName = baseName + suffix.ToString();
			suffix++;
		}

		characterComponent.displayName = uniqueName;
		return characterComponent;
	}

	private async UniTask Battle()
	{
		cancellationToken = this.GetCancellationTokenOnDestroy();
		List<Action> actions;
		do
		{
			actions = new List<Action>();
			foreach (Character ally in allies)
			{
				// IEnumerable<string> skillNameList = ally.skills.Select(x => x.skillName);
				// selectSkillPanel.GetComponent<SelectSkillPanel>().regenerateButtons(skillNameList);
				selectSkillPanel.GetComponent<SelectSkillPanel>().setSkills(ally.skills);
				selectSkillPanel.GetComponent<SelectSkillPanel>().Prepare();

				selectActionPanel.gameObject.SetActive(true);
				Action action = await selectActionPanel.selectAction(ally, cancellationToken);
				selectActionPanel.gameObject.SetActive(false);
				selectSkillPanel.SetActive(false);
				selectTargetEnemyPanel.SetActive(false);
				selectTargetAllyPanel.SetActive(false);
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

				foreach (var enemyName in action.enemies)
				{
					// TODO: 全体攻撃対応
					Creature enemyCharacter = enemies.Find(enemy => enemy.displayName.Equals(enemyName));
					if (enemyCharacter != null)
					{
						enemyCharacter.ReactToAction(action);
					}
				}

				foreach (var characterName in action.allies)
				{
					Creature allyCharacter = allies.Find(ally => ally.displayName.Equals(characterName));
					if (allyCharacter != null)
					{
						allyCharacter.ReactToAction(action);
					}
				}
			}

			BattleContinue = false;
		} while (BattleContinue);
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