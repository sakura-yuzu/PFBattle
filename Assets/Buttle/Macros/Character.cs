using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
class Character : Creature
{
  public List<Skill> skills;

	public async UniTask<int> Damaged(int damage){
		hp -= damage;
		return hp;
	}
  public async UniTask Death(){
		Destroy(gameObject);
	}
}