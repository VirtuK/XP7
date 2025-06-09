using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class SceneChanger : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 2f;

    public static SceneChanger instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        canvasGroup = GameObject.Find("FadeImage").GetComponent<CanvasGroup>();
        StartCoroutine(FadeIn());

    }
    public void changeScene(string scene)
    {
        if(InventoryUI.instance != null) InventoryUI.instance.abriuPapel = false;
        SceneSerializationManager.instance.SaveScene();
        if (scene == "sair")
        {
            Application.Quit();
        }
        else FadeToScene(scene);
    }

    private void Start()
    {
        canvasGroup = GameObject.Find("FadeImage").GetComponent<CanvasGroup>();
        
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeOutIn(sceneName));
    }

    private IEnumerator FadeIn()
    {
        float timer = fadeDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            canvasGroup.alpha = timer / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 0;
    }

    private IEnumerator FadeOutIn(string scene)
    {
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = timer / fadeDuration;
            yield return null;
        }
        canvasGroup.alpha = 1;
        if(InteractionManagar.instance != null) InteractionManagar.instance.resetInteractions();
        SceneManager.LoadScene(scene);
    }
}
