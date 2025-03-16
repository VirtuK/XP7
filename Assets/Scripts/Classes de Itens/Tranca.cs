using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tranca : Item
{
    [SerializeField] private List<Componente> componentes;

    public override void Use()
    {
        if(InteractionManagar.instance.selectedItem != null &&
            componentes.Contains(InteractionManagar.instance.selectedItem.GetComponent<Componente>()))
        {
            componentes.Remove(InteractionManagar.instance.selectedItem.GetComponent<Componente>());
            InventoryManager.instance.RemoveItem(InteractionManagar.instance.selectedItem);
        }
        else if (componentes.Count == 0)
        {
            StartCoroutine(SceneChanger.instance.changeScene("SlidingPuzzle"));
        }
        else
        {
            MessageText.instance.ShowText("Parece que faltam " + componentes.Count + " peças nesse dispositivo");
        }
        CursorGame.instance.resetCursor();
        
    }

    
}
