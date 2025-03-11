using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject Inventory;
    [SerializeField] private GameObject ItemPrefab;
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject OpenButton;
    public static InventoryUI instance;
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
    public void CreateItemUI(Item item)
    {
        print("criei");
        int index = InventoryManager.instance.CheckItemIndex(item);
        GameObject itemSlot = Instantiate(ItemPrefab, Inventory.transform);
        

        // Base positions
        float startX = -321;
        float startY = 190;
        float slotSpacingX = 193;
        float slotSpacingY = 246; // Vertical spacing between rows

        // Calculate row and column
        int row = index / 5; // Determines which row the item is in (0, 1, or 2)
        int column = index % 5; // Determines the X position within the row (0-4)

        // Compute positions
        float itemX = startX + (slotSpacingX * column);
        float itemY = startY - (slotSpacingY * row);

        // Set position
        itemSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(itemX, itemY);
        itemSlot.GetComponent<Image>().sprite = item.icon;
    }

    public void OpenUI()
    {
        HUD.SetActive(true);
        OpenButton.SetActive(false);
    }

    public void CloseUI()
    {
        HUD.SetActive(false);
        OpenButton.SetActive(true);
    }
}