using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InteractionManagar : MonoBehaviour
{
    public static InteractionManagar instance;
    private RectTransform interactionsParent;
    private Canvas canvas;
    [SerializeField] private List<GameObject> interactions;   
    private Item interacted;
    public bool interacting;

    public Item selectedItem ;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        canvas = FindAnyObjectByType<Canvas>();
        interactionsParent = GameObject.Find("Interactions").gameObject.GetComponent<RectTransform>();
    }

    public void CheckInteractions (Item item)
    {
        resetInteractions();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out Vector2 localPoint);
        interactionsParent.anchoredPosition = localPoint;
        interacted = item;
        if ((item.interactions & InteractionType.Pick) != 0)
        {
            interactions[0].SetActive(true);
            interacting = true;
        }
        if ((item.interactions & InteractionType.See) != 0)
        {
            interactions[1].SetActive(true);
            interacting = true;
        }
        if ((item.interactions & InteractionType.Use) != 0)
        {
            interactions[2].SetActive(true);
            interacting = true;
        }



    }

    public void resetInteractions()
    {
        for(int i = 0; i < interactions.Count; i++)
        {
            interactions[i].SetActive(false);
        }
        interacting = false;
    }

    public void Use()
    {
        interacted.Use();
        resetInteractions();
    }

    public void See()
    {
        interacted.See();
        resetInteractions();
    }

    public void Pick()
    {
        interacted.Pick();
        resetInteractions();
    }
}
