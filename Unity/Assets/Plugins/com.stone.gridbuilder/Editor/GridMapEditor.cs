#if UNITY_EDITOR
using System.IO;
using MemoryPack;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

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
            GUILayout.Label("xLength", GUILayout.Width(EditorGUIUtility.labelWidth));
            gridMap.gridData.xLength = EditorGUILayout.IntSlider(gridMap.gridData.xLength, 10, 100);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("zLength", GUILayout.Width(EditorGUIUtility.labelWidth));
            gridMap.gridData.zLength = EditorGUILayout.IntSlider(gridMap.gridData.zLength, 10, 100);
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
                gridMap.gridData.ResetCells();
                GenerateObstacle(gridMap);
                GeneratePlacement(gridMap);
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
                GridMapData data = new GridMapData
                {
                    position = gridMap.transform.position.ToTSVector(),
                    rotation = gridMap.transform.eulerAngles.ToTSVector(),
                    gridData = gridMap.gridData
                };
                byte[] gridMapBytes = MemoryPackSerializer.Serialize(data);
                var folder = EditorUtility.OpenFolderPanel("Save Folder Select", Application.dataPath, "");
                File.WriteAllBytes($"{folder}/{SceneManager.GetActiveScene().name}.bytes", gridMapBytes);
            }
        }
        
        private void GenerateObstacle(GridMap gridMap)
        {
            GridData gridData = gridMap.gridData;
            for (int x = 0; x < gridData.xLength; x++)
            {
                for (int z = 0; z < gridData.zLength; z++)
                {
                    Vector3 pos = new Vector3((x + 0.5f) * gridData.cellSize, 0, (z + 0.5f) * gridData.cellSize);
                    float offset = gridData.cellSize / 2.0f * gridMap.raycastFineness;
                    if (gridMap.Raycast(pos + new Vector3(-offset, 0, -offset), gridMap.obstacleMask))
                    {
                        gridData.SetObstacle(x, z, true);
                    }
                    else if (gridMap.Raycast(pos + new Vector3(offset, 0, -offset), gridMap.obstacleMask))
                    {
                        gridData.SetObstacle(x, z, true);
                    }
                    else if (gridMap.Raycast(pos + new Vector3(-offset, 0, offset), gridMap.obstacleMask))
                    {
                        gridData.SetObstacle(x, z, true);
                    }
                    else if (gridMap.Raycast(pos + new Vector3(offset, 0, offset), gridMap.obstacleMask))
                    {
                        gridData.SetObstacle(x, z, true);
                    }
                }
            }
        }

        private void GeneratePlacement(GridMap gridMap)
        {
            Placement[] placements = FindObjectsOfType<Placement>();
            foreach (Placement placement in placements)
            {
                IndexV2 index = gridMap.ConvertToIndex(placement.transform.position);
                if (!gridMap.gridData.CanPut(index.x, index.z, placement.placementData)) {
                    continue;
                }

                placement.placementData.id = gridMap.gridData.GetNextGuid();
                gridMap.gridData.Put(index.x, index.z, placement.placementData);
                placement.SetPosition(gridMap.GetPutPosition(placement.placementData));
                EditorUtility.SetDirty(placement);
            }
        }
        
    }
}
#endif