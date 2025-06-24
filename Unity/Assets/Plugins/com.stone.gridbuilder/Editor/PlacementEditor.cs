using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Placement))]
public class PlacementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        Placement placement = target as Placement;
        GUILayout.Label($"Id({placement.placementData.Id})  Shape", EditorStyles.boldLabel);
        for (int z = PlacementData.height - 1; z >= 0; z--)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < PlacementData.width; x++)
            {
                int index = x + z * PlacementData.width;
                placement.placementData.points[index] = EditorGUILayout.Toggle(placement.placementData.points[index], GUILayout.MaxWidth(20));
            }
            GUILayout.EndHorizontal();
        }
        
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Rotate -180°"))
            placement.placementData.Rotation(-2);
        if (GUILayout.Button("Rotate -90°"))
            placement.placementData.Rotation(-1);
        if (GUILayout.Button("Rotate 90°"))
            placement.placementData.Rotation(1);
        if (GUILayout.Button("Rotate 180°"))
            placement.placementData.Rotation(2);
        GUILayout.EndHorizontal();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
        serializedObject.ApplyModifiedProperties();
    }

}
