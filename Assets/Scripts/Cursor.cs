using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    public static Cursor instance;

    public GameObject cursorObject;
    public Sprite cursorPrincipalSprite;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        cursorObject.GetComponent<Image>().sprite = cursorPrincipalSprite;
    }

    // Update is called once per frame
    void Update()
    {
        cursorObject.transform.position = Input.mousePosition;
    }

    public void resetCursor()
    {
        InteractionManagar.instance.selectedItem = null;
        cursorObject.GetComponent<Image>().sprite = cursorPrincipalSprite;
    }
}
