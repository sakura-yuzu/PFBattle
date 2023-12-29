using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;
using UnityEngine.AddressableAssets;

class ButtleManager : MonoBehaviour {

	public Enemy[] enemies;
	public Character[] allies;

	public Transform allyListArea;

	private Vector3[] enemyPosition;

	void Awake(){
enemyPosition = new Vector3[]{
			new Vector3(-6,2,2),
			new Vector3(-2,2,2),
			new Vector3(2,2,2),
			new Vector3(6,2,2)		
		};
		Prepare();
	}

	private async void Prepare(){
		var allyPrefab = await Addressables.LoadAssetAsync<GameObject>("AllyButton").Task;
		var enemyPrefab = await Addressables.LoadAssetAsync<GameObject>("Assets/Buttle/Prefab/Bird.prefab").Task;

		foreach(Character character in allies){
			GameObject ally = Instantiate(allyPrefab, allyListArea, false);
			// サイズを指定
			RectTransform sd = ally.GetComponent<RectTransform>();
			sd.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);
			// 独自コンポーネント追加
			CharacterSelectButton characterButton = ally.GetComponent<CharacterSelectButton>();
			characterButton.character = character;
			characterButton.Prepare(); // TODO: これキモくね？
		}

		foreach(Vector3 position in enemyPosition){
			GameObject enemy = Instantiate(enemyPrefab);
			Transform transform = enemy.transform;
			transform.position = position;
		}
	}
}