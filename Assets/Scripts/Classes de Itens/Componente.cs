using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Componente : Item
{
    public override void Pick()
    {
        InventoryManager.instance.AddItem(this);
    }
}
