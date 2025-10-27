using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

class ActionManager
{
  public SelectActionPanel selectActionPanel;
  public GameObject selectSkillPanel;
  public GameObject selectItemPanel;
  public GameObject selectTargetEnemyPanel;
  public GameObject selectTargetAllyPanel;
  private List<ToggleGroupInherit> panels;
  private List<Creature> enemies;
  private List<Creature> allies;
  public ActionManager(
    // EventSystem eventSystem,
    SelectActionPanel selectActionPanel,
    GameObject selectSkillPanel,
    // SelectItemPanel selectItemPanel,
    GameObject selectTargetAllyPanel,
    GameObject selectTargetEnemyPanel
  )
  {
    // this.eventSystem = eventSystem;
    this.selectActionPanel = selectActionPanel;
    this.selectSkillPanel = selectSkillPanel;
    // this.selectItemPanel = selectItemPanel;
    this.selectTargetAllyPanel = selectTargetAllyPanel;
    this.selectTargetEnemyPanel = selectTargetEnemyPanel;

    this.panels = new List<ToggleGroupInherit>{
      selectTargetEnemyPanel.GetComponent<ToggleGroupInherit>(),
      selectTargetAllyPanel.GetComponent<ToggleGroupInherit>()
    };
  }
  public async UniTask<Action> selectAction(Character actioner, List<Creature> aliveAllies, List<Creature> aliveEnemies, CancellationToken cancellationToken)
  {
    selectActionPanel.gameObject.SetActive(true);
    selectSkillPanel.GetComponent<SelectSkillPanel>().setSkills(actioner.skills);
    selectSkillPanel.GetComponent<SelectSkillPanel>().Prepare();
    selectActionPanel.gameObject.SetActive(true);
    Action action = await userInput(actioner, aliveAllies, aliveEnemies, cancellationToken);
    selectActionPanel.gameObject.SetActive(false);
    selectSkillPanel.SetActive(false);
    selectTargetEnemyPanel.SetActive(false);
    selectTargetAllyPanel.SetActive(false);
    return action;
  }

  public async UniTask<Action> userInput(Character actioner, List<Creature> aliveAllies, List<Creature> aliveEnemies, CancellationToken cancellationToken)
  {
    // selectActionPanel.SetActive(true);
    // await selectActionPanel.GetComponent<ToggleGroupInherit>().selectAsync(cancellationToken);
    enemies = new List<Creature>();
    allies = new List<Creature>();

    await UniTask.WhenAny(panels
      .Select(panel => panel.selectAsync(cancellationToken)));

    string actionType = selectActionPanel.GetComponent<ToggleGroupInherit>()?.GetSelectedObject<string>();
    SkillSetting skill = selectSkillPanel.GetComponent<ToggleGroupInherit>()?.GetSelectedObject<SkillSetting>();
    Item item = null;//selectItemPanel.GetComponent<ToggleGroupInherit>()?.GetSelectedObject<Item>();

    if (skill != null)
    {
      if (skill.targetType == SkillSetting.TargetType.EnemyAll)
      {
        enemies = selectTargetEnemyPanel.GetComponent<ToggleGroupInherit>()?.GetAllObjects<Creature>();
      }
      else if (skill.targetType == SkillSetting.TargetType.AllyAll)
      {
        allies = selectTargetAllyPanel.GetComponent<ToggleGroupInherit>()?.GetAllObjects<Creature>();
      }
      else if (skill.targetType == SkillSetting.TargetType.EnemyOne)
      {
        enemies.Add(selectTargetEnemyPanel.GetComponent<ToggleGroupInherit>()?.GetSelectedObject<Creature>());
      }
      else if (skill.targetType == SkillSetting.TargetType.AllyOne)
      {
        allies.Add(selectTargetAllyPanel.GetComponent<ToggleGroupInherit>()?.GetSelectedObject<Creature>());
      }
      
    }else {
      // 通常攻撃とかここ
      enemies.Add(selectTargetEnemyPanel.GetComponent<ToggleGroupInherit>()?.GetSelectedObject<Creature>());
    }

    Action action = new Action(
      actionType,
      skill,
      item,
      enemies,
      allies
    );
    action.setActioner(actioner);
    return action;
  }
}