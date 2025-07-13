#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace ST.GridBuilder
{
    [CustomEditor(typeof(GridMapLines))]
    public class GridMapLinesEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GridMapLines gridMapLines = target as GridMapLines;
            if (gridMapLines == null || gridMapLines.gridMap == null)
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
                gridMapLines.GenerateLines();

                if (!Application.isPlaying)
                {
                    EditorUtility.SetDirty(gridMapLines);
                    EditorSceneManager.MarkSceneDirty(gridMapLines.gameObject.scene);
                }
            }
        }

    }
}
#endif