using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cofre : Item
{
    [SerializeField] GameObject numberPuzzle;
    [SerializeField] GameObject player;
    [SerializeField] GameObject puzzleCam;
    [SerializeField] TMP_InputField input;
    [SerializeField] bool abriu;
    [SerializeField] Animator animator;

    private void Start()
    {
        StartCoroutine(InitializeAfterSceneLoad());
        if (abriu)
        {
            animator.SetBool("abriu", true);
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private IEnumerator InitializeAfterSceneLoad()
    {
        yield return new WaitForEndOfFrame();
        numberPuzzle = GameObject.Find("NumberPuzzle");
        player = GameObject.Find("Player");
        input = GameObject.Find("InputField (TMP)").GetComponent<TMP_InputField>();
        animator = GameObject.Find("cofreFechado").GetComponent<Animator>();
        puzzleCam = GameObject.Find("PuzzleCamera");
        puzzleCam.SetActive(false);
        numberPuzzle.SetActive(false);
    }

    public override void Use()
    {
        if (!abriu)
        {
            input.text = "";
            numberPuzzle.SetActive(true);
            puzzleCam.SetActive(true);
            player.GetComponent<ClickToMove>().doingPuzzle = true;
        }
    }

    public void ClosePuzzle()
    {
        numberPuzzle.SetActive(false);
        puzzleCam.SetActive(false);
        player.GetComponent<ClickToMove>().doingPuzzle = false;
    }

    public void InsertInput()
    {
        string i = input.text;
        if(i == "2501")
        {
            animator.SetBool("abriu", true);
            abriu = true;
            GetComponent<BoxCollider>().enabled = false;
            ClosePuzzle();
            seeDescription = "Why would they keep this in a safe?";
        }
    }
}
