using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager instance;

    [SerializeField] private List<string> savedScenes = new List<string>();

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
        loadProgress();
    }

    private void loadProgress()
    {

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name.Trim();
        switch (sceneName)
        {
            case "LabCriogenia":
                Door door = GameObject.FindAnyObjectByType<Door>();
                if (savedScenes.Contains(currentScene.name))
                {
                    if (door != null)
                    {
                        door.isButtonPressed = true;
                        door.startDoorDisplay();
                        print(door.gameObject.name);
                    }
                    else
                    {
                        print("No Door object found!");
                    }
                    break;
                }
                break;

            case "Level2":
                break;

            default:
                break;

        }
    }

    public void saveScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (!savedScenes.Contains(currentScene.name))
        {
            savedScenes.Add(currentScene.name);
        }
    }
}

