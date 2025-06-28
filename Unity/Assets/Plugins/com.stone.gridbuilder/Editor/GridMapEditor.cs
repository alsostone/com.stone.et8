using System.IO;
using MemoryPack;
using TrueSync;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace ST.GridBuilder
{
    [CustomEditor(typeof(GridMap))]
    public class GridMapEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            GridMap gridMap = target as GridMap;
            if (gridMap == null || gridMap.gridData == null)
            {
                EditorGUILayout.HelpBox("GridMap or GridData is not set.", MessageType.Error);
                return;
            }
            GUILayout.BeginHorizontal();
            GUILayout.Label("xPosition", GUILayout.Width(EditorGUIUtility.labelWidth));
            gridMap.gridData.xPosition = EditorGUILayout.IntField(gridMap.gridData.xPosition);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("zPosition", GUILayout.Width(EditorGUIUtility.labelWidth));
            gridMap.gridData.zPosition = EditorGUILayout.IntField(gridMap.gridData.zPosition);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("xLength", GUILayout.Width(EditorGUIUtility.labelWidth));
            gridMap.gridData.xLength = EditorGUILayout.IntSlider(gridMap.gridData.xLength, 16, 96);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("zLength", GUILayout.Width(EditorGUIUtility.labelWidth));
            gridMap.gridData.zLength = EditorGUILayout.IntSlider(gridMap.gridData.zLength, 16, 96);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Cell Size", GUILayout.Width(EditorGUIUtility.labelWidth));
            gridMap.gridData.cellSize = EditorGUILayout.IntSlider(gridMap.gridData.cellSize, 1, 5);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("block Level Max", GUILayout.Width(EditorGUIUtility.labelWidth));
            gridMap.gridData.blockLevelMax = EditorGUILayout.IntField(gridMap.gridData.blockLevelMax);
            GUILayout.EndHorizontal();
            
            if (GUILayout.Button("Force Refresh"))
            {
                GUI.changed = true;
            }
            
            if (GUI.changed)
            {
                gridMap.transform.position = new Vector3(gridMap.gridData.xPosition, 0, gridMap.gridData.zPosition);
                gridMap.gridData.ResetCells();
                GenerateObstacle(gridMap);
                GenerateBuilding(gridMap);
                EditorUtility.SetDirty(gridMap);

                GridMapLines lines = FindObjectOfType<GridMapLines>();
                if (lines != null)
                {
                    lines.GenerateLines();
                    EditorUtility.SetDirty(lines);
                }
                GridMapTiles tiles = FindObjectOfType<GridMapTiles>();
                if (tiles != null)
                {
                    tiles.GenerateTiles();
                    EditorUtility.SetDirty(tiles);
                }
                
                if (!Application.isPlaying) {
                    EditorSceneManager.MarkSceneDirty(gridMap.gameObject.scene);
                }
            }
            
            if (GUILayout.Button("MemoryPack Serialize"))
            {
                byte[] gridBytes = MemoryPackSerializer.Serialize(gridMap.gridData);
                var folder = EditorUtility.OpenFolderPanel("Save Folder Select", Application.dataPath, "");
                File.WriteAllBytes(folder + "/GridData.bin", gridBytes);
            }
        }
        
        private void GenerateObstacle(GridMap gridMap)
        {
            GridData gridData = gridMap.gridData;
            for (int x = 0; x < gridData.xLength; x++)
            {
                for (int z = 0; z < gridData.zLength; z++)
                {
                    Vector3 pos = gridMap.GetCellPosition(x, z);
                    pos.y = gridMap.raycastHeight;

                    float offset = gridData.cellSize / 2.0f * gridMap.raycastFineness;
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
            Placement[] buildings = FindObjectsOfType<Placement>();
            foreach (Placement building in buildings)
            {
                IndexV2 index = gridMap.ConvertToIndex(building.transform.position);
                if (!gridMap.gridData.CanPut(index.x, index.z, building.placementData)) {
                    continue;
                }

                building.placementData.id = gridMap.gridData.GetNextGuid();
                gridMap.gridData.Put(index.x, index.z, building.placementData);
                building.SetPutPosition(gridMap.GetPutPosition(building.placementData));
                EditorUtility.SetDirty(building);
            }
        }
        
    }
}