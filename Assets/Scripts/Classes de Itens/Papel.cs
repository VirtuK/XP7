using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Papel : Item
{
    [SerializeField] GameObject paper;
    [SerializeField] GameObject player;
    [SerializeField] bool drawable;
    private void Start()
    {
        StartCoroutine(InitializeAfterSceneLoad());
        player = GameObject.Find("Player");
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
}
