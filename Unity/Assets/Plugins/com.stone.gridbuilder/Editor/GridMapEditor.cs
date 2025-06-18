using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(GridMap))]
public class GridMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        this.DrawDefaultInspector();
        
        GridMap grid = target as GridMap;
        if (GUILayout.Button("Force Refresh"))
        {
            GUI.changed = true;
        }
        
        if (GUI.changed)
        {
            GenerateGrid(grid);
            GenerateObstacle(grid);
            EditorUtility.SetDirty(grid);
            EditorSceneManager.MarkSceneDirty(grid.gameObject.scene);
        }
    }
    
    private void GenerateGrid(GridMap grid)
    {
        if (grid.cells == null || grid.xLength * grid.zLength != grid.cells.Length)
        {
            grid.cells = new GridCell[grid.xLength * grid.zLength];
            for (int x = 0; x < grid.xLength; x++) {
                for (int z = 0; z < grid.zLength; z++) {
                    grid.cells[x + z * grid.xLength] = new GridCell(x, z);
                }
            }
        }
        else
        {
            for (int x = 0; x < grid.xLength; x++) {
                for (int z = 0; z < grid.zLength; z++) {
                    grid.cells[x + z * grid.xLength].isObstacle = false;
                }
            }
        }
    }

    private void GenerateObstacle(GridMap grid)
    {
        for (int x = 0; x < grid.xLength; x++)
        {
            for (int z = 0; z < grid.zLength; z++)
            {
                Vector3 pos = grid.GetCellPositionCenter(x, z);
                pos.y = grid.raycastHeight;

                var offset = grid.cellSize / 2 * grid.raycastFineness;
                if (Physics.Raycast(pos + new Vector3(-offset, 0, -offset), Vector3.down, out RaycastHit _, grid.raycastHeight, grid.obstacleMask))
                {
                    grid.cells[x + z * grid.xLength].isObstacle = true;
                }
                else if (Physics.Raycast(pos + new Vector3(offset, 0, -offset), Vector3.down, out RaycastHit _, grid.raycastHeight, grid.obstacleMask))
                {
                    grid.cells[x + z * grid.xLength].isObstacle = true;
                }
                else if (Physics.Raycast(pos + new Vector3(-offset, 0, offset), Vector3.down, out RaycastHit _, grid.raycastHeight, grid.obstacleMask))
                {
                    grid.cells[x + z * grid.xLength].isObstacle = true;
                }
                else if (Physics.Raycast(pos + new Vector3(offset, 0, offset), Vector3.down, out RaycastHit _, grid.raycastHeight, grid.obstacleMask))
                {
                    grid.cells[x + z * grid.xLength].isObstacle = true;
                }
            }
        }
    }

}
