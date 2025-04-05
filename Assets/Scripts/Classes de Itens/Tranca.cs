using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

public class Tranca : Item
{
    [SerializeField] private List<int> requiredComponentIDs; // Unique IDs of required components
    [SerializeField] private List<MeshRenderer> tilesMesh;
    [SerializeField] private Material onMaterial;
    [SerializeField] private SceneAsset puzzleScene;

    public override void Use()
    {
        if (InteractionManagar.instance.selectedItem != null)
        {
            Componente selectedComponent = null;

            if (InteractionManagar.instance.selectedItem.returnComponent() is Componente comp)
            {
                selectedComponent = comp;
            }


            if (selectedComponent != null && requiredComponentIDs.Contains(selectedComponent.ID))
            {
                setComponentMaterial(selectedComponent);
                requiredComponentIDs.Remove(selectedComponent.ID);
                InventoryManager.instance.RemoveItem(InteractionManagar.instance.selectedItem);
            }
        }
        StartCoroutine(SceneChanger.instance.changeScene(puzzleScene));
        if (requiredComponentIDs.Count == 0)
        {
            StartCoroutine(SceneChanger.instance.changeScene(puzzleScene));
        }
        else
        {
            MessageText.instance.ShowText($"Parece que faltam {requiredComponentIDs.Count} peças nesse dispositivo");
        }

        CursorGame.instance.resetCursor();
    }

    public List<int> GetRequiredComponents()
    {
        return requiredComponentIDs;
    }

    public void setComponentMaterial(Componente component)
    {
        tilesMesh[component.ID].material = onMaterial;
    }
}