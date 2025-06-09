using UnityEngine;
using TMPro;

public class StartScene : MonoBehaviour
{
    public TMP_Text text;
    public AudioSource music;
    public AudioSource aS;
    public AudioClip heart;
    private float fadeDuration = 5f;
    private float fadeTimer = 0f;
    private float delayBeforeFade = 5f;
    private float delayTimer = 0f;
    private bool fading = false;

    void Start()
    {
        // Inicializa o texto com alpha 0 (invisível)
        text.color = new Color(1, 1, 1, 0);
        music = GameObject.Find("Music").GetComponent<AudioSource>();
        music.volume = 0;
        aS.clip = heart;
        aS.Play();
    }

    void Update()
    {
        // Espera os 3 segundos antes de iniciar o fade-in
        if (!fading)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer >= delayBeforeFade)
            {
                fading = true;
                fadeTimer = 0f; // garante que o fade comece do início
            }
        }
        else
        {
            // Inicia o fade-in
            if (fadeTimer < fadeDuration)
            {
                fadeTimer += Time.deltaTime;
                float alpha = Mathf.Clamp01(fadeTimer / fadeDuration);
                text.color = new Color(1, 1, 1, alpha);
            }
            if (Input.GetMouseButtonDown(0))
            {
                SceneChanger.instance.changeScene("LabCriogenia");
            }
        }
    }
}