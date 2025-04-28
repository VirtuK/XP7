using UnityEngine;

public class VignetteEffect : MonoBehaviour
{
    public Material vignetteMaterial;
    private bool active = false;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (active && vignetteMaterial != null)
        {
            Graphics.Blit(src, dest, vignetteMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }

    public void EnableVignette()
    {
        active = true;
        vignetteMaterial.SetFloat("_Intensity", 0.5f); // or any value you want
    }

    public void DisableVignette()
    {
        active = false;
    }
}