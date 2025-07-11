using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightItens : MonoBehaviour
{
    public Material outlineMaterial;
    public Material outlineMaterial2;
    private List<Material> originalMaterial = new List<Material>();
    private Renderer lastRenderer;
    private Camera mainCamera;
    private GameObject player;

    void Start()
    {
        mainCamera = Camera.main;
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (!player.GetComponent<ClickToMove>().doingPuzzle) 
        {
            HighlightObject();
        }
    }

    public void HighlightObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && !InteractionManagar.instance.interacting && InteractionManagar.instance.resetCooldown <= 0)
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
                        originalMaterial.Clear();
                        foreach (Material m in renderer.materials)
                         {
                             originalMaterial.Add(m);
                         }
                         List<Material> materials = new List<Material>();
                         foreach(Material m in originalMaterial)
                         {
                             materials.Add(m);
                         }
                        if (hitObject.GetComponent<Door_Button>())
                        {
                            materials.Add(outlineMaterial2);
                        }
                        else materials.Add(outlineMaterial);
                         renderer.SetMaterials(materials);
                        if (hitObject.GetComponent<Door>())
                        {
                            CursorGame.instance.DoorCursor();
                        }
                        else
                        {
                            CursorGame.instance.InteractCursor();
                            InteractionManagar.instance.highlightedItem = hitObject.GetComponent<Item>();

                        }
                        return;
                    }
                }
                return;
            }
            else if (hitObject.CompareTag("Ground"))
            {
                CursorGame.instance.ResetDrawCursor();
                CursorGame.instance.ResetDoorCursor();
                InteractionManagar.instance.highlightedItem = null;
                return;
            }
        }
        ResetHighlight();
        CursorGame.instance.PrincipalCursor();
        CursorGame.instance.ResetDoorCursor();
    }

    public void ResetHighlight()
    {
        if (lastRenderer != null)
        {
            lastRenderer.materials = new Material[0];
            List<Material> material = new List<Material>();
            foreach (Material m in originalMaterial)
            {
                material.Add(m);
            }
            lastRenderer.SetMaterials(material);
            lastRenderer = null;
        }
    }
}