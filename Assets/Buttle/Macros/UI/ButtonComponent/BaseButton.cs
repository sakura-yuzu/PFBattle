using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

class BaseButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler, ICancelHandler
{
	public BackgroundManager backgroundManager;
	public TextMeshProUGUI tmp;

	public GameObject mountedButton;

	void Awake()
	{
		backgroundManager = mountedButton.GetComponent<BackgroundManager>();
	}

	public void setText(string text)
	{
		tmp.text = text;
	}

// イベントハンドラもvirtual化できるのおもろ
	public virtual void OnSelect(BaseEventData e)
	{
		backgroundManager.select();
		// soundManager.Play();
	}

	public void OnDeselect(BaseEventData e)
	{
		backgroundManager.deselect();
	}

	public void OnPointerEnter(PointerEventData e)
	{
		backgroundManager.select();
	}

	public void OnPointerExit(PointerEventData e)
	{
		backgroundManager.deselect();
	}

	public void OnCancel(BaseEventData eventData)
	{
		// 	parentPanel?.Cancel();
	}
}