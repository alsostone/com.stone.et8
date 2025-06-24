using System.Collections.Generic;
using UnityEngine;

public partial class GridMap : MonoBehaviour
{
    [SerializeField, Min(0.1f)] public float raycastHeight = 10;
    [SerializeField, Range(0.1f, 2.0f)] public float raycastFineness = 0.5f;
    [SerializeField] public LayerMask obstacleMask;
    [SerializeField] public LayerMask terrainMask;
    [SerializeField, Min(0.01f)] public float yHeight = 0.01f;

    [SerializeField, HideInInspector] public GridData gridData = new();

    public Vector3Int ConvertToIndex(Vector3 point)
    {
        point -= GetPosition();
        point /= gridData.cellSize;
        return new Vector3Int((int)point.x, (int)point.y, (int)point.z);
    }

    public Vector3 GetCellPosition(int x, int z)
    {
        float offset = gridData.cellSize * 0.5f;
        return transform.position + new Vector3(gridData.cellSize * x + offset, yHeight, gridData.cellSize * z + offset);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    
    public Vector3 RaycastPosition(int x, int z)
    {
        Vector3 pos = transform.position + new Vector3(gridData.cellSize * x, 0, gridData.cellSize * z);
        if (Physics.Raycast(new Vector3(pos.x, raycastHeight, pos.z), Vector3.down, out RaycastHit hit, raycastHeight, terrainMask)) {
            pos.y = hit.point.y + yHeight;
        } else {
            pos.y = GetPosition().y + yHeight;
        }
        return pos;
    }
    
    public Vector3 RaycastPosition(Vector3 pos)
    {
        if (Physics.Raycast(new Vector3(pos.x, raycastHeight, pos.z), Vector3.down, out RaycastHit hit, raycastHeight, terrainMask)) {
            pos.y = hit.point.y + yHeight;
        } else {
            pos.y = GetPosition().y + yHeight;
        }
        return pos;
    }
    
    public Vector3 GetPutPosition(BuildingData buildingData)
    {
        int level = 0;
        if (gridData.IsInside(buildingData.x, buildingData.z)) {
            CellData cellData = gridData.GetCell(buildingData.x, buildingData.z);
            level = cellData.contentIds.IndexOf(buildingData.Id);
        }

        float x = gridData.cellSize * (buildingData.x + 0.5f);
        float y = gridData.cellSize * level;
        float z = gridData.cellSize * (buildingData.z + 0.5f);
        return transform.position + new Vector3(x, y, z);
    }
    
    public Vector3 GetLevelPosition(int x, int z, int level)
    {
        float x1 = gridData.cellSize * (x + 0.5f);
        float y1 = gridData.cellSize * level;
        float z1 = gridData.cellSize * (z + 0.5f);
        return transform.position + new Vector3(x1, y1, z1);
    }
    
#if UNITY_EDITOR
    private readonly float yOffset = 0.01f;
    private readonly List<Vector3> drawPoints = new List<Vector3>();
    void OnDrawGizmos()
    {
        if (gridData.cells == null || gridData.cells.Length != gridData.xLength * gridData.zLength) {
            return;
        }
        drawPoints.Clear();

        int xLength = gridData.xLength;
        int zLength = gridData.zLength;
        float size = gridData.cellSize;
        
        Gizmos.color = Color.yellow;
        for (int x = 0; x < xLength + 1; x++)
        {
            for (int z = 0; z < zLength; ++z)
            {
                if ((x - 1 < 0 || gridData.cells[x - 1 + z * xLength].isFill)
                    && (x >= xLength || gridData.cells[x + z * xLength].isFill)) {
                    continue;
                }
                Vector3 start = GetPosition() + new Vector3(x * size, yOffset, z * size);
                drawPoints.Add(start);
                
                for (; z < zLength; ++z)
                {
                    Vector3 end = GetPosition() + new Vector3(x * size, yOffset, z * size);
                    drawPoints.Add(end);
                    if ((x - 1 >= 0 && !gridData.cells[x - 1 + z * xLength].isFill)
                        || (x < xLength && !gridData.cells[x + z * xLength].isFill)) {
                        continue;
                    }
                    Gizmos.DrawLine(start, end);
                    break;
                }

                if (z == zLength)
                {
                    Vector3 end = GetPosition() + new Vector3(x * size, yOffset, zLength * size);
                    drawPoints.Add(end);
                    Gizmos.DrawLine(start, end);
                }
            }
        }
        
        for (int z = 0; z < zLength + 1; z++)
        {
            for (int x = 0; x < xLength; ++x)
            {
                if ((z - 1 < 0 || gridData.cells[x + (z - 1) * xLength].isFill)
                    && (z >= zLength || gridData.cells[x + z * xLength].isFill)) {
                    continue;
                }
                Vector3 start = GetPosition() + new Vector3(x * size, yOffset, z * size);
                
                for (; x < xLength; ++x)
                {
                    if ((z - 1 >= 0 && !gridData.cells[x + (z - 1) * xLength].isFill)
                        || (z < zLength && !gridData.cells[x + z * xLength].isFill)) {
                        continue;
                    }
                    Vector3 end = GetPosition() + new Vector3(x * size, yOffset, z * size);
                    Gizmos.DrawLine(start, end);
                    break;
                }

                if (x == xLength)
                {
                    Vector3 end = GetPosition() + new Vector3(xLength * size, yOffset, z * size);
                    Gizmos.DrawLine(start, end);
                }
            }
        }
        
        Gizmos.color = Color.green;
        foreach (Vector3 point in drawPoints)
        {
            Gizmos.DrawSphere(point, 0.1f);
        }
    }
#endif
    
}