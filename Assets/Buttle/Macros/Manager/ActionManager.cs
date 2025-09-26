using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
class ActionManager
{
  public SelectActionPanel selectActionPanel;
  public GameObject selectSkillPanel;
  public GameObject selectItemPanel;
  public GameObject selectTargetEnemyPanel;
  public GameObject selectTargetAllyPanel;
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
  }
  public async UniTask<Action> selectAction(Character ally, CancellationToken cancellationToken)
  {
    selectSkillPanel.GetComponent<SelectSkillPanel>().setSkills(ally.skills);
    selectSkillPanel.GetComponent<SelectSkillPanel>().Prepare();
    selectActionPanel.gameObject.SetActive(true);
    Action action = await selectActionPanel.selectAction(ally, cancellationToken);
    selectActionPanel.gameObject.SetActive(false);
    selectSkillPanel.SetActive(false);
    selectTargetEnemyPanel.SetActive(false);
    selectTargetAllyPanel.SetActive(false);
    return action;
  }
}