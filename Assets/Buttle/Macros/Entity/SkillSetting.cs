using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using WeaponTypeEnum;

[CreateAssetMenu]
[SerializeField]
public class SkillSetting : ScriptableObject
{
    public int id;                                      //ID
    public string skillName;
    public string description;
    public int requiredPoint;                           // 必要MP
    // public int target;                                  // 効果対象　自分、仲間一人、仲間全体、敵一人、敵全体
    public bool acquired;                               // 習得済み
    public int weaponType;                              // このスキルを利用できる武器　あとでenum書く
    public AttributeType attributeType;                           // 属性
    public string animationName;                        // 実行時に再生するアニメーション
    public SkillType skillType;
    public TargetType targetType;
    public enum WeaponEnum
    {
        Rod,                                             // 杖
        Sword,                                           // 剣
        Bow,                                             // 弓
        Spear,                                           // 槍
    }

    public enum AttributeType
    {
        None,                                            // なし
        Fire,                                            // 炎
        Water,                                           // 水
        Air,                                             // 風
        Earth,                                           // 土
    }

    public enum SkillType
    {
        Heal,                                            // 回復
        Buff,                                            // バフ
        Debuff,                                          // デバフ
        Attack                                           // 攻撃
    }

    public enum TargetType
    {
        AllyOne,                                         // 仲間一人
        AllyAll,                                         // 仲間全体
        EnemyOne,                                        // 敵一人
        EnemyAll                                         // 敵全体
    }

    
	public bool isAttack()
	{
		return skillType == SkillType.Attack;
	}
	public bool isHeal()
	{
		return skillType == SkillType.Heal;
	}

	public bool isBuff()
	{
		return skillType == SkillType.Buff;
	}

	public bool isDebuff()
	{
		return skillType == SkillType.Debuff;
	}
}