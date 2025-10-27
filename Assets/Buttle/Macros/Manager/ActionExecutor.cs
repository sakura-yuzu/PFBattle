using UnityEngine;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
class ActionExecutor
{
  public async UniTask execute(Action action)
  {
    Debug.Log("Executing action: " + action.actionType);
    switch (action.actionType)
    {
      case Action.ActionType.Attack:
      Debug.Log("Performing attack action");
        int damage = 100; // TODO: ダメージ計算
        foreach (Creature enemy in action.enemies)
        {
          if (enemy != null)
          {
            await enemy.Damaged(damage);
          }
        }
        break;
      case Action.ActionType.Defense:
        break;
      case Action.ActionType.Skill:
        skill(action);
        break;
      case Action.ActionType.Item:
        break;
    }
  }
  
  public void skill(Action action)
  {
    switch (action.skill.skillType)
    {
      case SkillSetting.SkillType.Heal:
        int healAmount = 100; // TODO: 回復量計算
        action.allies.ForEach(ally => ally.Healed(healAmount));
        break;
      case SkillSetting.SkillType.Buff:
        break;
      case SkillSetting.SkillType.Debuff:
        break;
      case SkillSetting.SkillType.Attack:
        int damage = 150; // TODO: ダメージ計算
        action.enemies.ForEach(enemy => enemy.Damaged(damage));
        break;
    }
  }
}