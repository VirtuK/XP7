using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class Door : Item
{
    [SerializeField] private bool isLocked;
    [SerializeField, ConditionalHide("isLocked")] private string keyItemName;
    [SerializeField] private bool button;
    [SerializeField, ConditionalHide("button")] public bool isButtonPressed;
    [SerializeField] public bool haveDisplay;
    [SerializeField, ConditionalHide("haveDisplay")] private MeshRenderer display;
    [SerializeField, ConditionalHide("haveDisplay")] private Material displayOn;
    [SerializeField, ConditionalHide("haveDisplay")] private Material displayOff;
    [SerializeField] private SceneAsset doorDestination;
    [SerializeField] private bool isPuzzleDoor;

    [SerializeField] private AudioSource audioSC;
    [SerializeField] private AudioClip openingSound;
    [SerializeField, ConditionalHide("haveDisplay")] private AudioClip displaySound;
    private void Awake()
    {
        StartCoroutine(InitializeAfterSceneLoad());
    }
    private IEnumerator InitializeAfterSceneLoad()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        audioSC = GetComponent<AudioSource>();
        if (haveDisplay) startDoorDisplay();
    }
    public void startDoorDisplay()
    {
        display = FindDisplayInChildren(transform, "Display");

        if (display != null)
        {
            
            if (!isButtonPressed) turnOffDisplay();
            else turnOnDisplay();

            if (isPuzzleDoor)
            {
                if (ProgressManager.instance.puzzleResolved)
                {
                    turnOnDisplay();
                }
            }
        }
        else
        {
            Debug.LogError("Tagged display not found in children of " + gameObject.name);
        }
    }

    private MeshRenderer FindDisplayInChildren(Transform parent, string tag)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.CompareTag(tag))
            {
                return child.GetComponent<MeshRenderer>();
            }
        }
        return null;
    }
    public override void Use()
    {
        if (isLocked)
        {
            if (InteractionManagar.instance.selectedItem != null && InteractionManagar.instance.selectedItem.itemName == keyItemName)
            {
                print("a porta abriu");
                audioSC.Stop();
                audioSC.clip = openingSound;
                audioSC.Play();
                InventoryManager.instance.RemoveItem(InteractionManagar.instance.selectedItem);
                SceneSerializationManager.instance.SaveScene();
                StartCoroutine(SceneChanger.instance.changeScene(doorDestination));
            }
            else
            {
                MessageText.instance.ShowText("eu preciso de: " + keyItemName);
            }
            CursorGame.instance.resetCursor();
        }
        else if (button)
        {
            if (!isButtonPressed)
            {
                MessageText.instance.ShowText("Parece que essa porta está conectada a algum dispositivo");
            }
            else
            {
                audioSC.Stop();
                audioSC.clip = openingSound;
                audioSC.Play();
                SceneSerializationManager.instance.SaveScene();
                StartCoroutine(SceneChanger.instance.changeScene(doorDestination));
            }
        }
        else if (isPuzzleDoor)
        {
            if (ProgressManager.instance.puzzleResolved)
            {
                audioSC.Stop();
                audioSC.clip = openingSound;
                audioSC.Play();
                SceneSerializationManager.instance.SaveScene();
                StartCoroutine(SceneChanger.instance.changeScene(null));
            }
        }
        else
        {
            audioSC.Stop();
            audioSC.clip = openingSound;
            audioSC.Play();
            SceneSerializationManager.instance.SaveScene();
            StartCoroutine(SceneChanger.instance.changeScene(doorDestination));
        }
    }

    public void turnOnDisplay()
    {
        audioSC.Stop();
        audioSC.clip = displaySound;
        audioSC.Play();
        display.material = displayOn;
    }

    public void turnOffDisplay()
    {
        display.material = displayOff;
    }

}
