using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;
using Cysharp.Threading.Tasks;
public class Item : MonoBehaviour
{
  public ItemSetting setting;

  public Item()
  {

  }

  public string itemName()
  {
    return setting.itemName;
  }
}