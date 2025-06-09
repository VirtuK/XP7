using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextAnim : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] TextMeshProUGUI _textMeshPro;
    [SerializeField] CanvasGroup fadeCanvas;

    [Header("Text Settings")]
    public string[] stringArray;
    [SerializeField] float timeBtwnChars = 0.05f;
    [SerializeField] float timeBtwnLines = 2f;

    [Header("Audio Settings")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip heartbeatIn;
    //[SerializeField] AudioClip heartbeatOut;
    [SerializeField] AudioClip alert;

    [Header("Fade Settings")]
    [SerializeField] float fadeDuration = 1f;

    int i = 0;

    void Start()
    {
        StartCoroutine(ShowNextLine());
    }

    IEnumerator ShowNextLine()
    {
        if (i >= stringArray.Length)
            yield break;

        string rawLine = stringArray[i];
        string line = ParseCommand(rawLine); // executa efeitos e remove tags

        _textMeshPro.text = line;
        _textMeshPro.maxVisibleCharacters = 0;
        _textMeshPro.ForceMeshUpdate();
        int totalVisibleCharacters = _textMeshPro.textInfo.characterCount;

        for (int j = 0; j <= totalVisibleCharacters; j++)
        {
            _textMeshPro.maxVisibleCharacters = j;
            yield return new WaitForSeconds(timeBtwnChars);
        }

        i++;
        yield return new WaitForSeconds(timeBtwnLines);
        StartCoroutine(ShowNextLine());
    }

    string ParseCommand(string line)
    {
        if (line.StartsWith("[sound="))
        {
            int endIdx = line.IndexOf("]");
            string command = line.Substring(7, endIdx - 7);
            PlayEffect(command);
            return line.Substring(endIdx + 1);
        }

        return line;
    }

    void PlayEffect(string id)
    {
        switch (id)
        {
            case "fadein":
                StartCoroutine(FadeCanvas(1f, 0f));
                break;
            case "fadeout":
                StartCoroutine(FadeCanvas(0f, 1f));
                break;
            case "alert":
                audioSource.PlayOneShot(alert);
                audioSource.volume = 0.3f;
                break;
            case "heartbeat_in":
                audioSource.PlayOneShot(heartbeatIn);
                break;
            /*case "heartbeat_out":
                audioSource.PlayOneShot(heartbeatOut);
                break;
*/        }
    }

    IEnumerator FadeCanvas(float from, float to)
    {
        float t = 0f;
        fadeCanvas.alpha = from;

        while (t < fadeDuration)
        {
            fadeCanvas.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }

        fadeCanvas.alpha = to;
    }
}
