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
    [SerializeField] Door portaFinal;
    [SerializeField] TMP_InputField input;
    [SerializeField] bool abriu;
    [SerializeField] Animator animator;

    private void Start()
    {
        StartCoroutine(InitializeAfterSceneLoad());
    }

    private IEnumerator InitializeAfterSceneLoad()
    {
       
        yield return new WaitForEndOfFrame();
        
        if (abriu)
        {
            GetComponent<BoxCollider>().enabled = false;
            portaFinal.puzzleSolved = true;
        }
        numberPuzzle = GameObject.Find("NumberPuzzle");
        player = GameObject.Find("Player");
        input = GameObject.Find("InputField (TMP)").GetComponent<TMP_InputField>();
        portaFinal = GameObject.Find("PortaSaida").GetComponent<Door>();

        //puzzleCam = GameObject.Find("PuzzleCamera");
        //puzzleCam.SetActive(false);
        numberPuzzle.SetActive(false);
    }

    public override void Use()
    {
        if (!abriu)
        {
            input.text = "";
            numberPuzzle.SetActive(true);
            CursorGame.instance.resetCursor();
            CursorGame.instance.ResetDoorCursor();
            InteractionManagar.instance.interacting = true;
            player.GetComponent<ClickToMove>().doingPuzzle = true;
            player.GetComponent<HighlightItens>().ResetHighlight();
        }
    }

    public void ClosePuzzle()
    {
        numberPuzzle.SetActive(false);
        player.GetComponent<ClickToMove>().doingPuzzle = false;
    }

    public void InsertInput()
    {
        string i = input.text;
        if(i == "2501")
        {
            abriu = true;
            GetComponent<BoxCollider>().enabled = false;
            portaFinal.turnOnDisplay();
            portaFinal.puzzleSolved = true;
            ClosePuzzle();
            
        }
    }
}
