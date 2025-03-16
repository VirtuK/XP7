using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : Item
{
    [SerializeField] private bool isLocked;
    [SerializeField, ConditionalHide("isLocked")] private string keyItemName;
    [SerializeField] private string doorDestination;
    public override void Use()
    {
        if (isLocked)
        {
            if (InteractionManagar.instance.selectedItem != null && InteractionManagar.instance.selectedItem.itemName == keyItemName)
            {
                print("a porta abriu");
                InventoryManager.instance.RemoveItem(InteractionManagar.instance.selectedItem);
                StartCoroutine(changeScene());
            }
            else
            {
                MessageText.instance.ShowText("eu preciso de: " + keyItemName);
            }
            CursorGame.instance.resetCursor();
        }
        else
        {
            print("essa porta não tem tranca");
        }
    }

    private IEnumerator changeScene()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(doorDestination);
    }
}
