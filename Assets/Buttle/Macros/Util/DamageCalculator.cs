using System;
using UnityEngine;

class DamageCalculator
{
  public DamageCalculator() { }

  public int calculate(Creature attacker, Creature defender, SkillSetting skill = null)
  {
    int attackPower = calculateAttackPower(attacker, skill);
    int defensePower = calculateDefensePower(defender);
    bool isCritical = calculateCritical(attacker);
    int affinity = calculateAffinityCoefficient(attacker, defender);
    if(isCritical){
      attackPower = attackPower * 2;
    }
    return attackPower * affinity - defensePower;
  }

  private int calculateAttackPower(Creature attacker, SkillSetting skill)
  {
    // TODO: 装備品の考慮
    return attacker.attackPower;
  }

  private int calculateDefensePower(Creature defender)
  {
    // TODO: 装備品の考慮
    return defender.defensePower;
  }

  private bool calculateCritical(Creature attacker)
  {
    // TODO: 装備品の考慮
    //現在時刻のミリ秒でシード値を初期化
    UnityEngine.Random.InitState(DateTime.Now.Millisecond);
    return UnityEngine.Random.value < 0.05;
  }

  private int calculateAffinityCoefficient(Creature attacker, Creature defender)
  {
    if(defender.attributeType == CreatureSetting.AttributeType.None){
      return 1;
    }
    // TODO: 実装
    return 1;
  }

}