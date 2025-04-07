using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Papel : Item
{
    [SerializeField] GameObject paper;
    [SerializeField] GameObject player;

    private void Start()
    {
        StartCoroutine(InitializeAfterSceneLoad());
    }

    private IEnumerator InitializeAfterSceneLoad()
    {
        yield return new WaitForEndOfFrame();
        player = GameObject.Find("Player");
        paper = GameObject.Find("NumberPuzzle");
        paper.SetActive(false);
    }
    public override void Use()
    {
        paper.SetActive(true);
        player.GetComponent<ClickToMove>().doingPuzzle = true;
    }
}
