using UnityEngine;
using UnityEngine.UI;

class CharacterSelectButton : BaseButton
{
	public AllyComponent character;
	public GameObject button;

	public Slider HPBar;
	public Slider MPBar;

	public async void Prepare(){
		HPBar.maxValue = character.characterData.hp;
		HPBar.value = character.characterData.hp;
			// backgroundManager = new BackgroundManager(character.characterData.selectedImage, character.characterData.deselectedImage, button);
	}

	public void updateHp(int hp){
		HPBar.value = hp;
	}

	public void Death(){
		Destroy(gameObject);
	}
}