using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FinalScene : MonoBehaviour
{
    public GameObject[] lights;
    public GameObject[] lamps;
    public Material black;
    // Start is called before the first frame update

    private void Start()
    {
        GetComponent<Animator>().SetBool("Final", true);
    }
   
    public void TurnOffLights()
    {
        foreach(GameObject l in lights)
        {
            var light = l.GetComponent<Light>();
            var flick = l.GetComponent<FlickLight>();
            if(flick != null) flick.enabled = false;
            light.intensity = 0; 
        }
        foreach(GameObject la in lamps)
        {
            var originalMaterial = new List<Material>();
            var mesh = la.GetComponent<Renderer>();
            foreach (Material m in mesh.materials)
            {
                originalMaterial.Add(m);
            }
            originalMaterial.Add(black);
            mesh.SetMaterials(originalMaterial);
        }
    }

    public void TurnOnLights()
    {
        // First: Remove the black material
        foreach (GameObject la in lamps)
        {
            var mesh = la.GetComponent<Renderer>();
            if (mesh != null)
            {
                List<Material> newMaterials = new List<Material>();

                foreach (Material m in mesh.materials)
                {
                    if (m.name.Replace(" (Instance)", "") != black.name)
                        newMaterials.Add(m);
                }

                mesh.materials = newMaterials.ToArray();
            }
        }

        // Then: Turn on lights (after materials are cleaned up)
        foreach (GameObject l in lights)
        {
            var flick = l.GetComponent<FlickLight>();
            if (flick != null)
                flick.enabled = true;

            var light = l.GetComponent<Light>();
            if (light != null)
                light.intensity = 1f; // or whatever default you want
        }
    }
}
