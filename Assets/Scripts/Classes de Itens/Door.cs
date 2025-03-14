using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Item
{
    public override void Use()
    {
        if (InteractionManagar.instance.selectedItem != null && InteractionManagar.instance.selectedItem.itemName == "Key")
        {
            print("a porta abriu");
            InventoryManager.instance.RemoveItem(InteractionManagar.instance.selectedItem); 
        }
        else
        {
            print("você precisa de uma chave");
        }
        Cursor.instance.resetCursor();
    }
}
