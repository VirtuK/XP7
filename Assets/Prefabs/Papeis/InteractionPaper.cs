using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionPaper : MonoBehaviour
{
    [SerializeField] GameObject puzzle;
    [SerializeField] GameObject page1Button;
    [SerializeField] GameObject page2Button;
    [SerializeField] TMP_Text text;
    [SerializeField] string page1;
    [SerializeField] string page2;
    // Start is called before the first frame update
    void Start()
    {
        if (page1Button != null)
        {
            text.text = page1;
        }
    }

    public void turnPage()
    {
        if (text.text == page1)
        {
            text.text = page2;
            page1Button.SetActive(false);
            page2Button.SetActive(true);
        }
        else
        {
            text.text = page1;
            page1Button.SetActive(true);
            page2Button.SetActive(false);
        }
    }

    public void closePuzzle()
    {

        GameObject.Find("Player").GetComponent<ClickToMove>().doingPuzzle = false;
        CursorGame.instance.ResetDrawCursor();
        InteractionManagar.instance.selectedItem = null;
        Destroy(puzzle);
    }
}
