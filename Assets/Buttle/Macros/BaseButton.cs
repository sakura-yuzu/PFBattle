using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
class BaseButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler, IPointerExitHandler
{
	public BackgroundManager backgroundManager;

	public void OnSelect(BaseEventData e){
		backgroundManager.select();
		// soundManager.Play();
	}

	public void OnDeselect(BaseEventData e){
		backgroundManager.deselect();
	}

	public void OnPointerEnter(PointerEventData e){
		backgroundManager.select();
	}

	public void OnPointerExit(PointerEventData e){
		backgroundManager.deselect();
	}
}