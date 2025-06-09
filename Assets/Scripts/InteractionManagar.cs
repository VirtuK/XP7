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
    public bool circleExist;

    public ItemData selectedItem;


    private bool isDragging;
    public bool haveItemSelected;
    public bool isNear;
    private bool test;
    private Vector2 dragStartPos;

    private Vector3 pos;

    public float resetCooldown = 0f;
    private float cooldownDuration = 1f;

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
        //resetInteractions();
        highlightedItem = null;
        circleExist = true;
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
                checkCircle();
            }
        }

        if (interacting)
        {
            interactionsParent.gameObject.SetActive(true);
            isDragging = true;
            dragStartPos = Input.mousePosition;
        }
    }

    private void checkCircle()
    {
        if (interactionCircle.activeInHierarchy)
        {
            highlightedItem = null;
            Image cursorImage = interactionCircle.GetComponent<Image>();
            Vector2 mousePosition = Input.mousePosition;

            RectTransform rt = interactionCircle.GetComponent<RectTransform>();

            Vector2 size = rt.sizeDelta * rt.lossyScale;

            // Offset to align center with mouse
            Vector2 offset = new Vector2(0, 0);
            interactionCircle.transform.position = mousePosition + offset;
        }
    }


    private void Update()
    {
        if (resetCooldown > 0f)
        {
            resetCooldown -= Time.deltaTime;
        }
        // Only process interactions if the mouse button is held down
        if (Input.GetMouseButton(0) && !haveItemSelected && !circleExist) // Mouse button is held down
        {
            if(highlightedItem != null)
            {
                /*if (!circleExist && highlightedItem.GetComponent<Item>())
                {
                    interactionCircle.SetActive(true);
                    circleExist = true;
                }*/
                if (isNear && !haveItemSelected && !circleExist)
                {
                    print("teste teste teste");
                }
            }
            /*if (interacting)
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
            }*/
        }

        // Check for mouse button release to trigger the interaction
        if (Input.GetMouseButtonDown(0) && circleExist)
        {
            /*if (circleExist) interactionCircle.SetActive(false);
            circleExist = false;*/
            if (interacting)
            {
                bool haveinteraction = false;
                foreach (var segment in interactionSegments)
                {
                    if (segment.IsHighlighted && interactedItem.interactions.HasFlag(segment.type))
                    {
                        segment.Trigger(interactedItem);
                        haveinteraction = true;
                        resetInteractions();
                        break;
                    }
                    
                }
                if (!haveinteraction)
                {
                    resetInteractions();
                    
                }
                
            }
            GameObject.Find("Player").GetComponent<HighlightItens>().HighlightObject();

        }
        

        // Hide the interaction wheel if mouse is not held down
        if (!Input.GetMouseButton(0) && interacting)
        {
            if (!IsMouseOverItem(highlightedItem))
            {
                //highlightedItem = null;
                //resetInteractions();
            }
        }

        if (isNear && !circleExist && interactedItem != null && !test && resetCooldown <= 0f && !interactedItem.GetComponent<Door>())
        {
            CheckInteractions();
            test = true;
            isNear = false;
        }

        if (interacting)
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
    public void resetInteractions()
    {
        if (interactionsParent != null)
        {
            interactionsParent.transform.localScale = Vector3.zero;
            interactionsParent.gameObject.SetActive(false);
            interacting = false;
            isDragging = false;
            circleExist = false;
            test = false;

            foreach (var segment in interactionSegments)
            {
                segment.Unhighlight();
                segment.gameObject.SetActive(false);
            }

            interactedItem = null; // <- isso aqui resolve o bug!
            resetCooldown = cooldownDuration;
        }
    }

    private bool IsMouseOverItem(Item item)
    {
        if (item == null) return false;

        RectTransform rt = item.GetComponent<RectTransform>();
        if (rt == null) return false;

        Vector2 mousePos = Input.mousePosition;
        print("mouse ta em cima");
        return RectTransformUtility.RectangleContainsScreenPoint(rt, mousePos, canvas.worldCamera);
    }
}