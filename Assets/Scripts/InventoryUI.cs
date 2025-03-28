using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject Inventory;
    [SerializeField] private GameObject ItemPrefab;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject OpenButton;
    [SerializeField] private CursorGame cursor;
    [SerializeField] private List<GameObject> itens;
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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        canvasGroup = GameObject.Find("Canvas").GetComponent<CanvasGroup>();
        Inventory = GameObject.Find("Itens");
        HUD = GameObject.Find("Inventory");
        OpenButton = GameObject.Find("OpenInventory");
        OpenButton.GetComponent<Button>().onClick.AddListener(() => OpenUI());
        GameObject CloseButton = GameObject.Find("Close Button");
        CloseButton.GetComponent<Button>().onClick.AddListener(() => CloseUI());

        canvasGroup.alpha = 1;
        HUD.SetActive(false);
    }
    public void CreateItemUI(Item item)
    {
        print("criei");
        
        GameObject itemSlot = Instantiate(ItemPrefab, Inventory.transform);
        itemSlot.GetComponent<Image>().sprite = item.icon;
        itemSlot.AddComponent<Button>();
        Item itemRef = item;
        itemSlot.GetComponent<Button>().onClick.AddListener(() => SelectItem(itemRef));
        itens.Add(itemSlot);
        OrganizeUI();
        
    }

    public void DeleteItemUI(Item item)
    {
        Destroy(itens[InventoryManager.instance.GetInventory().IndexOf(item)]);
        itens.Remove(itens[InventoryManager.instance.GetInventory().IndexOf(item)]);
        OrganizeUI();
    }

    private void OrganizeUI()
    {
        foreach(GameObject itemSlot in itens)
        {
            
            int index = itens.IndexOf(itemSlot);
            float startX = -321;
            float startY = 190;
            float slotSpacingX = 193;
            float slotSpacingY = 246; 

            
            int row = index / 5; 
            int column = index % 5; 

            
            float itemX = startX + (slotSpacingX * column);
            float itemY = startY - (slotSpacingY * row);

            // Set position
            itemSlot.GetComponent<RectTransform>().anchoredPosition = new Vector2(itemX, itemY);
        }
    }

    public void OpenUI()
    {
        HUD.SetActive(true);
        OpenButton.SetActive(false);
        InteractionManagar.instance.resetInteractions();
    }

    public void CloseUI()
    {
        HUD.SetActive(false);
        OpenButton.SetActive(true);
    }

    public void SelectItem(Item item)
    {
        if (item == null)
        {
            Debug.LogError("Item is null when SelectItem() is called!");
            return;
        }

        if (cursor == null)
        {
            Debug.LogError("Cursor is not assigned in InventoryUI!");
            return;
        }

        Image cursorImage = cursor.cursorObject.GetComponent<Image>();
        if (cursorImage == null)
        {
            Debug.LogError("Cursor does not have an Image component!");
            return;
        }

        if (item.icon == null)
        {
            Debug.LogError("Item icon is null for item: " + item.itemName);
            return;
        }

        cursorImage.sprite = item.icon;
        InteractionManagar.instance.selectedItem = item;
        print("Item selected: " + item.itemName);
        CloseUI();
        
    }
}