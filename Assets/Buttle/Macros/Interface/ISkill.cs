using System.Collections;
using System.Collections.Generic;
public interface ISkill
{
	// 誰が誰にスキルを使うかを引数で受け取る
	public void Effect(Creature user, List<Creature> target);
}