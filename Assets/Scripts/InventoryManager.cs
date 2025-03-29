using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    [SerializeField] private List<Item> inventory = new List<Item>();

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

    public void AddItem(Item item)
    {
        if (!inventory.Contains(item))
        {
            inventory.Add(item);
            item.gameObject.SetActive(false);
            for (int i = 0; i < inventory.Count; i++)
            {
                print(inventory[i].itemName);
            }
            InventoryUI.instance.CreateItemUI(item);
        }
    }

    public void RemoveItem(Item item)
    {
        if (inventory.Contains(item))
        {
            InventoryUI.instance.DeleteItemUI(item);
            inventory.Remove(item);
        }
    }

    public bool CheckItem(string itemName)
    {
        foreach (Item item in inventory)
        {
            if (item.itemName == itemName)
            {
                return true;
            }
        }
        return false;
    }

    public int CheckItemIndex(Item item)
    {
        int index = inventory.IndexOf(item);
        return index;
    }

    public List<Item> GetInventory()
    {
        return inventory;
    }
}
