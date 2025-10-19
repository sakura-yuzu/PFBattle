using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using WeaponTypeEnum;

[CreateAssetMenu]
[SerializeField]
public class ItemSetting : ScriptableObject
{
    public int id;                                      //ID
    public string itemName;
    public string description;
    public AttributeType attributeType;                           // 属性
    public string animationName;                        // 実行時に再生するアニメーション
    public ItemType itemType;
    public TargetType targetType;

    public enum AttributeType
    {
        None,                                            // なし
        Fire,                                            // 炎
        Water,                                           // 水
        Air,                                             // 風
        Earth,                                           // 土
    }

    public enum ItemType
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
}