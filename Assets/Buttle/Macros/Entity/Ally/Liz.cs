using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
class Liz : Character
{
	private Skill fire;

	void Start()
	{
		// TODO
		fire = new Skill();
	}

	public async UniTaskVoid execute(Action action)
	{
		switch (action.actionType)
		{
			case Action.ActionType.Attack:
				break;
			case Action.ActionType.Defense:
				break;
			case Action.ActionType.Skill:
				fire.Effect(anim);
				break;
			case Action.ActionType.Item:
				break;
		}
	}

	public void Fire()
	{
		fire.Effect(anim);
	}
}