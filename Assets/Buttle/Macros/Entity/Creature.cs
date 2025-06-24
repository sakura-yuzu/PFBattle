using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using System.Threading;
using Cysharp.Threading.Tasks;
public class Creature : MonoBehaviour
{
	public CreatureSetting setting;
	public List<SkillSetting> skills;
	public string displayName;
	public int hp;
	public int mp;
	public int attackPower;
	public int defensePower;
	public int speed;
	public string prefabAddress;
	public string description;
	public Animator anim;

	protected virtual void Start()
	{
		Debug.Log("Start");
		// displayName = setting.displayName;
		hp = setting.hp;
		mp = setting.mp;
		attackPower = setting.attackPower;
		defensePower = setting.defensePower;
		speed = setting.speed;
		// Debug.Log("displayName: " + displayName);
		// Debug.Log("hp: " + hp);
		// Debug.Log("mp: " + mp);
		// Debug.Log("attackPower: " + attackPower);
		// Debug.Log("defensePower: " + defensePower);
		// Debug.Log("speed: " + speed);
	}

	public async UniTask<int> Damaged(Action action)
	{
		Debug.Log("Damaged");
		int damage = 100 * action.actioner.attackPower / defensePower;
		hp -= damage;
		if (anim == null || anim.Equals(null))
		{
			return hp;
		}
		anim?.SetBool("Damaged", true);
		await UniTask.Delay(TimeSpan.FromSeconds(1f));
		anim?.SetBool("Damaged", false);
		return hp;
	}

	public async UniTask Death()
	{
		if (anim == null || anim.Equals(null))
		{
			return;
		}
		anim?.SetBool("Death", true);
		await UniTask.Delay(TimeSpan.FromSeconds(1f));
		anim?.SetBool("Death", false);

		anim = null;
		Destroy(gameObject);
	}

	public async UniTask execute(Action action)
	{
		if (anim == null || anim.Equals(null))
		{
			return;
		}
		Debug.Log(anim);
		// switch (action.actionType)
		// {
		// 	case Action.ActionType.Attack:
		// 		anim?.SetBool("Attack", true);
		// 		await UniTask.Delay(TimeSpan.FromSeconds(1f));
		// 		anim?.SetBool("Attack", false);
		// 		break;
		// 	case Action.ActionType.Defense:
		// 		anim?.SetBool("Defense", true);
		// 		await UniTask.Delay(TimeSpan.FromSeconds(1f));
		// 		anim?.SetBool("Defense", false);
		// 		break;
		// 	case Action.ActionType.Skill:
		// 		SkillSetting skill = skills.Where(skill => skill.skillName.Equals(action.skill)).First();
		// 		anim?.SetBool(skill.skillName, true);
		// 		await UniTask.Delay(TimeSpan.FromSeconds(1f));
		// 		anim?.SetBool(skill.skillName, false);
		// 		break;
		// 	case Action.ActionType.Item:
		// 		anim?.SetBool("Item", true);
		// 		await UniTask.Delay(TimeSpan.FromSeconds(1f));
		// 		anim?.SetBool("Item", false);
		// 		break;
		// }
	}

	public void ReactToAction(Action action)
	{
		// ここでアクションに対するリアクションを書く
		Debug.Log("ReactToAction");
		Debug.Log(this);
		if (action.isAttack())
		{
			// 攻撃された時の処理
			this.Damaged(action);
		}
	}
}