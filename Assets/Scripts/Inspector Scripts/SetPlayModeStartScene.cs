using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[InitializeOnLoad]
public static class SetPlayModeStartScene
{
    static SetPlayModeStartScene()
    {
        string startScenePath = "Assets/Scenes/LabCriogenia.unity";

        SceneAsset startScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(startScenePath);
        if (startScene != null)
        {
            EditorSceneManager.playModeStartScene = startScene;
            Debug.Log($"Play mode start scene set to: {startScenePath}");
        }
        else
        {
            Debug.LogError($"Could not find scene at: {startScenePath}");
        }
    }
}