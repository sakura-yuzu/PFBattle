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

	private bool ButtleEnd = false;

	private CancellationTokenSource m_cancellationTokenSource;

	void Awake()
	{
		cancellationToken = m_cancellationTokenSource.Token;
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

    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Z ) )
        {
            Buttle().Forget();
        }
        else if ( Input.GetKeyDown( KeyCode.Escape ) )
        {
            Debug.Log( "キャンセル" );

            m_cancellationTokenSource?.Cancel();
            m_cancellationTokenSource = null;
        }
    }

	private async UniTaskVoid Buttle()
	{
		// 与ダメを計算する
		// 残っているエネミーの攻撃を計算する
		// ユーザーの版
		
		// do{
		// ユーザーの入力を待つ
		await PlayerAction();
		// await EnemyAttack();
		// }while(ButtleEnd);
	}

	private async UniTask PlayerAction()
	{
		// TODO 何を返すか決めてない
		var action = await UniTask.WhenAny(actionButtons
											.Select(button => button.OnClickAsync(cancellationToken)));
		switch(action){
			case 0:
				var (attackTechnique, target) = await SelectAttackTechnique(cancellationToken);
				// var target = await SelectAttackTarget(cancellationToken);
				return calculateAttackEffect();
				break;
			case 1:
				var (skill, target) = await SelectSkill(cancellationToken);
				// var target = await SelectSkillTarget(cancellationToken);
				return calculateSkillEffect();
				break;
			case 2:
				var (item, target) = await SelectItem(cancellationToken);
				// var target = await SelectItemTarget(cancellationToken);
				return calculateItemEffect();
				break;
			case 3:
				var target = await SelectDefenceTarget(cancellationToken);
				return calculateDefenceEffect();
				break;
		}
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

	// private async UniTask<int> SelectSkillTarget(CancellationToken cancellationToken)
	// {
	// 	skillPanel.SetActive(true);
	// 	SkillPanel skillPanelComponent = skillPanel.GetComponent<SkillPanel>();
	// 	return await skillPanelComponent.AwaitAnyButtonClikedAsync(cancellationToken);
	// }

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