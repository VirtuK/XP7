using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CursorGame : MonoBehaviour
{
    public static CursorGame instance;

    public GameObject cursorObject;
    public Sprite cursorPrincipalSprite;
    public Sprite cursorInteractionSprite;
    
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
        cursorObject.GetComponent<Image>().sprite = cursorPrincipalSprite;
        Cursor.visible = false;
    }

    
    void Update()
    {
        cursorObject.transform.position = Input.mousePosition;
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
