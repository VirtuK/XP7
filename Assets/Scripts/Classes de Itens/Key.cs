using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Key : Item
{

    public override void Pick()
    {
        ItemData item = new ItemData(itemName, itemID, icon, this);
        InventoryManager.instance.AddItem(item);
    }
}
