using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
[System.Serializable]
public class SceneChanger : MonoBehaviour
{
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
    }

    public IEnumerator changeScene(SceneAsset scene)
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(scene.name);
    }
}
