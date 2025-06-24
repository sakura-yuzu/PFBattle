using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AddressableAssets;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
class ButtonPanel : MonoBehaviour
{
	public GameObject selfPanel;
	public GameObject prevPanel;
	public EventSystem eventSystem;
	// public List<Button> buttons;
	protected CancellationTokenSource m_cancellationTokenSource;
	// Bボタンとかで（プレイヤーの意思で）キャンセルされるやつ
	protected CancellationToken playerCancellationToken;

	protected CancellationToken gameManagerCancellationToken;


}