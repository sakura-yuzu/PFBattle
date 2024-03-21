using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[SerializeField]
public class CharacterDatabase : ScriptableObject
{
    public List<Character> characterList = new List<Character>();
}
