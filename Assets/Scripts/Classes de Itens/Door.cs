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
        if (haveDisplay) startDoorDisplay();
        audioSC = GetComponent<AudioSource>();
    }
    public void startDoorDisplay()
    {
        display = GameObject.Find("Display").GetComponent<MeshRenderer>();
        if (!isButtonPressed) turnOffDisplay();
        else turnOnDisplay();
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
