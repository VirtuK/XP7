using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ItemInspectionRenderer : MonoBehaviour
{
    public Material vignetteMaterial;

    public void SetMaterial(Material mat)
    {
        vignetteMaterial = mat;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (vignetteMaterial != null)
        {
            Graphics.Blit(src, dest, vignetteMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}