using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface ISkill
{
	// 誰が誰にスキルを使うかを引数で受け取る
	public UniTaskVoid Effect(Animator anim);
}