using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class Door : Item
{
    [SerializeField] private bool isLocked;
    [SerializeField] private string keyItemName;
    [SerializeField] private bool button;
    [SerializeField] public bool isButtonPressed;
    [SerializeField] public bool haveDisplay;
    [SerializeField] private MeshRenderer display;
    [SerializeField] private Light displayLight;
    [SerializeField] private Material displayOn;
    [SerializeField] private Material displayOff;
    [SerializeField] private string doorDestinationSceneName; // Scene name for runtime use

#if UNITY_EDITOR
    [SerializeField] private SceneAsset doorDestinationSceneAsset; // Editor-only scene reference
#endif

    [SerializeField] private bool isPuzzleDoor;

    [SerializeField] private AudioSource audioSC;
    [SerializeField] private AudioClip openingSound;
    [SerializeField] private AudioClip displaySound;

    private void Awake()
    {
        StartCoroutine(InitializeAfterSceneLoad());
    }

    private IEnumerator InitializeAfterSceneLoad()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        audioSC = GetComponent<AudioSource>();
        if (haveDisplay) StartDoorDisplay();
    }

    public void StartDoorDisplay()
    {
        display = FindDisplayInChildren(transform, "Display");
        displayLight = FindLightInChildren(transform, "DisplayLight");

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

    private Light FindLightInChildren(Transform parent, string tag)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>(true))
        {
            if (child.CompareTag(tag))
            {
                return child.GetComponent<Light>();
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
                Debug.Log("The door opened");
                audioSC.Stop();
                audioSC.clip = openingSound;
                audioSC.Play();
                InventoryManager.instance.RemoveItem(InteractionManagar.instance.selectedItem);
                SceneSerializationManager.instance.SaveScene();
                StartCoroutine(SceneChanger.instance.changeScene(doorDestinationSceneName));
            }
            else
            {
                MessageText.instance.ShowText("I need something like a " + keyItemName + " here.");
            }
            CursorGame.instance.resetCursor();
        }
        else if (button)
        {
            if (!isButtonPressed)
            {
                MessageText.instance.ShowText("Looks like this door is connected to something");
            }
            else
            {
                audioSC.Stop();
                audioSC.clip = openingSound;
                audioSC.Play();
                SceneSerializationManager.instance.SaveScene();
                StartCoroutine(SceneChanger.instance.changeScene(doorDestinationSceneName));
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
                StartCoroutine(SceneChanger.instance.changeScene(doorDestinationSceneName));
            }
        }
        else
        {
            audioSC.Stop();
            audioSC.clip = openingSound;
            audioSC.Play();
            SceneSerializationManager.instance.SaveScene();
            StartCoroutine(SceneChanger.instance.changeScene(doorDestinationSceneName));
        }
    }

    public void turnOnDisplay()
    {
        audioSC.Stop();
        audioSC.clip = displaySound;
        audioSC.Play();
        display.material = displayOn;
        displayLight.color = new Color32(34, 180, 0, 255);
        interactions = InteractionType.Use;
    }

    public void turnOffDisplay()
    {
        display.material = displayOff;
        displayLight.color = new Color32(180, 0, 0, 255);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (doorDestinationSceneAsset != null)
        {
            doorDestinationSceneName = doorDestinationSceneAsset.name;
        }
    }
#endif
}
