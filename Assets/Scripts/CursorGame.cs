using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[System.Serializable]
public class CursorGame : MonoBehaviour
{
    public static CursorGame instance;

    [SerializeField] public GameObject cursorObject;
    [SerializeField] public Sprite cursorPrincipalSprite;
    [SerializeField] public Sprite cursorInteractionSprite;
    
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
        cursorObject = GameObject.Find("Cursor");
        if (cursorObject != null)
        {
            cursorObject.GetComponent<Image>().sprite = cursorPrincipalSprite;
        }
        else
        {
            Debug.LogError("Cursor not found in scene: " + SceneManager.GetActiveScene().name);
        }
        Cursor.visible = false;
    }

    
    void Update()
    {
        if(cursorObject != null)
        {
            cursorObject.transform.position = Input.mousePosition;
        }
    }

    public void resetCursor()
    {
        InteractionManagar.instance.selectedItem = null;
        cursorObject.GetComponent<Image>().sprite = cursorPrincipalSprite;
    }

    public void InteractCursor()
    {
        if(cursorObject.GetComponent<Image>().sprite == cursorPrincipalSprite)
        {
            cursorObject.GetComponent<Image>().sprite = cursorInteractionSprite;
        }
    }

    public void ResetInteractCursor()
    {
        if (cursorObject.GetComponent<Image>().sprite == cursorInteractionSprite)
        {
            cursorObject.GetComponent<Image>().sprite = cursorPrincipalSprite;
        }
    }
}
