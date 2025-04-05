using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private GameObject puzzle;
    [SerializeField] public GameObject[] slots;
    [SerializeField] public int[] correctSymbols;
    [SerializeField] private string doorName; // Store Door name instead of reference
    public Door door; // Runtime reference (not serialized)


    private void Start()
    {
        puzzle = GameObject.Find("Icon Match Puzzle");
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
    }
    public void CheckSolution()
    {
        bool isSolved = true;
        for (int i = 0; i < slots.Length; i++)
        {
            Transform stone = slots[i].transform.GetChild(0);
            if (stone == null || stone.GetComponent<DraggableStone>().actualSide != correctSymbols[i])
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
        InventoryUI.instance.OpenUI();
        InteractionManagar.instance.interacting = false;
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
            print("Porta não encontrada");
        }
    }
}