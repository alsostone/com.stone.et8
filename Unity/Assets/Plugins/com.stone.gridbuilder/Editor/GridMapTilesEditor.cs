using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(GridMapTiles))]
public class GridMapTilesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        GridMapTiles gridMapTiles = target as GridMapTiles;
        if (GUILayout.Button("Force Refresh"))
        {
            GUI.changed = true;
        }
        
        if (GUI.changed)
        {
            gridMapTiles.GenerateTiles();
            
            if (!Application.isPlaying) {
                EditorUtility.SetDirty(gridMapTiles);
                EditorSceneManager.MarkSceneDirty(gridMapTiles.gameObject.scene);
            }
        }
    }
    
}
