using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Componente : Item
{
    [SerializeField] public string ID;
    public override void Pick()
    {
        InventoryManager.instance.AddItem(this);
    }
}
