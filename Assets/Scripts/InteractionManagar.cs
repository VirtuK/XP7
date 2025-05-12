using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InteractionManagar : MonoBehaviour
{
    public static InteractionManagar instance;

    [SerializeField] private RectTransform interactionsParent;
    [SerializeField] private Canvas canvas;
    [SerializeField] private List<InteractionSegment> interactionSegments;
    [SerializeField] private Item interactedItem;
    [SerializeField] public Item highlightedItem;
    [SerializeField] public GameObject interactionCircle;
    public bool interacting;
    private bool circleExist;

    public ItemData selectedItem;

    private bool isDragging;
    public bool haveItemSelected;
    public bool isNear;
    private Vector2 dragStartPos;

    private Vector3 pos;

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
        interactionCircle = GameObject.Find("InteractionCircle");
        interactionCircle.SetActive(false);
        StartCoroutine(InitializeAfterSceneLoad());
    }

    private IEnumerator InitializeAfterSceneLoad()
    {
        yield return new WaitForEndOfFrame();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        interactionsParent = GameObject.Find("Interactions").GetComponent<RectTransform>();

        // Initialize interaction segments
        interactionSegments = new List<InteractionSegment>(interactionsParent.GetComponentsInChildren<InteractionSegment>());

        foreach (var segment in interactionSegments)
        {
            segment.Initialize();
        }

        resetInteractions();
    }

    public void SaveInteractions(Item item, Vector3 position)
    {
        interactedItem = item;
        pos = position;
    }


    public void CheckInteractions()
    {
        resetInteractions();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            pos,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        interactionsParent.anchoredPosition = localPoint;
        interacting = false;

        foreach (var segment in interactionSegments)
        {
            bool supported = (interactedItem.interactions & segment.type) != 0;
            segment.gameObject.SetActive(supported);
            if (supported)
            {
                interacting = true;
            }
        }

        if (interacting)
        {
            interactionsParent.gameObject.SetActive(true);
            isDragging = true;
            dragStartPos = Input.mousePosition;
        }
    }

    private void Update()
    {
        if (interactionCircle.activeInHierarchy)
        {
            Image cursorImage = interactionCircle.GetComponent<Image>();
            Vector2 mousePosition = Input.mousePosition;

            RectTransform rt = interactionCircle.GetComponent<RectTransform>();

            Vector2 size = rt.sizeDelta * rt.lossyScale;

            // Offset to align center with mouse
            Vector2 offset = new Vector2(0, 0);
            interactionCircle.transform.position = mousePosition + offset;
        }
        // Only process interactions if the mouse button is held down
        if (Input.GetMouseButton(0)) // Mouse button is held down
        {
            if(highlightedItem != null)
            {
                if (!circleExist && highlightedItem.GetComponent<Item>())
                {
                    interactionCircle.SetActive(true);
                    circleExist = true;
                }
            }
            if (isDragging && interacting)
            {
                Vector2 currentMousePos = Input.mousePosition;
                Vector2 direction = currentMousePos - dragStartPos;

                if (direction.sqrMagnitude > 0.001f)
                {
                    // Calculate angle from Vector2.up, going clockwise
                    float angle = Vector2.SignedAngle(Vector2.up, direction);
                    if (angle < 0) angle += 360f;

                    foreach (var segment in interactionSegments)
                    {
                        segment.HighlightIfInAngle(angle);
                    }
                }
            }
        }

        // Check for mouse button release to trigger the interaction
        if (Input.GetMouseButtonUp(0))
        {
            if (circleExist) interactionCircle.SetActive(false);
            circleExist = false;
            if (isDragging && interacting)
            {
                foreach (var segment in interactionSegments)
                {
                    if (segment.IsHighlighted && interactedItem.interactions.HasFlag(segment.type))
                    {
                        segment.Trigger(interactedItem);
                        break;
                    }
                }
                resetInteractions();
            }
           
        }
        

        // Hide the interaction wheel if mouse is not held down
        if (!Input.GetMouseButton(0) && interacting)
        {
            resetInteractions();
        }
    }
    public void resetInteractions()
    {
        interactionsParent.transform.localScale = Vector3.zero;
        interactionsParent.gameObject.SetActive(false);
        interacting = false;
        isDragging = false;

        foreach (var segment in interactionSegments)
        {
            segment.Unhighlight();
            segment.gameObject.SetActive(false);
        }
    }
}