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
    [SerializeField] public Sprite cursorNormalSprite;
    [SerializeField] public Sprite cursorInteractionSprite;
    [SerializeField] public Sprite cursorDrawSprite;
    [SerializeField] public Sprite cursorClickSprite;
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

            RectTransform rt = cursorObject.GetComponent<RectTransform>();

            if (cursorImage.sprite == cursorDrawSprite)
            {
                // Get size of the cursor image in screen space
                Vector2 size = rt.sizeDelta * rt.lossyScale;

                // Offset to align bottom-left corner with mouse
                Vector2 offset = new Vector2(size.x / 2, size.y / 2);
                cursorObject.transform.position = mousePosition + offset;
            }
            else if (cursorImage.sprite == cursorInteractionSprite || cursorImage.sprite == cursorClickSprite)
            {
                // Get size of the cursor image in screen space
                Vector2 size = rt.sizeDelta * rt.lossyScale;

                // Offset to align center with mouse
                Vector2 offset = new Vector2(0, 0);
                cursorObject.transform.position = mousePosition + offset;
            }
            else
            {
                // Align top-left corner with mouse position
                Vector2 size = rt.sizeDelta * rt.lossyScale;
                Vector2 offset = new Vector2(size.x / 2, -size.y / 2);
                cursorObject.transform.position = mousePosition + offset;
            }

            if (Input.GetMouseButtonDown(0) && cursorObject.GetComponent<Image>().sprite != cursorDrawSprite) 
            {
                cursorImage.sprite = cursorClickSprite;
                StartCoroutine(click());
            }
        }
    }

    public void resetCursor()
    {
        InteractionManagar.instance.selectedItem = null;
        cursorObject.GetComponent<Image>().sprite = cursorPrincipalSprite;
    }

    public void InteractCursor()
    {
        cursorAnimator.SetBool("Door", false);
        cursorAnimator.enabled = false;
        cursorObject.GetComponent<Image>().sprite = cursorInteractionSprite;
        print("detectando intera��o");
        if(cursorObject.GetComponent<Image>().sprite = cursorInteractionSprite)
        {
            print("mudou icone");
        }
        cursorObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
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
        cursorAnimator.enabled = true;
    }

    public void PrincipalCursor()
    {
        cursorObject.GetComponent<Image>().sprite = cursorNormalSprite;
    }

    public void ResetDoorCursor()
    {
        cursorAnimator.SetBool("Door", false);
        cursorAnimator.enabled = false;
        cursorObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

    }

    public IEnumerator click()
    {
        cursorObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.3f);
        ResetInteractCursor();
    }
}
