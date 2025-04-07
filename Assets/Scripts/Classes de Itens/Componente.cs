using System.Collections;
using System.Collections.Generic;
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
                tranca.setComponentMaterial(this);
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
}
