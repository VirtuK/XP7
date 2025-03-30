using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    [SerializeField] private List<ItemData> inventory = new List<ItemData>(); // Store item data instead of Item objects

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddItem(ItemData itemData)
    {
        if (!inventory.Exists(i => i.itemID == itemData.itemID))
        {
            inventory.Add(itemData);
            InventoryUI.instance.CreateItemUI(itemData);
        }
    }

    public void RemoveItem(ItemData itemData)
    {
        if (inventory.Exists(i => i.itemID == itemData.itemID))
        {
            InventoryUI.instance.DeleteItemUI(itemData);
            inventory.RemoveAll(i => i.itemID == itemData.itemID);
        }
    }

    public bool CheckItem(string itemName)
    {
        return inventory.Exists(item => item.itemName == itemName);
    }

    public List<ItemData> GetInventory()
    {
        return inventory;
    }
}

[System.Serializable]
public class ItemData
{
    public string itemName;
    public string itemID;
    public Sprite itemIcon;
    public Item itemComponent; // Reference to the base Item component

    // Constructor to initialize ItemData with metadata and the associated Item component
    public ItemData(string name, string id, Sprite icon, Item itemComp)
    {
        itemName = name;
        itemID = id;
        itemIcon = icon;
        itemComponent = itemComp;
    }

    public override bool Equals(object obj)
    {
        return obj is ItemData data && data.itemID == itemID;
    }

    public override int GetHashCode()
    {
        return itemID.GetHashCode();
    }

    public Item returnComponent()
    {
        return itemComponent;
    }
}
