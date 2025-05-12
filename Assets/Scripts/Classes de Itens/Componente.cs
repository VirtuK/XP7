using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[System.Serializable]
public class Componente : Item
{
    [SerializeField] public int ID;

    private void Start()
    {
        Tranca tranca = GameObject.Find("TrancaDormitorios").GetComponent<Tranca>();
        if(tranca != null)
        {
            if (!tranca.GetRequiredComponents().Contains(itemName))
            {
            }
        }
    }
    public override void Pick()
    {
        ItemData item = new ItemData(itemName, itemID, icon, this);
        InventoryManager.instance.AddItem(item);
        ListDestruction();
        gameObject.SetActive(false);
    }

   public override void Use()
    {
        Inspect();
    }
}
