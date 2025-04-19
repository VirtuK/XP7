using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject puzzle;
    [SerializeField] private GameObject player;
    [SerializeField] public GameObject[] slots;
    [SerializeField] public GameObject puzzleCam;
    [SerializeField] public Sprite[] correctSymbols;
    [SerializeField] private string doorName; // Store Door name instead of reference
    public Door door; // Runtime reference (not serialized)

    public List<Sprite> figures = new List<Sprite>();
    public List<Image> pieces = new List<Image>();
    private int index;
    private void Start()
    {
        puzzleCam = GameObject.Find("PuzzleCamera");
        puzzle = GameObject.Find("Icon Match Puzzle");
        player = GameObject.Find("Player");
        StartCoroutine(InitializeAfterSceneLoad());
    }

    private IEnumerator InitializeAfterSceneLoad()
    {
        yield return new WaitForEndOfFrame();
        if (door != null)
        {
            doorName = door.gameObject.name;
        }
        puzzle.SetActive(false);
        FindDoor();
        index = 0;
        foreach (Image im in pieces)
        {
            RandomizePiece(im);
        }
    }

    public void RandomizePiece(Image im)
    {
        if (figures.Count == 0)
        {
            return;
        }

        int r;
        Sprite selectedSprite;

        do
        {
            r = UnityEngine.Random.Range(0, figures.Count);
            selectedSprite = figures[r];
        }
        while (selectedSprite == correctSymbols[index] && figures.Count > 1);

        im.sprite = selectedSprite;
        figures.RemoveAt(r);
        index++;
    }
    public void CheckSolution()
    {
        bool isSolved = true;
        for (int i = 0; i < slots.Length; i++)
        {
            Transform stone = slots[i].transform.GetChild(0);
            if (stone == null || stone.GetComponent<Image>().sprite != correctSymbols[i])
            {
                isSolved = false;
                break;
            }
        }

        if (isSolved)
        {
            door.isButtonPressed = true;
            door.turnOnDisplay();
            closePuzzle();
        }
        else
        {
            Debug.Log("Incorrect arrangement. Try again.");
        }
    }

    public void closePuzzle()
    {
        puzzle.SetActive(false);
        puzzleCam.SetActive(false);
        InventoryUI.instance.OpenUI();
        InteractionManagar.instance.interacting = false;
        player.GetComponent<ClickToMove>().doingPuzzle = false;
    }

    public void FindDoor()
    {
        GameObject doorObject = GameObject.Find(doorName);
        print("Porta encontrada");
        if (doorObject != null)
        {
            door = doorObject.GetComponent<Door>();
        }
        else
        {
            print("Porta n�o encontrada");
        }
    }
}