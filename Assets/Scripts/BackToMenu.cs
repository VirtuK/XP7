using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackToMenu : MonoBehaviour
{
    public float delay = 5f;
    public float fadeDuration = 5f;
    public Image fadeImage;



    public void BackToMenuButton()
    {
        StartCoroutine(BackToMenuAfterDelay());
    }


    IEnumerator BackToMenuAfterDelay()
    {

        float t = 0f;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Clamp01(t / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

         fadeImage.color = new Color(color.r, color.g, color.b, 1f);

        SceneManager.LoadScene("Menu");
    }
}
