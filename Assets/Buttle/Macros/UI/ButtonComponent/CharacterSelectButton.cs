using UnityEngine;
using UnityEngine.UI;

class CharacterSelectButton : BaseButton
{
	public Character character;
	public GameObject button;

	public Slider HPBar;
	public Slider MPBar;

	public async void Prepare(){
			backgroundManager = new BackgroundManager(character.selectedImage, character.deselectedImage, button);
	}
}