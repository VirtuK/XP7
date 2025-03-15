using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Item
{
    [SerializeField] private bool isLocked;
    [SerializeField, ConditionalHide("isLocked")] private string keyItemName;
    public override void Use()
    {
        if (isLocked)
        {
            if (InteractionManagar.instance.selectedItem != null && InteractionManagar.instance.selectedItem.itemName == keyItemName)
            {
                print("a porta abriu");
                InventoryManager.instance.RemoveItem(InteractionManagar.instance.selectedItem);
            }
            else
            {
                print("você precisa de: " + keyItemName);
            }
            CursorGame.instance.resetCursor();
        }
        else
        {
            print("essa porta não tem tranca");
        }
    }
}
