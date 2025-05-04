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
    [SerializeField] public Sprite cursorDrawSprite;
    [SerializeField] public Animator cursorAnimator;
    
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
            cursorAnimator = cursorObject.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("Cursor not found in scene: " + SceneManager.GetActiveScene().name);
        }
        Cursor.visible = false;
    }


    void Update()
    {
        if (cursorObject != null)
        {
            Image cursorImage = cursorObject.GetComponent<Image>();
            Vector2 mousePosition = Input.mousePosition;

            if (cursorImage.sprite == cursorDrawSprite)
            {
                // Get size of the cursor image in screen space
                RectTransform rt = cursorObject.GetComponent<RectTransform>();
                Vector2 size = rt.sizeDelta * rt.lossyScale;

                // Offset to align bottom-left corner with mouse
                Vector2 offset = new Vector2(size.x / 2, size.y / 2);
                cursorObject.transform.position = mousePosition + offset;
            }
            else
            {
                // Default behavior: center cursor on mouse
                cursorObject.transform.position = mousePosition;
            }
        }
    }

    public void resetCursor()
    {
        InteractionManagar.instance.selectedItem = null;
        cursorObject.GetComponent<Image>().sprite = cursorPrincipalSprite;
        cursorAnimator.SetBool("Door", false);
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

    public void DrawCursor()
    {
        cursorObject.GetComponent<Image>().sprite = cursorDrawSprite;
    }

    public void ResetDrawCursor()
    {
        cursorObject.GetComponent<Image>().sprite = cursorPrincipalSprite;
    }

    public void DoorCursor()
    {
        cursorAnimator.SetBool("Door", true);
    }

    public void ResetDoorCursor()
    {
        cursorObject.GetComponent<Image>().sprite = cursorPrincipalSprite;
        cursorAnimator.SetBool("Door", false);
    }
}
