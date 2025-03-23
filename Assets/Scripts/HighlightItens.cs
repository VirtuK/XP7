using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightItens : MonoBehaviour
{
    public Material outlineMaterial; 
    private Material originalMaterial;
    private Renderer lastRenderer;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
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
                        originalMaterial = renderer.material;
                        List<Material> materials = new List<Material>();
                        materials.Add(originalMaterial);
                        materials.Add(outlineMaterial);
                        //renderer.SetMaterials(materials);
                        CursorGame.instance.InteractCursor();
                    }
                }
                return;
            }
        }
        ResetHighlight();
        CursorGame.instance.ResetInteractCursor();
    }

    void ResetHighlight()
    {
        if (lastRenderer != null)
        {
            List<Material> material = new List<Material>();
            material.Add(originalMaterial);
            lastRenderer.SetMaterials(material);
            lastRenderer.material = originalMaterial;
            lastRenderer = null;
        }
    }
}

