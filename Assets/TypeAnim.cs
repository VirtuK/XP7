using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

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
    [SerializeField] AudioSource typingSource;
    [SerializeField] AudioClip heartbeatIn;
    //[SerializeField] AudioClip heartbeatOut;
    [SerializeField] AudioClip alert;
    [SerializeField] AudioClip typing;

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
        string line = ParseCommand(rawLine);

        _textMeshPro.color = (i == 2 || i == 3 || i == 6)
            ? new Color32(173, 216, 230, 255)
            : new Color32(255, 255, 255, 255);

        if (i != 2 && i != 3 && i != 6 && i != 10 && !typingSource.isPlaying)
            typingSource.Play();

        int totalChars = line.Length;
        int visibleChars = 0;

        while (visibleChars <= totalChars)
        {
            string textToShow = line.Substring(0, visibleChars);

            // Show the cursor at the end ALWAYS if not in strings 2,3,6 and line not fully revealed
            if (i != 2 && i != 3 && i != 6 && visibleChars < totalChars)
            {
                textToShow += "▮";  // Cursor always visible
            }

            _textMeshPro.text = textToShow;

            visibleChars++;
            yield return new WaitForSeconds(timeBtwnChars);
        }

        if (typingSource.isPlaying)
            typingSource.Stop();

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
