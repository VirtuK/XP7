using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSerializationManager : MonoBehaviour
{
    public static SceneSerializationManager instance;
    private string saveFilePath;
    private string destroyedObjectsFilePath;
    private bool isFirstLoad;
    private List<GameObjectData> sceneData = new List<GameObjectData>();
    private List<GameObjectData> destroyedObjects = new List<GameObjectData>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        DontDestroyOnLoad(gameObject);
        UpdateSaveFilePath();
        LoadDestroyedObjects();
        CheckFirstLoad();
        sceneData.Clear();
    }

    private void UpdateSaveFilePath()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        saveFilePath = Path.Combine(Application.persistentDataPath, sceneName + "_sceneData.json");
        destroyedObjectsFilePath = Path.Combine(Application.persistentDataPath, sceneName + "_destroyedObjects.json");
    }

    private void CheckFirstLoad()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (!ProgressManager.instance.IsSceneLoaded(sceneName))
        {
            isFirstLoad = true;
            DeleteSceneDataFile();
        }
        else
        {
            isFirstLoad = false;
        }
    }

    private void DeleteSceneDataFile()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }
        if (File.Exists(destroyedObjectsFilePath))
        {
            File.Delete(destroyedObjectsFilePath);
        }
    }

    public void DeleteAllFiles()
    {

                string sceneName = "LabCriogenia";
                saveFilePath = Path.Combine(Application.persistentDataPath, sceneName + "_sceneData.json");
                destroyedObjectsFilePath = Path.Combine(Application.persistentDataPath, sceneName + "_destroyedObjects.json");
                print(saveFilePath);
                DeleteSceneDataFile();
            
            
                sceneName = "Corredor V1";
                saveFilePath = Path.Combine(Application.persistentDataPath, sceneName + "_sceneData.json");
                destroyedObjectsFilePath = Path.Combine(Application.persistentDataPath, sceneName + "_destroyedObjects.json");
                print(saveFilePath);
                DeleteSceneDataFile();
           
            
                sceneName = "QuartoRoberto";
                saveFilePath = Path.Combine(Application.persistentDataPath, sceneName + "_sceneData.json");
                destroyedObjectsFilePath = Path.Combine(Application.persistentDataPath, sceneName + "_destroyedObjects.json");
                print(saveFilePath);
                DeleteSceneDataFile();
           
            
                sceneName = "QuartoAmelia";
                saveFilePath = Path.Combine(Application.persistentDataPath, sceneName + "_sceneData.json");
                destroyedObjectsFilePath = Path.Combine(Application.persistentDataPath, sceneName + "_destroyedObjects.json");
                print(saveFilePath);
                DeleteSceneDataFile();
          
            
                sceneName = "QuartoVinicius";
                saveFilePath = Path.Combine(Application.persistentDataPath, sceneName + "_sceneData.json");
                destroyedObjectsFilePath = Path.Combine(Application.persistentDataPath, sceneName + "_destroyedObjects.json");
                print(saveFilePath);
                DeleteSceneDataFile();
            
    }

    private void LoadDestroyedObjects()
    {
        UpdateSaveFilePath();
        if (File.Exists(destroyedObjectsFilePath))
        {
            string json = File.ReadAllText(destroyedObjectsFilePath);
            SerializationWrapper<GameObjectData> wrapper = JsonUtility.FromJson<SerializationWrapper<GameObjectData>>(json);
            destroyedObjects = wrapper.items ?? new List<GameObjectData>();
        }
        else
        {
            destroyedObjects = new List<GameObjectData>();
        }
    }

    private void SaveDestroyedObjects()
    {
        string json = JsonUtility.ToJson(new SerializationWrapper<GameObjectData>(destroyedObjects), true);
        File.WriteAllText(destroyedObjectsFilePath, json);
    }

    public void RegisterDestroyedObject(GameObject gameObject)
    {
        GameObjectData objectData = new GameObjectData(gameObject);

        if (!destroyedObjects.Any(data => data.uniqueID == objectData.uniqueID))
        {
            destroyedObjects.Add(objectData);
            SaveDestroyedObjects();
            Debug.Log($"Registered destroyed object: {objectData.name} (ID: {objectData.uniqueID})");
        }
    }

    public void SaveScene()
    {
        UpdateSaveFilePath();
        List<GameObjectData> sceneData = new List<GameObjectData>();

        foreach (GameObject gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            SaveObject(gameObject, sceneData);
        }

        GameObjectDataList dataList = new GameObjectDataList { items = sceneData };
        string json = JsonUtility.ToJson(dataList, true);
        File.WriteAllText(saveFilePath, json);
    }

    private void SaveObject(GameObject gameObject, List<GameObjectData> sceneData)
    {
        string id = gameObject.GetInstanceID().ToString();

        if (destroyedObjects.Any(data => data.uniqueID == id))
        {
            Debug.Log($"Skipping saving destroyed object: {gameObject.name}");
            return;
        }

        if ((gameObject.name == "Player" || HasItemComponent(gameObject)) && gameObject.name != "Mensagem")
        {
            if(gameObject.name == "Player")
            {
                Debug.Log($"Saving GameObject: {gameObject.name}");
                sceneData.Add(new GameObjectData(gameObject));
            }
            else if (!gameObject.GetComponent<Item>().IgnoreOnSave)
            {
                Debug.Log($"Saving GameObject: {gameObject.name}");
                sceneData.Add(new GameObjectData(gameObject));
            }
            
        }

        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.name != "Mensagem")
                SaveObject(child.gameObject, sceneData);
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

        HashSet<string> savedObjectIDs = new HashSet<string>(wrapper.items.Select(data => data.uniqueID));

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
            allObjects[data.uniqueID] = gameObject;
        }

        foreach (GameObject gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            CheckAndDestroyUnsavedItems(gameObject, savedObjectIDs);
        }
    }

    private void CheckAndDestroyUnsavedItems(GameObject gameObject, HashSet<string> savedObjectIDs)
    {
        string name = gameObject.name;

        if (HasItemComponent(gameObject))
        {
            if (destroyedObjects.Any(data => data.name == name))
            {
                Debug.Log($"Destroying previously destroyed Item: {gameObject.name}");
                gameObject.SetActive(false);
                return;
            }
        }

        foreach (Transform child in gameObject.transform)
        {
            CheckAndDestroyUnsavedItems(child.gameObject, savedObjectIDs);
        }
    }
}

[System.Serializable]
public class SerializationWrapper<T>
{
    public List<T> items;
    public SerializationWrapper() { }
    public SerializationWrapper(List<T> items) { this.items = items; }
}