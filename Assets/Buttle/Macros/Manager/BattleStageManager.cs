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
class BattleStageManager
{
  private BattleData battleData;
  private List<Vector3> enemyPositionList;
  public List<Creature> enemies;
  public List<Creature> allies;
  private GameObject selectTargetEnemyPanel;
  private GameObject selectTargetAllyPanel;
  public BattleStageManager(
    BattleData battleData,
    GameObject selectTargetEnemyPanel,
    GameObject selectTargetAllyPanel
  )
  {
    this.battleData = battleData;
    this.selectTargetEnemyPanel = selectTargetEnemyPanel;
    this.selectTargetAllyPanel = selectTargetAllyPanel;
    enemies = new List<Creature>();
    allies = new List<Creature>();
  }

  public async UniTask Prepare()
  {
    // ステージ生成
    UnityEngine.Object.Instantiate(battleData.stage.field, new Vector3(0, 0, 0), Quaternion.identity);

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
  }

  public List<Vector3> LoadFromJSON(string json)
  {
    var data = JsonUtility.FromJson<EnemyPositionData>(json);
    return data.enemyPositions;
  }

  private async UniTask<Creature> instantiateCharacter(CreatureSetting creatureSetting, Vector3 position, GameObject parent)
  {
    var characterPrefab = await Addressables.LoadAssetAsync<GameObject>(creatureSetting.prefabAddress).Task;
    GameObject characterObject = UnityEngine.Object.Instantiate(characterPrefab);
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
	public class EnemyPositionData
	{
		public List<Vector3> enemyPositions;
	}
}