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
    [SerializeField] private string doorDestination;
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

    
}
