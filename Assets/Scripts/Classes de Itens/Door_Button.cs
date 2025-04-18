using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
[System.Serializable]
public class Door_Button : Item
{
    [SerializeField] private string doorName; // Store Door name instead of reference
    public Door door; // Runtime reference (not serialized)


    private void Start()
    {
        StartCoroutine(InitializeAfterSceneLoad());
    }

    private IEnumerator InitializeAfterSceneLoad()
    {
        yield return new WaitForEndOfFrame();
        if (door != null)
        {
            doorName = door.gameObject.name;
        }
        FindDoor();
    }

    public override void Use()
    {
        if (door != null && !door.isButtonPressed)
        {
            MessageText.instance.ShowText("This activated something on this room");
            door.isButtonPressed = true;
            if (door.haveDisplay) door.turnOnDisplay();
        }
        else
        {
            MessageText.instance.ShowText("Can't use it again.");
        }
    }

    // Look for the Door object using its stored name
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
