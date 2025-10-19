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

	private ActionManager actionManager;
	private BattleStageManager battleStageManager;

	private bool BattleContinue = true;
	private CancellationToken cancellationToken;

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
		/**
		* 行動選択マネージャ
		* 行動→詳細→対象の選択までのUIを一括で管理する
		*/
		actionManager = new ActionManager(
			// eventSystem,
			selectActionPanel,
			selectSkillPanel,
			// selectItemPanel,
			selectTargetAllyPanel,
			selectTargetEnemyPanel
		);

    /**
		* バトルステージマネージャ
		* エネミー、味方の生成、ステージの生成を行う
		*/
		battleStageManager = new BattleStageManager(battleData, selectTargetEnemyPanel, selectTargetAllyPanel);
		await battleStageManager.Prepare();

		enemies = battleStageManager.enemies;
		allies = battleStageManager.allies;

		selectTargetAllyPanel.GetComponent<SelectTargetAllyPanel>().setAllies(allies);
		await selectTargetAllyPanel.GetComponent<SelectTargetAllyPanel>().Prepare();
		selectTargetEnemyPanel.GetComponent<SelectTargetEnemyPanel>().setEnemies(enemies);
		await selectTargetEnemyPanel.GetComponent<SelectTargetEnemyPanel>().Prepare();
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
				Action action = await actionManager.selectAction(ally, allies, enemies, cancellationToken);
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
}