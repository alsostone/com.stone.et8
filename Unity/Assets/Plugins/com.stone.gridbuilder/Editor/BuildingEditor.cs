using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Building))]
public class BuildingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        Building building = target as Building;
        GUILayout.Label($"Id({building.buildingData.Id})  Shape", EditorStyles.boldLabel);
        for (int z = BuildingData.height - 1; z >= 0; z--)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < BuildingData.width; x++)
            {
                int index = x + z * BuildingData.width;
                building.buildingData.points[index] = EditorGUILayout.Toggle(building.buildingData.points[index], GUILayout.MaxWidth(20));
            }
            GUILayout.EndHorizontal();
        }
        
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Rotate -180°"))
            building.buildingData.Rotation(-2);
        if (GUILayout.Button("Rotate -90°"))
            building.buildingData.Rotation(-1);
        if (GUILayout.Button("Rotate 90°"))
            building.buildingData.Rotation(1);
        if (GUILayout.Button("Rotate 180°"))
            building.buildingData.Rotation(2);
        GUILayout.EndHorizontal();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
        serializedObject.ApplyModifiedProperties();
    }

}
