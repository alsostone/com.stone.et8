using System.Collections.Generic;
using UnityEngine;

public partial class GridMap : MonoBehaviour
{
    [SerializeField, Range(16, 96)] public int xLength = 16;
    [SerializeField, Range(16, 96)] public int zLength = 16;
    [SerializeField, Range(0.5f, 5.0f)] public float cellSize = 1;
    
    [SerializeField, Min(0.1f)] public float raycastHeight = 10;
    [SerializeField, Range(0.1f, 2.0f)] public float raycastFineness = 0.5f;
    [SerializeField] public LayerMask obstacleMask;
    
    [SerializeField, HideInInspector] public GridCell[] cells;
    
    public Vector3Int ConvertToIndex(Vector3 point)
    {
        point -= GetPosition();
        point /= cellSize;
        return new Vector3Int((int)point.x, (int)point.y, (int)point.z);
    }

    public bool CheckInside(int x, int z)
    {
        return x >= 0 && z >= 0 && x < this.xLength && z < this.zLength;
    }
    
    public Vector3 GetCellPositionCenter(int x, int z)
    {
        float offset = cellSize * 0.5f;
        return transform.position + new Vector3(cellSize * x + offset, 0, cellSize * z + offset);
    }
    
    public Vector3 GetCellPosition(int x, int z)
    {
        return transform.position + new Vector3(cellSize * x, 0, cellSize * z);
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

#if UNITY_EDITOR
    private readonly float yOffset = 0.01f;
    private readonly List<Vector3> drawPoints = new List<Vector3>();
    void OnDrawGizmos()
    {
        if (cells == null || cells.Length != xLength * zLength) {
            return;
        }
        drawPoints.Clear();
        
        Gizmos.color = Color.yellow;
        for (int x = 0; x < this.xLength + 1; x++)
        {
            for (int z = 0; z < this.zLength; ++z)
            {
                if ((x - 1 < 0 || this.cells[x - 1 + z * this.xLength].isObstacle)
                    && (x >= this.xLength || this.cells[x + z * this.xLength].isObstacle)) {
                    continue;
                }
                Vector3 start = this.GetPosition() + new Vector3(x * this.cellSize, yOffset, z * this.cellSize);
                drawPoints.Add(start);
                
                for (; z < this.zLength; ++z)
                {
                    Vector3 end = this.GetPosition() + new Vector3(x * this.cellSize, yOffset, z * this.cellSize);
                    if ((x - 1 >= 0 && !this.cells[x - 1 + z * this.xLength].isObstacle)
                        || (x < this.xLength && !this.cells[x + z * this.xLength].isObstacle)) {
                        drawPoints.Add(end);
                        continue;
                    }
                    Gizmos.DrawLine(start, end);
                    break;
                }

                if (z == this.zLength)
                {
                    Vector3 end = new Vector3(start.x, yOffset, this.zLength * cellSize);
                    drawPoints.Add(end);
                    Gizmos.DrawLine(start, end);
                }
            }
        }
        
        for (int z = 0; z < this.zLength + 1; z++)
        {
            for (int x = 0; x < this.xLength; ++x)
            {
                if ((z - 1 < 0 || this.cells[x + (z - 1) * this.xLength].isObstacle)
                    && (z >= this.zLength || this.cells[x + z * this.xLength].isObstacle)) {
                    continue;
                }
                Vector3 start = this.GetPosition() + new Vector3(x * this.cellSize, yOffset, z * this.cellSize);
                
                for (; x < this.xLength; ++x)
                {
                    Vector3 end = this.GetPosition() + new Vector3(x * this.cellSize, yOffset, z * this.cellSize);
                    if ((z - 1 >= 0 && !this.cells[x + (z - 1) * this.xLength].isObstacle)
                        || (z < this.zLength && !this.cells[x + z * this.xLength].isObstacle)) {
                        drawPoints.Add(end);
                        continue;
                    }
                    Gizmos.DrawLine(start, end);
                    break;
                }

                if (x == this.xLength)
                {
                    Vector3 end = new Vector3(this.xLength * cellSize, yOffset, start.z);
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