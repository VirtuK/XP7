using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class TrancaIconPuzzle : Item
{
    [SerializeField] private GameObject iconPuzzle;
    [SerializeField] private GameObject player;
    [SerializeField] private string doorName; // Store Door name instead of reference
    public Door door; // Runtime reference (not serialized)


    private void Start()
    {
        iconPuzzle = GameObject.Find("Icon Match Puzzle");
        StartCoroutine(InitializeAfterSceneLoad());
    }

    private IEnumerator InitializeAfterSceneLoad()
    {
        yield return new WaitForEndOfFrame();
        if (door != null)
        {
            doorName = door.gameObject.name;
        }
        iconPuzzle.SetActive(false);
        player = GameObject.Find("Player");
        FindDoor();

        
    }


    public override void Use()
    {
        if (!door.isButtonPressed)
        {
            InventoryUI.instance.CloseUI();
            iconPuzzle.SetActive(true);
            InteractionManagar.instance.interacting = true;
            player.GetComponent<ClickToMove>().doingPuzzle = true;
        }
        else
        {
            MessageText.instance.ShowText("Eu já resolvi isso");
        }
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
