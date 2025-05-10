using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ItemData itemData;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        InteractionManagar.instance.selectedItem = itemData;
        InteractionManagar.instance.haveItemSelected = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePosition = eventData.position;

        // Set the position of the RectTransform to match the mouse position
        rectTransform.position = mousePosition;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        InventoryUI.instance.OrganizeUI();

        if (InteractionManagar.instance.haveItemSelected)
        {
            print("bbbbbbbbbba");
            if (InteractionManagar.instance.highlightedItem.interactions.HasFlag(InteractionType.Use))
            {
                print("aaaaaaaaaab");
                InteractionManagar.instance.highlightedItem.Use();
                InteractionManagar.instance.highlightedItem = null;
            }
        }
        InteractionManagar.instance.haveItemSelected = false;
        InteractionManagar.instance.selectedItem = null;
    }
}