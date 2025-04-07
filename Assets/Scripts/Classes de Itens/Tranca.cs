using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

public class Tranca : Item
{
    [SerializeField] private List<string> requiredComponentNames; // Unique IDs of required components
    [SerializeField] private List<MeshRenderer> tilesMesh;
    [SerializeField] private Material onMaterial;
    [SerializeField] private SceneAsset puzzleScene;

    public override void Use()
    {
        if (InteractionManagar.instance.selectedItem != null)
        {
            ItemData selectedComponent = InteractionManagar.instance.selectedItem;
            print(selectedComponent.itemName);
            if (selectedComponent != null && requiredComponentNames.Contains(selectedComponent.itemName))
            {
                requiredComponentNames.Remove(selectedComponent.itemName);
                InventoryManager.instance.RemoveItem(InteractionManagar.instance.selectedItem);
            }
        }
        if (requiredComponentNames.Count == 0)
        {
            StartCoroutine(SceneChanger.instance.changeScene(puzzleScene));
        }
        else
        {
            MessageText.instance.ShowText($"Parece que faltam {requiredComponentNames.Count} peças nesse dispositivo");
        }

        CursorGame.instance.resetCursor();
    }

    public List<string> GetRequiredComponents()
    {
        return requiredComponentNames;
    }

    public void setComponentMaterial(Componente component)
    {
        tilesMesh[component.ID].material = onMaterial;
    }
}