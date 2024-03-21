using UnityEngine;
using TMPro;
class CharacterDetailPanelComponent : MonoBehaviour
{
	private Character character;

	public TextMeshProUGUI characterName;

	public void setCharacter(Character _character){
		character = _character;
		characterName.text = character.displayName;
	}
	public Character getCharacter(){
		return character;
	}
}