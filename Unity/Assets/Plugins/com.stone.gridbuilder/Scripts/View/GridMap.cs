using System.Collections.Generic;
using UnityEngine;

public partial class GridMap : MonoBehaviour
{
    [SerializeField, Min(0.1f)] public float raycastHeight = 10;
    [SerializeField, Range(0.1f, 2.0f)] public float raycastFineness = 0.5f;
    [SerializeField] public LayerMask obstacleMask;
    [SerializeField] public LayerMask terrainMask;
    
    [SerializeField, HideInInspector] public GridData gridData = new();

    public Vector3Int ConvertToIndex(Vector3 point)
    {
        point -= GetPosition();
        point /= this.gridData.cellSize;
        return new Vector3Int((int)point.x, (int)point.y, (int)point.z);
    }

    public Vector3 GetCellPositionCenter(int x, int z)
    {
        float offset = this.gridData.cellSize * 0.5f;
        return transform.position + new Vector3(this.gridData.cellSize * x + offset, 0, this.gridData.cellSize * z + offset);
    }
    
    public Vector3 GetCellPosition(int x, int z)
    {
        return transform.position + new Vector3(this.gridData.cellSize * x, 0, this.gridData.cellSize * z);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
    
    public Vector3 RaycastPosition(int x, int z)
    {
        Vector3 pos = GetCellPosition(x, z);
        if (Physics.Raycast(pos, Vector3.down, out RaycastHit hit, raycastHeight, terrainMask)) {
            pos.y = hit.point.y + 0.01f;
        } else {
            pos.y = GetPosition().y + 0.01f;
        }
        return pos;
    }
    
    public Vector3 RaycastPosition(Vector3 pos)
    {
        if (Physics.Raycast(new Vector3(pos.x, raycastHeight, pos.z), Vector3.down, out RaycastHit hit, raycastHeight, terrainMask)) {
            pos.y = hit.point.y + 0.01f;
        } else {
            pos.y = GetPosition().y + 0.01f;
        }
        return pos;
    }
    
    public bool TryPlace(Building building, bool resetPosition = true)
    {
        Vector3Int index = ConvertToIndex(building.transform.position);
        if (!gridData.CanPut(index.x, index.z, building.buildingData)) {
            return false;
        }

        building.buildingData.Id = gridData.GetNextGuid();
        gridData.Put(index.x, index.z, building.buildingData);
        
        if (resetPosition) {
            building.transform.position = GetCellPositionCenter(index.x, index.z);
        }
        return true;
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

        int xLength = this.gridData.xLength;
        int zLength = this.gridData.zLength;
        float size = this.gridData.cellSize;
        
        Gizmos.color = Color.yellow;
        for (int x = 0; x < xLength + 1; x++)
        {
            for (int z = 0; z < zLength; ++z)
            {
                if ((x - 1 < 0 || this.gridData.cells[x - 1 + z * xLength].isFill)
                    && (x >= xLength || this.gridData.cells[x + z * xLength].isFill)) {
                    continue;
                }
                Vector3 start = this.GetPosition() + new Vector3(x * size, yOffset, z * size);
                drawPoints.Add(start);
                
                for (; z < zLength; ++z)
                {
                    Vector3 end = this.GetPosition() + new Vector3(x * size, yOffset, z * size);
                    drawPoints.Add(end);
                    if ((x - 1 >= 0 && !this.gridData.cells[x - 1 + z * xLength].isFill)
                        || (x < xLength && !this.gridData.cells[x + z * xLength].isFill)) {
                        continue;
                    }
                    Gizmos.DrawLine(start, end);
                    break;
                }

                if (z == zLength)
                {
                    Vector3 end = this.GetPosition() + new Vector3(x * size, yOffset, zLength * size);
                    drawPoints.Add(end);
                    Gizmos.DrawLine(start, end);
                }
            }
        }
        
        for (int z = 0; z < zLength + 1; z++)
        {
            for (int x = 0; x < xLength; ++x)
            {
                if ((z - 1 < 0 || this.gridData.cells[x + (z - 1) * xLength].isFill)
                    && (z >= zLength || this.gridData.cells[x + z * xLength].isFill)) {
                    continue;
                }
                Vector3 start = this.GetPosition() + new Vector3(x * size, yOffset, z * size);
                
                for (; x < xLength; ++x)
                {
                    if ((z - 1 >= 0 && !this.gridData.cells[x + (z - 1) * xLength].isFill)
                        || (z < zLength && !this.gridData.cells[x + z * xLength].isFill)) {
                        continue;
                    }
                    Vector3 end = this.GetPosition() + new Vector3(x * size, yOffset, z * size);
                    Gizmos.DrawLine(start, end);
                    break;
                }

                if (x == xLength)
                {
                    Vector3 end = this.GetPosition() + new Vector3(xLength * size, yOffset, z * size);
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