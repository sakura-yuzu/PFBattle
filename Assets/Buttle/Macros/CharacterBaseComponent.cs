using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Cysharp.Threading.Tasks;


[SerializeField]
public class CharacterBaseComponent : MonoBehaviour
{

	public CharacterBase characterData;
	public string displayName;
	public int hp;
	public int mp;
	public int attackPower;
	public int defensePower;

	public int speed;

	public Animator anim;

	void Start(){
		displayName = characterData.displayName;
		hp = characterData.hp;
		mp = characterData.mp;
		attackPower = characterData.attackPower;
		defensePower = characterData.defensePower;
		speed = characterData.speed;
	}

	public async UniTask<int> Damaged(int damage){
		hp -= damage;
		anim?.SetBool("Damaged", true);
		await UniTask.Delay(TimeSpan.FromSeconds(1f));
		anim?.SetBool("Damaged", false);
		return hp;
	}

  public async UniTask Death(){
		anim?.SetBool("Death", true);
		await UniTask.Delay(TimeSpan.FromSeconds(1f));
		anim?.SetBool("Death", false);
		Destroy(gameObject);
	}
}