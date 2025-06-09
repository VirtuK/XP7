using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FinalScene : MonoBehaviour
{
    public GameObject[] lights;
    public GameObject[] lamps;
    public Material black;
    public Image fadeImage;
    public CameraShake cameraShake; 

    public AudioSource aS;
    public AudioSource pS;
    public AudioSource pC;
    public AudioClip metal;
    public AudioClip monstro;
    public AudioClip rugido;
    public AudioClip footsteps;
    public AudioClip coracao;

    private void Start()
    {
        GetComponent<Animator>().SetBool("Final", true);
    }

    public void HeartSound()
    {
        pC.clip = coracao;
        pC.Play();
        StartCoroutine(GraduallyIncreaseVolume(pC, 1f, 2f));

    }

    private IEnumerator GraduallyIncreaseVolume(AudioSource source, float targetVolume, float duration)
    {
        float startVolume = 0f;
        source.volume = 0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            yield return null;
        }

        source.volume = targetVolume;
    }

    public void FootStepSound()
    {
        pS.clip = footsteps;
        pS.Play();

    }

    public void StopFootStepSound()
    {
        pS.Stop();
    }

    public void PlayMetal()
    {
        aS.clip = metal;
        aS.Play();
    }

    public void PlayMonster()
    {
        aS.clip = monstro;
        aS.Play();
    }

    public void PlayRoar()
    {
        aS.clip = rugido;
        aS.Play();
    }

    public void TurnOffLights()
    {
        foreach (GameObject l in lights)
        {
            var flick = l.GetComponent<FlickLight>();
            var light = l.GetComponent<Light>();
            if (flick != null) flick.enabled = false;
            if (light != null) light.intensity = 0;
        }

        foreach (GameObject la in lamps)
        {
            var mesh = la.GetComponent<Renderer>();
            if (mesh != null)
            {
                var mats = new List<Material>(mesh.materials);
                mats.Add(black);
                mesh.materials = mats.ToArray();
            }
        }

        fadeImage.color = new Color32(0, 0, 0, 185);
    }

    public void TurnOnLights()
    {
        foreach (GameObject la in lamps)
        {
            var mesh = la.GetComponent<Renderer>();
            if (mesh != null)
            {
                var newMats = new List<Material>();
                foreach (var m in mesh.materials)
                {
                    if (m.name.Replace(" (Instance)", "") != black.name)
                        newMats.Add(m);
                }
                mesh.materials = newMats.ToArray();
            }
        }

        foreach (GameObject l in lights)
        {
            var flick = l.GetComponent<FlickLight>();
            var light = l.GetComponent<Light>();
            if (flick != null) flick.enabled = true;
            if (light != null) light.intensity = 1f;
        }

        fadeImage.color = new Color32(0, 0, 0, 0);
    }

    public void ActivateShake()
    {
        if (cameraShake != null)
            cameraShake.Shake(0.5f, 0.1f);
    }

    public void ActivateShake2()
    {
        if (cameraShake != null)
            cameraShake.Shake(0.8f, 0.3f);
    }

    public void ActivateShake3()
    {
        if (cameraShake != null)
            cameraShake.Shake(2f, 0.6f);
    }


    public void DeactivateShake()
    {
        if (cameraShake != null)
        {
            cameraShake.shakeDuration = 0f;
        }
    }

    public void FadeInScreen()
    {
        StopCoroutine(nameof(FadeToOpaque));
        StartCoroutine(FadeToOpaque(2f));
    }

    private IEnumerator FadeToOpaque(float duration)
    {
        float elapsed = 0f;
        Color c = fadeImage.color;
        float startAlpha = 0.72f;
        float targetAlpha = 1f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = targetAlpha;
        fadeImage.color = c;
    }
}
