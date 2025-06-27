using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace ST.GridBuilder
{
    [CustomEditor(typeof(GridMapTiles))]
    public class GridMapTilesEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GridMapTiles gridMapTiles = target as GridMapTiles;
            if (gridMapTiles == null || gridMapTiles.gridMap == null)
            {
                EditorGUILayout.HelpBox("GridMap or GridData is not set.", MessageType.Error);
                return;
            }
            if (GUILayout.Button("Force Refresh"))
            {
                GUI.changed = true;
            }

            if (GUI.changed)
            {
                gridMapTiles.GenerateTiles();

                if (!Application.isPlaying)
                {
                    EditorUtility.SetDirty(gridMapTiles);
                    EditorSceneManager.MarkSceneDirty(gridMapTiles.gameObject.scene);
                }
            }
        }

    }
}