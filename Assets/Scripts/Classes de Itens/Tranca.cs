using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Tranca : Item
{
    [SerializeField] private List<string> requiredComponentNames; // Unique IDs of required components
    [SerializeField] private List<MeshRenderer> tilesMesh;
    [SerializeField] private Material onMaterial;
    [SerializeField] private string puzzleSceneName; // Store scene name for runtime use

#if UNITY_EDITOR
    [SerializeField] private SceneAsset puzzleSceneAsset; // Editor-only scene reference
#endif

    public override void Use()
    {
        if (InteractionManagar.instance.selectedItem != null)
        {
            ItemData selectedComponent = InteractionManagar.instance.selectedItem;
            if (selectedComponent != null && requiredComponentNames.Contains(selectedComponent.itemName))
            {
                requiredComponentNames.Remove(selectedComponent.itemName);
                InventoryManager.instance.RemoveItem(selectedComponent);
            }
        }
        if (requiredComponentNames.Count == 0)
        {
            StartCoroutine(SceneChanger.instance.changeScene(puzzleSceneName));
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

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (puzzleSceneAsset != null)
        {
            puzzleSceneName = puzzleSceneAsset.name;
        }
    }
#endif
}