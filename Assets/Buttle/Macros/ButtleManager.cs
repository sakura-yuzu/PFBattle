using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

using Cysharp.Threading.Tasks;

class ButtleManager : MonoBehaviour
{

	public Enemy[] enemies;
	public Character[] allies;

	public Transform allyListArea;

	private Vector3[] enemyPosition;

	public Button[] actionButtons;
	private CancellationToken cancellationToken;

	public GameObject attackTechniquePanel;
	public GameObject enemyListPanel;
	public GameObject skillPanel;

	void Awake()
	{
		cancellationToken = this.GetCancellationTokenOnDestroy();
		enemyPosition = new Vector3[]{
			new Vector3(-6,2,2),
			new Vector3(-2,2,2),
			new Vector3(2,2,2),
			new Vector3(6,2,2)
		};
		Prepare();
		Buttle();
	}

	private async void Prepare()
	{
		var allyPrefab = await Addressables.LoadAssetAsync<GameObject>("AllyButton").Task;
		var enemyPrefab = await Addressables.LoadAssetAsync<GameObject>("Assets/Buttle/Prefab/Bird.prefab").Task;

		foreach (Character character in allies)
		{
			GameObject ally = Instantiate(allyPrefab, allyListArea, false);
			// サイズを指定
			RectTransform sd = ally.GetComponent<RectTransform>();
			sd.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);
			// 独自コンポーネント追加
			CharacterSelectButton characterButton = ally.GetComponent<CharacterSelectButton>();
			characterButton.character = character;
			characterButton.Prepare(); // TODO: これキモくね？
		}

		foreach (Vector3 position in enemyPosition)
		{
			GameObject enemy = Instantiate(enemyPrefab);
			Transform transform = enemy.transform;
			transform.position = position;
		}
	}

	private async void Buttle()
	{
		// 与ダメを計算する
		// 残っているエネミーの攻撃を計算する
		// ユーザーの版


		bool win = false;
		bool lose = false;

		// while(true){
		// ユーザーの入力を待つ
		await PlayerAction();
		// await calculatePlayerAttack();
		// await EnemyAttack();
		// };
	}

	private async UniTask PlayerAction()
	{
		// (action, target) = await PlayerDecide();
		// 	await ActionPanel.AwaitAnyButtonClikedAsync();
		var action = await UniTask.WhenAny(actionButtons
											.Select(button => button.OnClickAsync(cancellationToken)));
		switch(action){
			// case 0:
			// 	var attackTechnique = await SelectAttackTechnique(cancellationToken);
			// 	var target = await SelectAttackTarget(cancellationToken);
			// 	break;
			case 1:
			var skill = await SelectSkill(cancellationToken);
				break;
			// case 2:
			// 	break;
			// case 3:
			// 	break;
		}
		// return null;
	}

	// private async UniTask<int> SelectAttackTechnique(CancellationToken cancellationToken){
	// 	attackTechniquePanel.SetActive(true);
	// 	AttackTechniquePanel AttackTechniquePanelComponent = attackTechniquePanel.GetComponent<AttackTechniquePanel>();
	// 	return await AttackTechniquePanelComponent.SelectAttackTechnique();
	// }

	// private async UniTask<int> SelectAttackTarget(CancellationToken cancellationToken){
	// 	attackTechniquePanel.SetActive(true);
	// 	AttackTechniquePanel AttackTechniquePanelComponent = attackTechniquePanel.GetComponent<AttackTechniquePanel>();
	// 	return await AttackTechniquePanelComponent.SelectAttackTechnique();
	// }

	private async UniTask<int> SelectSkill(CancellationToken cancellationToken)
	{
		skillPanel.SetActive(true);
		SkillPanel skillPanelComponent = skillPanel.GetComponent<SkillPanel>();
		return await skillPanelComponent.AwaitAnyButtonClikedAsync(cancellationToken);
	}

	private async UniTask<int> SelectSkillTarget(CancellationToken cancellationToken)
	{
		skillPanel.SetActive(true);
		SkillPanel skillPanelComponent = skillPanel.GetComponent<SkillPanel>();
		return await skillPanelComponent.AwaitAnyButtonClikedAsync(cancellationToken);
	}

	// (int min, int max) FindMinMax(int[] input){

	// }
	// private async void calculatePlayerAttack()
	// {
	// 	foreach(Character ally in allies){
	// 		ally.attackPower
	// 	}
	// }
	// private async void EnemyAttack()
	// {

	// }

}