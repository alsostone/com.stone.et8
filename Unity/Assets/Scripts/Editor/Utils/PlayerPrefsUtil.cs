using UnityEditor;
using UnityEngine;

static class PlayerPrefsUtil
{
    [MenuItem("ET/PlayerPrefs/ClearAll", false, 20)]
    private static void ClearAll()
    {
        PlayerPrefs.DeleteAll();
    }
}