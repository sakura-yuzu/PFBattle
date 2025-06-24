using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
class SelectTargetAllyPanel : ToggleGroupInherit
{
	public Transform parentPanel;

	private List<Creature> allies;

	// async void Awake()
	// {
	// 	// await Prepare();
	// }

	public void setAllies(List<Creature> _allies)
	{
		allies = _allies;
	}

	public async UniTask Prepare(){
		var buttonPrefab = await Addressables.LoadAssetAsync<GameObject>("Assets/Buttle/Prefab/UI/SelectTargetToggle.prefab").Task;
		GameObject instance;
		// Button button;
		BaseButton systemButton;
		Debug.Log("allies.Count: " + allies.Count);
		foreach(Creature ally in allies)
		{
			instance = Instantiate(buttonPrefab, parentPanel);
			// button = instance.GetComponent<Button>();
			systemButton = instance.GetComponent<BaseButton>();
			systemButton.setText(ally.displayName);
			toggles.Add(instance.GetComponent<Toggle>());
			// buttons.Add(button);
		}
	}

	// public async UniTask Update(){
		
	// }

	// public override async UniTask<Action> AwaitAnyButtonClickedAsync(CancellationToken cancellationToken)
	// {
	// 	var pushed = await UniTask.WhenAny(buttons
	// 									.Select(button => button.OnClickAsync(cancellationToken)));
	// 	Action act = new Action();
	// 	act.targetAlly = allies[pushed];
	// 	return act;
	// }
}