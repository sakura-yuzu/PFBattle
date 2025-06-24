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
	private List<string> enemies;
	private List<string> allies;

	public async UniTask<Action> selectAction(Character actioner, CancellationToken cancellationToken)
	{
		// selectActionPanel.SetActive(true);
		// await selectActionPanel.GetComponent<ToggleGroupInherit>().selectAsync(cancellationToken);
		enemies = new List<string>();
		allies = new List<string>();

		await UniTask.WhenAny(panels
			.Select(panel => panel.selectAsync(cancellationToken)));

		string actionType = selectActionPanel.GetComponent<ToggleGroupInherit>().getValue();
		string skillName = selectSkillPanel.GetComponent<ToggleGroupInherit>()?.getValue();
		string item = selectItemPanel.GetComponent<ToggleGroupInherit>()?.getValue();

		enemies.Add(selectTargetEnemyPanel.GetComponent<ToggleGroupInherit>()?.getValue());
		allies.Add(selectTargetAllyPanel.GetComponent<ToggleGroupInherit>()?.getValue());

		return new Action(
			actionType,
			skillName,
			item,
			enemies,
			allies
		);
	}
}