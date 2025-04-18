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

    public IEnumerator changeScene(string scene)
    {
        yield return new WaitForSeconds(2f);
        if(scene == "sair")
        {
            Application.Quit();
        }
        else SceneManager.LoadScene(scene);
    }
}
