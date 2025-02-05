using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

class Character : Creature
{

	void Start()
	{
		// TODO
		// fire = new Skill();
	}

	public async UniTaskVoid execute(Action action)
	{
		if (anim == null || anim.Equals(null))
		{
			return;
		}
			Debug.Log(anim);
		switch (action.actionType)
		{
			case Action.ActionType.Attack:
				anim?.SetBool("Attack", true);
				await UniTask.Delay(TimeSpan.FromSeconds(1f));
				anim?.SetBool("Attack", false);
				break;
			case Action.ActionType.Defense:
				anim?.SetBool("Defense", true);
				await UniTask.Delay(TimeSpan.FromSeconds(1f));
				anim?.SetBool("Defense", false);
				break;
			case Action.ActionType.Skill:
				SkillSetting skill = skills.Where(skill => skill.skillName.Equals(action.skill)).First();
				anim?.SetBool(skill.skillName, true);
				await UniTask.Delay(TimeSpan.FromSeconds(1f));
				anim?.SetBool(skill.skillName, false);
				break;
			case Action.ActionType.Item:
				anim?.SetBool("Item", true);
				await UniTask.Delay(TimeSpan.FromSeconds(1f));
				anim?.SetBool("Item", false);
				break;
		}
	}
}
