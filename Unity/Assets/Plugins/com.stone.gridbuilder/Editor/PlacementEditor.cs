using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Placement))]
public class PlacementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Placement placement = target as Placement;
        GUILayout.BeginHorizontal();
        GUILayout.Label("Placement Type", GUILayout.Width(EditorGUIUtility.labelWidth));
        placement.placementData.placementType = (PlacedLayer)EditorGUILayout.EnumFlagsField(placement.placementData.placementType);
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Placed Layer", GUILayout.Width(EditorGUIUtility.labelWidth));
        placement.placementData.placedLayer = (PlacedLayer)EditorGUILayout.EnumFlagsField(placement.placementData.placedLayer);
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Id", GUILayout.Width(EditorGUIUtility.labelWidth));
        GUILayout.Label(placement.placementData.Id.ToString());
        GUILayout.EndHorizontal();
        
        GUILayout.Label($"Shape", EditorStyles.boldLabel);
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
        serializedObject.ApplyModifiedProperties();
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

}
