using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public float fadeDuration = 5f;
    public Image fadeImage;
    public AudioSource audioSC;
    public ParticleSystem particulas;

    private ParticleSystem.Particle[] particlesArray;

    public void PlayGame()
    {
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        float t = 0f;
        Color imgColor = fadeImage.color;

        if (particulas != null)
        {
            // Inicializa o array de partículas com a capacidade máxima
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

            // Fade das partículas, se houver
            if (particulas != null)
            {
                int alive = particulas.GetParticles(particlesArray);

                for (int i = 0; i < alive; i++)
                {
                    Color32 c = particlesArray[i].startColor;
                    c.a = (byte)(255 * (1f - normalizedTime)); // Alpha diminui com o tempo
                    particlesArray[i].startColor = c;
                }

                particulas.SetParticles(particlesArray, alive);
            }

            yield return null;
        }

        // Garante que o fade chegou ao fim
        fadeImage.color = new Color(imgColor.r, imgColor.g, imgColor.b, 1f);

        if (particulas != null)
            particulas.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        SceneManager.LoadScene("LabCriogenia");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}