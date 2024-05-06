using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
class AllyComponent : CharacterBaseComponent
{
  public List<Skill> skills;
	public CharacterSelectButton characterButton;

	public void setCharacterButton(CharacterSelectButton _characterButton){
		characterButton = _characterButton;
	}

	public async UniTask<int> Damaged(int damage){
		hp -= damage;
		return hp;
	}
  public async UniTask Death(){
		Destroy(gameObject);
	}
}