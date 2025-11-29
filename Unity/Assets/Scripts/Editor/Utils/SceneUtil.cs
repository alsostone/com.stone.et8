using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

static class SceneUtil
{
    [MenuItem("ET/Util/StartApp &r", false, 25)]
    public static void StartApp()
    {
        SwitchApp(false);
    }
    
    [MenuItem("ET/Util/StartBattle #&r", false, 26)]
    private static void StartBattle()
    {
        SwitchApp(true);
    }

    private static void SwitchApp(bool isLocalBattle = false)
    {
        if (EditorApplication.isPlaying)
        {
            EditorApplication.isPlaying = false;
        }
        else if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Init.unity");
            PlayerPrefs.SetInt("G_StartLocalBattle", isLocalBattle ? 1 : 0);
            EditorApplication.isPlaying = true;
        }
    }
}