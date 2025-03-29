using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HighlightItens : MonoBehaviour
{
    [SerializeField] public Material outlineMaterial;
    [SerializeField] private List<Material> originalMaterials = new List<Material>();
    [SerializeField] private Renderer lastRenderer;
    [SerializeField] private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        HighlightObject();
    }

    void HighlightObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.CompareTag("Item"))
            {
                Renderer renderer = hitObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    if (renderer != lastRenderer)
                    {
                        ResetHighlight();
                        lastRenderer = renderer;
                        originalMaterials.Clear();
                        foreach (Material m in renderer.materials)
                        {
                            originalMaterials.Add(m);
                        }
                        List<Material> materials = new List<Material>(originalMaterials)
                        {
                            outlineMaterial
                        };
                        renderer.materials = materials.ToArray();
                    }
                }
                return;
            }
        }
        ResetHighlight();
    }

    void ResetHighlight()
    {
        if (lastRenderer != null)
        {
            lastRenderer.materials = originalMaterials.ToArray();
            lastRenderer = null;
        }
    }
}