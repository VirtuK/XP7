using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[System.Serializable]
public class InteractionManagar : MonoBehaviour
{
    [SerializeField] public static InteractionManagar instance;
    [SerializeField] private RectTransform interactionsParent;
    [SerializeField] private Canvas canvas;
    [SerializeField] private List<GameObject> interactions;
    [SerializeField] private Item interacted;
    [SerializeField] public bool interacting;

    public ItemData selectedItem ;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(InitializeAfterSceneLoad());
    }

    private IEnumerator InitializeAfterSceneLoad()
    {
        yield return new WaitForEndOfFrame();
        canvas = FindAnyObjectByType<Canvas>();
        interactionsParent = GameObject.Find("Interactions").gameObject.GetComponent<RectTransform>();
        GameObject pick = GameObject.Find("Pick");
        GameObject see = GameObject.Find("See");
        GameObject use = GameObject.Find("Use");
        interactions[0] = pick;
        interactions[1] = see;
        interactions[2] = use;
        pick.GetComponent<Button>().onClick.AddListener(() => Pick());
        see.GetComponent<Button>().onClick.AddListener(() => See());
        use.GetComponent<Button>().onClick.AddListener(() => Use());
        foreach (GameObject i in interactions)
        {
            i.SetActive(false);
        }
    }
    public void CheckInteractions (Item item, Vector3 position)
    {
        resetInteractions();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
            position,
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
