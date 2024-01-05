using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
class ButtonPanel : MonoBehaviour
{
	public ButtonPanel nextPanel;
	public List<Button> buttons;
	// public async UniTask<Action> AwaitAnyButtonClikedAsync(CancellationToken cancellationToken)
  //   {
	// 		var pushed = await UniTask.WhenAny(buttons
  //   									.Select(button => button.OnClickAsync(cancellationToken)));
	// 		if(nextPanel){
	// 			return await nextPanel.AwaitAnyButtonClikedAsync(cancellationToken);
	// 		}else{
  //       //ボタンのどれかが押されたら完了
  //       return pushed;
	// 		}
  //   }
}