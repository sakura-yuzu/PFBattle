using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;
using Cysharp.Threading.Tasks;
public class Creature : MonoBehaviour
{
  CreatureSetting setting;
  public string displayName;
  public int hp;
  public int mp;
  public int attackPower;
  public int defensePower;
  public int speed;
  public Sprite deselectedImage;
  public string prefabAddress;
  public string description;
  public Animator anim;

	void Start(){
		displayName = setting.displayName;
		hp = setting.hp;
		mp = setting.mp;
		attackPower = setting.attackPower;
		defensePower = setting.defensePower;
		speed = setting.speed;
    prefabAddress = setting.prefabAddress;
    anim = setting.anim;
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