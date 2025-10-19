using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System.Linq;
class SelectActionPanel : MonoBehaviour
{
	public GameObject selectActionPanel;
	public GameObject selectSkillPanel;
	public GameObject selectItemPanel;
	public GameObject selectTargetEnemyPanel;
	public GameObject selectTargetAllyPanel;
	public ToggleGroupInherit[] panels;
	public BattleManager battleManager;
	private List<Creature> enemies;
	private List<Creature> allies;

	public async UniTask<Action> selectAction(Character actioner, CancellationToken cancellationToken)
	{
		// selectActionPanel.SetActive(true);
		// await selectActionPanel.GetComponent<ToggleGroupInherit>().selectAsync(cancellationToken);
		enemies = new List<Creature>();
		allies = new List<Creature>();

		await UniTask.WhenAny(panels
			.Select(panel => panel.selectAsync(cancellationToken)));

		string actionType = selectActionPanel.GetComponent<ToggleGroupInherit>().GetSelectedObject<string>();
		SkillSetting skill = selectSkillPanel.GetComponent<ToggleGroupInherit>()?.GetSelectedObject<SkillSetting>();
		Item item = selectItemPanel.GetComponent<ToggleGroupInherit>()?.GetSelectedObject<Item>();

		// TODO: どうやって全体攻撃認識しようかなあ
		enemies = selectTargetEnemyPanel.GetComponent<ToggleGroupInherit>()?.GetSelectedObjects<Creature>() ?? new List<Creature>();
		allies.Add(selectTargetAllyPanel.GetComponent<ToggleGroupInherit>()?.GetSelectedObject<Creature>());

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