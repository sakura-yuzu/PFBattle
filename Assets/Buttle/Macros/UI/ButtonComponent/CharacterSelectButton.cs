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
		backgroundManager.setData(
			character.characterData.selectedImage,
			character.characterData.deselectedImage,
			button
		);
		setText(character.displayName);
	}

	public void updateHp(int hp){
		HPBar.value = hp;
	}

	public void Death(){
		Destroy(gameObject);
	}
}