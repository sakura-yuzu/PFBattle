using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
class ItemPanel : ButtonPanel
{
	public ItemDatabase itemDatabase;
	private ItemManager itemManager;
	public Transform scrollArea;

	public Canvas allySelectPanel;
	public Canvas enemySelectPanel;

	void Awake()
	{
		Debug.Log("ItemPanel.Awake");
		itemManager = new ItemManager(itemDatabase);
		Prepare();
	}

	private async void Prepare()
	{
		var buttonPrefab = await Addressables.LoadAssetAsync<GameObject>("SystemButton").Task;
		foreach (Item item in itemManager.getAll())
		{
			var button = Instantiate(buttonPrefab, scrollArea);
			button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.itemName;
			buttons.Add(button.GetComponent<Button>());
		}
	}

	public override async UniTask<Action> AwaitAnyButtonClickedAsync(CancellationToken cancellationToken)
	{
		var pushed = await UniTask.WhenAny(buttons
										.Select(button => button.OnClickAsync(cancellationToken)));
		Debug.Log(pushed);
		Action act = new Action();
		Item selectedItem = itemManager.getAll()[pushed];

		m_cancellationTokenSource = null;

		allySelectPanel.enabled = true;
		act = await allySelectPanel.GetComponent<SelectTargetAllyPanel>().AwaitAnyButtonClickOrCancelAsync(cancellationToken);
		allySelectPanel.enabled = false;

		act.item = selectedItem;
		return act;
	}
}