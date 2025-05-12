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
    [SerializeField] bool drawable;
    private void Start()
    {
        StartCoroutine(InitializeAfterSceneLoad());
        player = GameObject.Find("Player");

    }

    private IEnumerator InitializeAfterSceneLoad()
    {

        yield return new WaitForEndOfFrame();

    }
    public override void Use()
    {
        ItemData item = new ItemData(itemName, itemID, icon, this);
        InventoryUI.instance.SelectItem(item);
        player.GetComponent<ClickToMove>().doingPuzzle = true;
        if(drawable) CursorGame.instance.DrawCursor();
    }

    public void closePuzzle()
    {
        paper.SetActive(false);
        player.GetComponent<ClickToMove>().doingPuzzle = false;
        CursorGame.instance.ResetDrawCursor();
        InventoryUI.instance.abriuPapel = false;
    }

   
}
