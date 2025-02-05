using System;
using UnityEngine;

class Action
{
	public string skill;
	public string item;
	public string[] enemy;
	public string[] character;
	public ActionType actionType;
	public Character actioner;

	public Action(string actionTypeStr, string skill, string item, string[] enemy, string[] character)
	{
		actionType = (ActionType)Enum.Parse(typeof(ActionType), actionTypeStr);
		this.skill = skill;
		this.item = item;
		this.enemy = enemy;
		this.character = character;
	}

	public void setActioner(Character character)
	{
		this.actioner = character;
	}

	public void execute()
	{
		actioner.execute(this);
	}

	public enum ActionType
	{
		Attack,
		Defense,
		Skill,
		Item
	}
}