using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
[SerializeField]
public class ItemDatabase : ScriptableObject
{
    public List<Item> itemList = new List<Item>();
}
