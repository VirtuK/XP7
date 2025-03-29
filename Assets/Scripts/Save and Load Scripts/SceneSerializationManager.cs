using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSerializationManager : MonoBehaviour
{
    public static SceneSerializationManager instance;
    private string saveFilePath;
    private bool isFirstLoad;
    private List<GameObjectData> sceneData = new List<GameObjectData>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        UpdateSaveFilePath();
        CheckFirstLoad();
        sceneData.Clear();
    }

    private void UpdateSaveFilePath()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        saveFilePath = Path.Combine(Application.persistentDataPath, sceneName + "_sceneData.json");
    }

    private void CheckFirstLoad()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (!ProgressManager.instance.IsSceneLoaded(sceneName))
        {
            isFirstLoad = true;

            print("first");
            DeleteSceneDataFile();
        }
        else
        {
            isFirstLoad = false;
        }

    }

    public bool getFirstLoad()
    {
        return isFirstLoad;
    }

    private void DeleteSceneDataFile()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                File.Delete(saveFilePath);
                Debug.Log($"Deleted existing scene data file: {saveFilePath}");
            }
            catch (IOException e)
            {
                Debug.LogError($"Failed to delete scene data file: {e.Message}");
            }
        }
    }

    public void SaveScene()
    {
        UpdateSaveFilePath(); // Ensure the file path is updated to the current scene
        List<GameObjectData> sceneData = new List<GameObjectData>();

        foreach (GameObject gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            SaveObject(gameObject, sceneData);
        }

        Debug.Log($"Number of GameObjects to save: {sceneData.Count}");

        GameObjectDataList dataList = new GameObjectDataList { items = sceneData };
        string json = JsonUtility.ToJson(dataList, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Scene saved to {saveFilePath}");
    }

    private void SaveObject(GameObject gameObject, List<GameObjectData> sceneData)
    {
        

        if ((gameObject.name == "Player" || HasItemComponent(gameObject)) && gameObject.name != "Mensagem")
        {
            Debug.Log($"Processing GameObject: {gameObject.name}");
            sceneData.Add(new GameObjectData(gameObject));
            Debug.Log("Saved");
        }

        foreach (Transform child in gameObject.transform)
        {
           if(child.gameObject.name != "Mensagem") SaveObject(child.gameObject, sceneData);
        }
    }

    public bool HasItemComponent(GameObject gameObject)
    {
        return gameObject.GetComponent<Item>() != null;
    }

    public void LoadScene()
    {
        UpdateSaveFilePath();

        if (!File.Exists(saveFilePath))
        {
            Debug.LogError($"Save file not found at {saveFilePath}");
            return;
        }

        string json = File.ReadAllText(saveFilePath);
        SerializationWrapper<GameObjectData> wrapper = JsonUtility.FromJson<SerializationWrapper<GameObjectData>>(json);
        Dictionary<string, GameObject> allObjects = new Dictionary<string, GameObject>();

        // Reconstruct all GameObjects and apply component data
        foreach (GameObjectData data in wrapper.items)
        {
            GameObject gameObject = data.Reconstruct(allObjects);

            if (gameObject != null)
            {
                foreach (var componentData in data.componentsData)
                {
                    componentData.ApplyTo(gameObject);
                }
            }
            else
            {
                Debug.LogError($"Falha ao reconstruir o GameObject com ID {gameObject.name}. Objeto é null.");
            }

            allObjects[data.uniqueID] = gameObject;
        }

        Debug.Log("Scene loaded.");
        MessageText.instance.Research();
    }
}

[System.Serializable]
public class SerializationWrapper<T>
{
    public List<T> items;

    public SerializationWrapper() { }

    public SerializationWrapper(List<T> items)
    {
        this.items = items;
    }
}
