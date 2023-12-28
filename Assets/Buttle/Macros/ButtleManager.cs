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

	void Awake(){
		Prepare();
	}

	private async void Prepare(){
		var allyPrefab = await Addressables.LoadAssetAsync<GameObject>("AllyButton").Task;
		foreach(Character character in allies){
			GameObject ally = Instantiate(allyPrefab, allyListArea, false);
			RectTransform sd = ally.GetComponent<RectTransform>();
			sd.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 200);
			BaseButton characterButton = ally.AddComponent<BaseButton>();
			characterButton.backgroundManager = new BackgroundManager(character.selectedImage, character.deselectedImage, ally);
		}
	}
}