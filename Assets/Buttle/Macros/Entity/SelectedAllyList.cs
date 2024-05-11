using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Sprite;
// using WeaponTypeEnum;

[CreateAssetMenu]
[SerializeField]
public class SelectedAllyList : ScriptableObject
{
    public List<Creature> selectedCharacterList;
}