using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject Inventory;
    [SerializeField] private GameObject ItemPrefab;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject HUD;
    [SerializeField] private GameObject OpenButton;
    [SerializeField] private CursorGame cursor;
    [SerializeField] private List<GameObject> itens = new List<GameObject>();

    [SerializeField] private List<GameObject> papeis = new List<GameObject>();

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
        cursor = GameObject.Find("CursorManager").GetComponent<CursorGame>();
        canvasGroup.alpha = 1;

        // Reload inventory UI after scene change
        RefreshInventoryUI();
    }

    private void RefreshInventoryUI()
    {
        foreach (GameObject itemSlot in itens)
        {
            Destroy(itemSlot);
        }
        itens.Clear();

        foreach (ItemData itemData in InventoryManager.instance.GetInventory())
        {
            CreateItemUI(itemData);
        }
    }

    public void CreateItemUI(ItemData itemData)
    {
        print("Item UI created: " + itemData.itemName);

        GameObject itemSlot = Instantiate(ItemPrefab, Inventory.transform);
        itemSlot.GetComponent<Image>().sprite = itemData.itemIcon;
        itemSlot.AddComponent<Button>();
        itemSlot.GetComponent<Button>().onClick.AddListener(() => SelectItem(itemData));
        itens.Add(itemSlot);
        OrganizeUI();
    }

    public void DeleteItemUI(ItemData itemData)
    {
        int index = InventoryManager.instance.GetInventory().FindIndex(i => i.itemID == itemData.itemID);
        if (index >= 0 && index < itens.Count)
        {
            Destroy(itens[index]);
            itens.RemoveAt(index);
        }
        OrganizeUI();
    }

    private void OrganizeUI()
    {
        for (int i = 0; i < itens.Count; i++)
        {
            float startX = -299;
            float startY = -1.8f;
            float slotSpacingX = 90;
            float slotSpacingY = 78;

            int row = i / 3;
            int column = i % 3;

            float itemX = startX + (slotSpacingX * column);
            float itemY = startY - (slotSpacingY * row);

            itens[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(itemX, itemY);
            itens[i].transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        }
    }

    public void OpenUI()
    {
        HUD.SetActive(true);
        InteractionManagar.instance.resetInteractions();
    }

    public void CloseUI()
    {
        HUD.SetActive(false);
    }

    public void SelectItem(ItemData itemData)
    {
        if (itemData == null)
        {
            Debug.LogError("ItemData is null when SelectItem() is called!");
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

        if (itemData.itemIcon == null)
        {
            Debug.LogError("Item icon is null for item: " + itemData.itemName);
            return;
        }

        if(itemData == InteractionManagar.instance.selectedItem)
        {
            CursorGame.instance.resetCursor();
            foreach (GameObject g in papeis)
            {
                GameObject p = GameObject.Find(g.name);
                if (p != null) Destroy(p);
            }
            InteractionManagar.instance.selectedItem = null;
            return;
        }

        cursorImage.sprite = itemData.itemIcon;
        InteractionManagar.instance.selectedItem = itemData;
        print("Item selected: " + itemData.itemName);


        switch (itemData.itemName)
        {
            case "PapelCofre":
                GameObject pc = Instantiate(papeis[0], canvasGroup.gameObject.transform);
                pc.name = papeis[0].name;
                pc.gameObject.transform.SetSiblingIndex(3);
                CursorGame.instance.resetCursor();
                InteractionManagar.instance.selectedItem = itemData;
                break;
            case "PapelQuarto":
                GameObject pq = Instantiate(papeis[1], canvasGroup.gameObject.transform);
                pq.name = papeis[1].name;
                pq.gameObject.transform.SetSiblingIndex(3);
                CursorGame.instance.resetCursor();
                InteractionManagar.instance.selectedItem = itemData;
                break;
        }
        //CloseUI();
    }
}
