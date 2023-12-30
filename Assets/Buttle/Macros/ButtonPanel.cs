using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
class ButtonPanel : MonoBehaviour
{
	public ButtonPanel nextPanel;
	public List<Button> buttons;
	public async UniTask<int> AwaitAnyButtonClikedAsync(CancellationToken cancellationToken)
    {
			if(nextPanel){
				return await nextPanel.AwaitAnyButtonClikedAsync(cancellationToken);
			}else{
        //ボタンのどれかが押されたら完了
        return await UniTask.WhenAny(buttons
    									.Select(button => button.OnClickAsync(cancellationToken)));
			}
    }
}