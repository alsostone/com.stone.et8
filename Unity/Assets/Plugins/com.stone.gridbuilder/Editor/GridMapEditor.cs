using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(GridMap))]
public class GridMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GridMap gridMap = target as GridMap;
        
        this.DrawDefaultInspector();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("xLength");
        gridMap.gridData.xLength = EditorGUILayout.IntSlider(gridMap.gridData.xLength, 16, 96);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("zLength");
        gridMap.gridData.zLength = EditorGUILayout.IntSlider(gridMap.gridData.zLength, 16, 96);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Cell Size");
        gridMap.gridData.cellSize = EditorGUILayout.Slider(gridMap.gridData.cellSize, 0.5f, 5.0f);
        GUILayout.EndHorizontal();
        
        if (GUILayout.Button("Force Refresh"))
        {
            GUI.changed = true;
        }
        
        if (GUI.changed)
        {
            gridMap.gridData.ResetCells();
            GenerateObstacle(gridMap);
            GenerateBuilding(gridMap);
            
            if (!Application.isPlaying) {
                EditorUtility.SetDirty(gridMap);
                EditorSceneManager.MarkSceneDirty(gridMap.gameObject.scene);
            }
        }
    }
    
    private void GenerateObstacle(GridMap gridMap)
    {
        GridData gridData = gridMap.gridData;
        for (int x = 0; x < gridData.xLength; x++)
        {
            for (int z = 0; z < gridData.zLength; z++)
            {
                Vector3 pos = gridMap.GetCellPositionCenter(x, z);
                pos.y = gridMap.raycastHeight;

                var offset = gridData.cellSize / 2 * gridMap.raycastFineness;
                if (Physics.Raycast(pos + new Vector3(-offset, 0, -offset), Vector3.down, out RaycastHit _, gridMap.raycastHeight, gridMap.obstacleMask))
                {
                    gridData.SetObstacle(x, z, true);
                }
                else if (Physics.Raycast(pos + new Vector3(offset, 0, -offset), Vector3.down, out RaycastHit _, gridMap.raycastHeight, gridMap.obstacleMask))
                {
                    gridData.SetObstacle(x, z, true);
                }
                else if (Physics.Raycast(pos + new Vector3(-offset, 0, offset), Vector3.down, out RaycastHit _, gridMap.raycastHeight, gridMap.obstacleMask))
                {
                    gridData.SetObstacle(x, z, true);
                }
                else if (Physics.Raycast(pos + new Vector3(offset, 0, offset), Vector3.down, out RaycastHit _, gridMap.raycastHeight, gridMap.obstacleMask))
                {
                    gridData.SetObstacle(x, z, true);
                }
            }
        }
    }

    private void GenerateBuilding(GridMap gridMap)
    {
        Building[] buildings = FindObjectsOfType<Building>();
        foreach (Building building in buildings)
        {
            if (gridMap.TryPlace(building, true))
                EditorUtility.SetDirty(building);
        }
    }

}
