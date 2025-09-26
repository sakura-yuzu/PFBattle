
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using TMPro;
using Cysharp.Threading.Tasks;

class ToggleGroupInherit : ToggleGroup
{
	public List<Toggle> toggles;
	public EventSystem eventSystem;
	public GameObject selfPanel;
	public GameObject prevPanel;

	public void SetAllTogglesEnable(bool enable)
	{
		toggles.ForEach((Toggle toggle) => { toggle.enabled = enable; });
	}

	public string getValue()
	{
		IEnumerable<Toggle> ggles = this.ActiveToggles();
		Toggle toggle = ggles.FirstOrDefault();
		if (toggle)
		{
			return toggle.GetComponent<BaseButton>().value;
		}
		return "";
	}

	public string[] getValues()
	{
		return toggles
			.Select(toggle => toggle.GetComponent<BaseButton>().value)
			.ToArray();
	}

	public async UniTask selectAsync(CancellationToken cancellationToken)
	{
		if (toggles.Count == 0)
		{
			return;
		}
		await UniTask.WhenAny(toggles
			.Select(toggle => toggle.OnValueChangedAsync(cancellationToken)));
	}

	public async UniTaskVoid regenerateButtons(IEnumerable<string> nameList)
	{
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
		var togglePrefab = await Addressables.LoadAssetAsync<GameObject>("Assets/Buttle/Prefab/UI/SelectSkillToggle.prefab").Task;
		foreach (string name in nameList)
		{
			var toggle = Instantiate(togglePrefab, transform);
			toggle.GetComponent<BaseButton>().value = name;
			toggle.GetComponentInChildren<TextMeshProUGUI>().text = name;
			// toggle.Find("Label").GetComponent<Text>().text = name;
			toggles.Add(toggle.GetComponent<Toggle>());
		}
		await UniTask.Yield();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Fire2"))
		{
			Cancel();
		}
	}
	public void OnActivate()
	{
		// toggles[0]でOutOfRange どうして？
		eventSystem.SetSelectedGameObject(toggles[0].gameObject);
	}
	public void Cancel()
	{
		// 操作不能にしていたパネルのトグルを復活させる
		SetAllTogglesEnable(true);
		// 選択していたものがOn状態のままでは困るのでOffにする
		Toggle selected = ActiveToggles().FirstOrDefault();
		selected.isOn = false;
		// eventSystemで前のパネルの選択状態を復元
		// ここでeventSystemを操作しないと十字キーとかで動かせなくなる
		eventSystem.SetSelectedGameObject(selected.gameObject);
		// このパネルを非表示にする
		selfPanel.SetActive(false);
	}


	override protected void OnDestroy()
	{
		Debug.Log($"{gameObject.name} destroyed");
	}
}