using System;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Linq;

public class Action
{
	public SkillSetting skill;
	public Item item;
	public List<Creature> enemies;
	public List<Creature> allies;
	public ActionType actionType;
	public Character actioner;

	public int damage;
	public int heal; // TODO: 回復量の計算

	public Action(string actionTypeStr, SkillSetting skill, Item item, List<Creature> enemies, List<Creature> allies)
	{
		actionType = ActionType.Attack; //(ActionType)Enum.Parse(typeof(ActionType), actionTypeStr);
		this.skill = skill;
		this.item = item;
		this.enemies = enemies;
		this.allies = allies;
	}

	public void setActioner(Character character)
	{
		this.actioner = character;
	}

	public async UniTask execute()
	{
		await this.actioner.execute(this);
	}

	public enum ActionType
	{
		Attack,
		Defense,
		Skill,
		Item
	}

	public bool isAttack()
	{
		if (actionType == ActionType.Attack)
		{
			return true;
		}
		if (actionType == ActionType.Skill)
		{
			return skill.isAttack();
		}
		// TODO: アイテム判定
		return false;
	}
	public bool isHeal()
	{
		if (actionType == ActionType.Skill)
		{
			return skill.isHeal();
		}
		// TODO: アイテム判定
		return false;
	}

	public bool isBuff()
	{
		if (actionType == ActionType.Skill)
		{
			return skill.isBuff();
		}
		// TODO: アイテム判定
		return false;
	}

	public bool isDebuff()
	{
		if (actionType == ActionType.Skill)
		{
			return skill.isDebuff();
		}
		// TODO: アイテム判定
		return false;
	}
}