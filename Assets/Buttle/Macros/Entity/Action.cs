using System;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Linq;

public class Action
{
	public string skill;
	public string item;
	public List<string> enemies;
	public List<string> allies;
	public ActionType actionType;
	public Character actioner;

	public Action(string actionTypeStr, string skill, string item, List<string> enemies, List<string> allies)
	{
		Debug.Log("actionTypeStr: " + actionTypeStr);
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
		// TODO: スキル側の判定が違う
		return actionType == ActionType.Attack || (actionType == ActionType.Skill && skill == "Attack");
	}
}