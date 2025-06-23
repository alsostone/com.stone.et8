using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(GridMapLines))]
public class GridMapLinesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        GridMapLines gridMapLines = target as GridMapLines;
        if (GUILayout.Button("Force Refresh"))
        {
            GUI.changed = true;
        }
        
        if (GUI.changed)
        {
            gridMapLines.GenerateLines();
            
            if (!Application.isPlaying) {
                EditorUtility.SetDirty(gridMapLines);
                EditorSceneManager.MarkSceneDirty(gridMapLines.gameObject.scene);
            }
        }
    }

}
