using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System;

class Enemy : Creature, IEnemyAttack
{
	protected override void Start()
	{
		base.Start();
	}
	public void Attack(Creature target)
	{}
}