using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public float fadeDuration = 5f;
    public Image fadeImage;
    public AudioSource audioSC;
    public ParticleSystem particulas;

    private ParticleSystem.Particle[] particlesArray;

    public GameObject[] buttonsToDisable;
    public AudioSource uiAudioSource;

    public void PlayGame()
    {
        foreach (var obj in buttonsToDisable)
        {
            var button = obj.GetComponent<Button>();
            var selectable = obj.GetComponent<Selectable>();

            if (button != null)
            {
                

                ColorBlock colors = button.colors;
                Color invisivel = new Color(1f, 1f, 1f, 0f);
                colors.normalColor = invisivel;
                colors.highlightedColor = invisivel;
                colors.pressedColor = invisivel;
                colors.selectedColor = invisivel;
                colors.disabledColor = invisivel;
                button.colors = colors;
                button.interactable = false;
            }

        }

        EventSystem.current.SetSelectedGameObject(null);

        if (uiAudioSource != null && uiAudioSource.isPlaying)
        {
            Destroy(uiAudioSource);
        }

        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        float t = 0f;
        Color imgColor = fadeImage.color;

        if (particulas != null)
        {
            particlesArray = new ParticleSystem.Particle[particulas.main.maxParticles];
        }

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(t / fadeDuration);

            // Fade da imagem
            fadeImage.color = new Color(imgColor.r, imgColor.g, imgColor.b, normalizedTime);

            // Fade do som
            if (audioSC != null)
                audioSC.volume = 1f - normalizedTime;

            // Fade das partículas
            if (particulas != null)
            {
                int alive = particulas.GetParticles(particlesArray);

                for (int i = 0; i < alive; i++)
                {
                    Color32 c = particlesArray[i].startColor;
                    c.a = (byte)(255 * (1f - normalizedTime));
                    particlesArray[i].startColor = c;
                }

                particulas.SetParticles(particlesArray, alive);
            }

            yield return null;
        }

        // Garante que tudo está no estado final
        fadeImage.color = new Color(imgColor.r, imgColor.g, imgColor.b, 1f);

        if (particulas != null)
            particulas.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        SceneChanger.instance.changeScene("CenaInicio");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}