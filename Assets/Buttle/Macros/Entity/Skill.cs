using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;
using Cysharp.Threading.Tasks;
public class Skill : MonoBehaviour, ISkill
{
  public SkillSetting setting;

	public async UniTaskVoid Effect(Animator anim)
	{
		Debug.Log(setting.skillName);
		anim?.SetBool(setting.animationName, true);
		await UniTask.Delay(TimeSpan.FromSeconds(1f));
		anim?.SetBool(setting.animationName, false);
	}
}