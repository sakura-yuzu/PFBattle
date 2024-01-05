using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
class SkillPanel : ButtonPanel
{
	public SkillDatabase skillDatabase;
	private SkillManager skillManager;
	public Transform scrollArea;

	void Awake(){
		skillManager = new SkillManager(skillDatabase);
		Prepare();
	}

	private async void Prepare(){
		var buttonPrefab = await Addressables.LoadAssetAsync<GameObject>("SystemButton").Task;
		foreach(Skill skill in skillManager.getAll())
		{
			var button = Instantiate(buttonPrefab, scrollArea);
			button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = skill.skillName;
			buttons.Add(button.GetComponent<Button>());
		}
	}

	public async UniTask<Action> AwaitAnyButtonClikedAsync(CancellationToken cancellationToken)
    {
			var pushed = await UniTask.WhenAny(buttons
    									.Select(button => button.OnClickAsync(cancellationToken)));
			Debug.Log(pushed);
			return new Action();
			// if(nextPanel){
			// 	return await nextPanel.AwaitAnyButtonClikedAsync(cancellationToken);
			// }else{
      //   //ボタンのどれかが押されたら完了
      //   return pushed;
			// }
    }
}