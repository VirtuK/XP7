using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tranca : Item
{
    [SerializeField] private List<string> requiredComponentIDs; // Unique IDs of required components
    [SerializeField] private List<MeshRenderer> tilesMesh;
    [SerializeField] private Material onMaterial;
    [SerializeField] private SceneAsset puzzleScene;

    public override void Use()
    {
        if (InteractionManagar.instance.selectedItem != null)
        {
            Componente selectedComponent = InteractionManagar.instance.selectedItem.GetComponent<Componente>();

            if (selectedComponent != null && requiredComponentIDs.Contains(selectedComponent.ID))
            {
                tilesMesh[requiredComponentIDs.IndexOf(selectedComponent.ID)].material = onMaterial;
                requiredComponentIDs.Remove(selectedComponent.ID);
                InventoryManager.instance.RemoveItem(InteractionManagar.instance.selectedItem);
            }
        }

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

    public List<string> GetRequiredComponents()
    {
        return requiredComponentIDs;
    }
}