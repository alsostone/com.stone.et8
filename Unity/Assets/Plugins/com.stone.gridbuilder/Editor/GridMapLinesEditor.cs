using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(GridMapLines))]
public class GridMapLinesEditor : Editor
{
    public override void OnInspectorGUI()
    {
        this.DrawDefaultInspector();
        
        GridMapLines gridMapLines = target as GridMapLines;
        if (GUILayout.Button("Force Refresh"))
        {
            GUI.changed = true;
        }
        
        if (GUI.changed)
        {
            GenerateLines(gridMapLines);
            
            if (!Application.isPlaying) {
                EditorUtility.SetDirty(gridMapLines);
                EditorSceneManager.MarkSceneDirty(gridMapLines.gameObject.scene);
            }
        }
    }

    void GenerateLines(GridMapLines lines)
    {
        GridData gridData = lines.gridMap.gridData;
        List<Vector3> positions = new();
        
        Vector3 pos = lines.gridMap.GetPosition();
        int dir = 1;
        for (int z = 0; z < gridData.zLength + 1; z++)
        {
            pos = RaycastPosition(lines, pos);
            positions.Add(pos);
            
            for(int x = 1; x < gridData.xLength + 1; x++)
            {
                pos.x += dir * gridData.cellSize;
                pos = RaycastPosition(lines, pos);
                positions.Add(pos);
            }
            dir *= -1;
            pos.z += gridData.cellSize;
        }
        
        pos = lines.gridMap.GetPosition() + new Vector3(0, 0, gridData.cellSize * gridData.zLength);
        dir = -1;
        for (int index = 0; index < gridData.xLength + 1; index++)
        {
            pos = RaycastPosition(lines, pos);
            positions.Add(pos);
            
            for (int j = 0; j < gridData.zLength; j++)
            {
                pos.z += dir * gridData.cellSize;
                pos = RaycastPosition(lines, pos);
                positions.Add(pos);
            }
            dir *= -1;
            pos.x += gridData.cellSize;
        }

        LineRenderer lineRenderer = lines.GetComponent<LineRenderer>();
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
    
    private Vector3 RaycastPosition(GridMapLines lines, Vector3 pos)
    {
        GridMap grid = lines.gridMap;
        float distance = grid.raycastHeight - grid.GetPosition().y;
        pos.y = grid.raycastHeight;
        pos.y = Physics.Raycast(pos, Vector3.down, out RaycastHit hit, distance, lines.terrainMask) ? hit.point.y : grid.GetPosition().y;
        pos.y += lines.lineHeight;
        return pos;
    }
}
