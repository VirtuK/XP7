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

    public bool abriuPapel;

    public bool porta;

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
        porta = false;

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
        GameObject itemSlot = Instantiate(ItemPrefab, Inventory.transform);
        Image itemImage = itemSlot.GetComponent<Image>();
        itemImage.sprite = itemData.itemIcon;
        itemImage.preserveAspect = true;

        // Ensure CanvasGroup component is added first
        CanvasGroup canvasGroup = itemSlot.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = itemSlot.AddComponent<CanvasGroup>();
        }
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Now add DraggableItem
        DraggableItem draggable = itemSlot.AddComponent<DraggableItem>();
        draggable.itemData = itemData;

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

    public void OrganizeUI()
    {
        for (int i = 0; i < itens.Count; i++)
        {
            float startX = 716;
            float startY = 380;
            float slotSpacingX = 0;
            float slotSpacingY = 89;

            int row = i;

            float itemX = startX;
            float itemY = startY - (slotSpacingY * row);

            itens[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(itemX, itemY);
            itens[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
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
        if (!porta)
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


            switch (itemData.itemName)
            {
                case "PapelCofre":
                    if (!abriuPapel)
                    {
                        GameObject pc = Instantiate(papeis[0], canvasGroup.gameObject.transform);
                        pc.name = papeis[0].name;
                        pc.gameObject.transform.SetSiblingIndex(3);
                        CursorGame.instance.DrawCursor();
                        InteractionManagar.instance.selectedItem = itemData;
                        GameObject.Find("Player").GetComponent<ClickToMove>().doingPuzzle = true;
                        abriuPapel = true;
                    }
                    else
                    {
                        GameObject pc = GameObject.Find(papeis[0].name);
                        pc.GetComponentInChildren<UIDraw>().closePuzzle();
                        abriuPapel = false;
                    }
                    break;
                case "PapelQuarto":
                    if (!abriuPapel)
                    {
                        GameObject pq = Instantiate(papeis[1], canvasGroup.gameObject.transform);
                        pq.name = papeis[1].name;
                        pq.gameObject.transform.SetSiblingIndex(3);
                        InteractionManagar.instance.selectedItem = itemData;
                        GameObject.Find("Player").GetComponent<ClickToMove>().doingPuzzle = true;
                        abriuPapel = true;

                    }
                    else
                    {
                        GameObject pq = GameObject.Find(papeis[1].name);
                        pq.GetComponent<InteractionPaper>().closePuzzle();
                        abriuPapel = false;
                    }
                    break;

            }
            //CloseUI();
        }
    }

    public bool isPaper(ItemData itemData)
    {
        switch (itemData.itemName)
        {
            case "PapelCofre":
                return true;
            case "PapelQuarto":
                return true;
        }
        return false;
    }
}
