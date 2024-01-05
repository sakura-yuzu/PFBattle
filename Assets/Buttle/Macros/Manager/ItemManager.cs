using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ItemManager : MonoBehaviour
{
    protected ItemDatabase itemDatabase;

    public ItemManager(ItemDatabase _itemDatabase){
        itemDatabase = _itemDatabase;
    }

    public void AddItemData(Item item)
    {
        itemDatabase.itemList.Add(item);
    }

    public List<Item> getItemListWithConditions(Dictionary<string, string> conditions)
    {
        return itemDatabase.itemList;
    }

    public List<Item> getAll()
    {
        return itemDatabase.itemList;
    }
}
