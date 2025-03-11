using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Item
{
    public override void Use()
    {
        if (InventoryManager.instance.CheckItem("Key"))
        {
            print("a porta abriu");
        }
        else
        {
            print("você precisa de uma chave");
        }
    }
}
