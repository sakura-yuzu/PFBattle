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
	public CreatureSetting.AttributeType attributeType = CreatureSetting.AttributeType.None;

	protected virtual void Start()
	{
		hp = setting.hp;
		mp = setting.mp;
		attackPower = setting.attackPower;
		defensePower = setting.defensePower;
		speed = setting.speed;
		attributeType = setting.attributeType;
	}

	public async UniTask<int> Damaged(int damage)
	{
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
	public async UniTask<int> Healed(int heal)
	{
		hp += heal;
		if (anim == null || anim.Equals(null))
		{
			return hp;
		}
		anim?.SetBool("Healed", true);
		await UniTask.Delay(TimeSpan.FromSeconds(1f));
		anim?.SetBool("Healed", false);
		return hp;
	}
	public async UniTask<int> Buffed()
	{
		if (anim == null || anim.Equals(null))
		{
			// TODO: 返すべきはHPではない
			return hp;
		}
		anim?.SetBool("Buffed", true);
		await UniTask.Delay(TimeSpan.FromSeconds(1f));
		anim?.SetBool("Buffed", false);
		// TODO: 返すべきはHPではない
		return hp;
	}
	public async UniTask<int> Debuffed()
	{
		if (anim == null || anim.Equals(null))
		{
			// TODO: 返すべきはHPではない
			return hp;
		}
		anim?.SetBool("Debuffed", true);
		await UniTask.Delay(TimeSpan.FromSeconds(1f));
		anim?.SetBool("Debuffed", false);
		// TODO: 返すべきはHPではない
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
		if (action.isAttack())
		{
			// 攻撃された時の処理
			this.Damaged(action.damage);
		}
		else if (action.isHeal())
		{
			// 回復された時の処理
			this.Healed(action.heal);
		}
		else if (action.isBuff())
		{
			// バフされた時の処理
			this.Buffed();
		}
		else if (action.isDebuff())
		{
			// デバフされた時の処理
			this.Debuffed();
		}
	}
}