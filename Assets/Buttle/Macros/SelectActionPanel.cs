using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
class SelectActionPanel : ButtonPanel
{
	public async UniTask<int> AwaitAnyButtonClikedAsync(CancellationToken cancellationToken)
	{
		return await UniTask.WhenAny(buttons
										.Select(button => button.OnClickAsync(cancellationToken)));
	}
}