using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightItens : MonoBehaviour
{
    public Material outlineMaterial;
    private List<Material> originalMaterial = new List<Material>();
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
                        
                       /* ResetHighlight();
                         lastRenderer = renderer;
                        originalMaterial.Clear();
                        foreach (Material m in renderer.materials)
                         {
                             originalMaterial.Add(m);
                         }
                         List<Material> materials = new List<Material>();
                         //materials.Add(originalMaterial);
                         foreach(Material m in originalMaterial)
                         {
                             materials.Add(m);
                         }
                         materials.Add(outlineMaterial);
                         //renderer.SetMaterials(materials);
                         renderer.SetMaterials(materials);*/
                        CursorGame.instance.InteractCursor();
                    }
                }
                return;
            }
        }
        //ResetHighlight();
        CursorGame.instance.ResetInteractCursor();
    }

    void ResetHighlight()
    {
        if (lastRenderer != null)
        {
            lastRenderer.materials = new Material[0];
            List<Material> material = new List<Material>();
            //material.Add(originalMaterial);
            foreach (Material m in originalMaterial)
            {
                material.Add(m);
            }
            lastRenderer.SetMaterials(material);
           // lastRenderer.material = originalMaterial;
            lastRenderer = null;
        }
    }
}