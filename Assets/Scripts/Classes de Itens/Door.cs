using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private string doorDestination;

    private void Start()
    {
        turnOffDisplay();
    }

    public override void Use()
    {
        if (isLocked)
        {
            if (InteractionManagar.instance.selectedItem != null && InteractionManagar.instance.selectedItem.itemName == keyItemName)
            {
                print("a porta abriu");
                InventoryManager.instance.RemoveItem(InteractionManagar.instance.selectedItem);
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
                StartCoroutine(SceneChanger.instance.changeScene(doorDestination));
            }
        }
        else
        {
            print("essa porta não tem tranca");
        }
    }

    public void turnOnDisplay()
    {
        display.material = displayOn;
    }

    public void turnOffDisplay()
    {
        display.material = displayOff;
    }
}
