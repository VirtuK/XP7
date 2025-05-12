using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public float fadeDuration = 5f;
    public Image fadeImage;
    public AudioSource audioSC;
    // Start is called before the first frame update
    public void PlayGame()
    {
        StartCoroutine(BackToMenuAfterDelay());
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    IEnumerator BackToMenuAfterDelay()
    {

        float t = 0f;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            audioSC.volume = Mathf.Clamp01(1f - (t / fadeDuration));
            float alpha = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, 1f);

        SceneManager.LoadScene("LabCriogenia");
    }
}
