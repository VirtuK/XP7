using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Key : Item
{

    public override void Pick()
    {
        InventoryManager.instance.AddItem(this);
    }
}
