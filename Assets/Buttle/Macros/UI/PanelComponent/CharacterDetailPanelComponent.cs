using UnityEngine;
using TMPro;
class CharacterDetailPanelComponent : MonoBehaviour
{
	private Creature character;

	public TextMeshProUGUI characterName;

	public void setCharacter(Creature _character){
		character = _character;
		characterName.text = character.displayName;
	}
	public Creature getCharacter(){
		return character;
	}
}