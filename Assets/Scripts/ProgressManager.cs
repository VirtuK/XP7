using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class ProgressManager : MonoBehaviour
{
    public static ProgressManager instance;
    [SerializeField] public List<string> loadedScenes = new List<string>();
    [SerializeField] public bool puzzleResolved;
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(InitializeAfterSceneLoad(scene));
    }

    private IEnumerator InitializeAfterSceneLoad(Scene scene)
    {
        yield return new WaitForEndOfFrame();
         if(!loadedScenes.Contains(scene.name)) loadedScenes.Add(scene.name);
        SceneSerializationManager.instance.LoadScene();
    }

    public bool IsSceneLoaded(string sceneName)
    {
        return loadedScenes.Contains(sceneName);
    }
}

