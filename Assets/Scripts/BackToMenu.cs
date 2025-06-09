using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BackToMenu : MonoBehaviour
{
    public float delay = 5f;
    public float fadeDuration = 5f;
    public Image fadeImage;

    public GameObject[] buttonsToDisable;
    public AudioSource uiAudioSource;

    private void Start()
    {
        Cursor.visible = true;
    }
    public void BackToMenuButton()
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
        SceneSerializationManager.instance.DeleteAllFiles();
        SceneManager.LoadScene("Menu");
    }
}
