using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Papel : Item
{
    [SerializeField] GameObject paper;
    [SerializeField] GameObject player;
    [SerializeField] GameObject page1Button;
    [SerializeField] GameObject page2Button;
    [SerializeField] TMP_Text text;
    [SerializeField] string page1;
    [SerializeField] string page2;
    [SerializeField] bool drawable;
    private void Start()
    {
        StartCoroutine(InitializeAfterSceneLoad());
        player = GameObject.Find("Player");

        if(page1Button != null)
        {
            text.text = page1;
        }
    }

    private IEnumerator InitializeAfterSceneLoad()
    {

        paper.SetActive(false);
        yield return new WaitForEndOfFrame();

    }
    public override void Use()
    {
        paper.SetActive(true);
        player.GetComponent<ClickToMove>().doingPuzzle = true;
        if(drawable) CursorGame.instance.DrawCursor();
    }

    public void closePuzzle()
    {
        paper.SetActive(false);
        player.GetComponent<ClickToMove>().doingPuzzle = false;
        CursorGame.instance.ResetDrawCursor();
    }

    public void turnPage()
    {
        if(text.text == page1)
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
}
