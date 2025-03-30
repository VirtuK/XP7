using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[Serializable]
public class TransformData
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public TransformData() { }

    public TransformData(Transform transform)
    {
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
    }

    public void ApplyTo(Transform transform)
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;
        Debug.Log(transform.gameObject.name + "eu");
        Debug.Log(position);
        Debug.Log(rotation);
        Debug.Log(scale);
    }

    public void ApplyToRec(RectTransform transform)
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;
    }
}

[Serializable]
public class ComponentData
{
    public string type;
    public string jsonData;

    public ComponentData() { }

    public ComponentData(Component component)
    {
        type = component.GetType().AssemblyQualifiedName;
        try
        {
            jsonData = JsonUtility.ToJson(component);
        }
        catch (ArgumentException e)
        {
            Debug.LogError($"Serialization failed for component '{type}' on GameObject '{component.gameObject.name}': {e.Message}");
            jsonData = null; // Or handle as appropriate
        }
    }

    public void ApplyTo(GameObject gameObject)
    {
        Type componentType = Type.GetType(type);
        if (componentType == null)
        {
            Debug.LogWarning($"Component type {type} not found.");
            return;
        }

        Component existingComponent = gameObject.GetComponent(componentType);
        if (existingComponent == null)
        {
            existingComponent = gameObject.AddComponent(componentType);
        }
        JsonUtility.FromJsonOverwrite(jsonData, existingComponent);
    }
}

[Serializable]
public class GameObjectDataList
{
    public List<GameObjectData> items;
}

[Serializable]
public class GameObjectData
{
    public string uniqueID;
    public string name;
    public TransformData transformData;
    public List<ComponentData> componentsData = new List<ComponentData>();
    public List<string> childIDs = new List<string>();
    public List<GameObjectData> childData = new List<GameObjectData>();

    public GameObjectData(GameObject gameObject)
    {
        uniqueID = gameObject.GetInstanceID().ToString();
        name = gameObject.name;
        transformData = new TransformData(gameObject.transform);

        // Serialize components
        foreach (var component in gameObject.GetComponents<Component>())
        {
            if (component != null && !(component is Transform) && component is Item)
            {
                componentsData.Add(new ComponentData(component));
            }
        }

        // Recursively collect child GameObjects
        foreach (Transform child in gameObject.transform)
        {
            if (child != null && child.gameObject.name != "Mensagem")
            {
                childData.Add(new GameObjectData(child.gameObject));
            }
        }
    }

    private bool IsDontDestroyOnLoad(GameObject gameObject)
    {
        return gameObject.scene.name == "DontDestroyOnLoad";
    }

    private bool IsUnityBuiltInComponent(MonoBehaviour component)
    {
        return component.GetType().Namespace == "UnityEngine";
    }

    public bool HasItemComponent(GameObject gameObject)
    {
        return gameObject.GetComponent<Item>() != null;
    }

    private void CollectChildData(Transform parentTransform)
    {
        foreach (Transform child in parentTransform)
        {
            if (child == null) continue;

            // Skip GameObjects named "Canvas" or marked with DontDestroyOnLoad
            if (child.name == "Canvas" || IsDontDestroyOnLoad(child.gameObject) || child.name == "Mensagem")
            {
                continue;
            }

            // Check if the child GameObject is "Player" or has a component derived from Item
            if ((child.name == "Player" || HasItemComponent(child.gameObject)))
            {
                if (child == null) continue;

                // Skip GameObjects marked with DontDestroyOnLoad
                if (IsDontDestroyOnLoad(child.gameObject))
                {
                    continue;
                }

                // Serialize the child GameObject
                GameObjectData childData = new GameObjectData(child.gameObject);
                this.childData.Add(childData);

                // Recursively collect data for this child's children
                childData.CollectChildData(child);
                // Recursively collect data for this child's children
                CollectChildData(child);
            }

            
        }
    }

    public GameObject Reconstruct(Dictionary<string, GameObject> existingObjects)
    {
        GameObject existingGameObject = existingObjects.ContainsKey(uniqueID) ? existingObjects[uniqueID] : null;
        if (existingGameObject == null)
        {
            existingGameObject = GameObject.Find(name);
            existingObjects[uniqueID] = existingGameObject;
            if(existingGameObject == null)
            {
                return null;
            }
         
        }

            // Apply component data
            foreach (var componentData in componentsData)
            {
                componentData.ApplyTo(existingGameObject);
            }
            transformData.ApplyTo(existingGameObject.transform);
 
        // Reconstruct child GameObjects and set their parents after applying the transform
        foreach (var childData in childData)
        {
            GameObject childObject = childData.Reconstruct(existingObjects);
            if(childObject == null)
            {
                continue;
            }
                childData.transformData.ApplyTo(childObject.transform);
            childObject.transform.SetParent(existingGameObject.transform);
        }

        return existingGameObject;
    }
    public void ApplyTo(GameObject gameObject, Dictionary<string, GameObject> allObjects)
    {
        transformData.ApplyTo(gameObject.transform);

        foreach (var componentData in componentsData)
        {
            componentData.ApplyTo(gameObject);
        }

        foreach (string childID in childIDs)
        {
            if (allObjects.TryGetValue(childID, out GameObject childObject))
            {
                childObject.transform.SetParent(gameObject.transform);
            }
        }
    }
}


[System.Serializable]
public class SerializableMeshRenderer
{
    public bool enabled;
    public string materialName;

    public SerializableMeshRenderer(MeshRenderer renderer)
    {
        enabled = renderer.enabled;
        materialName = renderer.sharedMaterial != null ? renderer.sharedMaterial.name : string.Empty;
    }
}