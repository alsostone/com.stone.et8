using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public partial class GridMap : MonoBehaviour
{
    [SerializeField, Min(0.1f)] public float raycastHeight = 10;
    [SerializeField, Range(0.1f, 2.0f)] public float raycastFineness = 0.5f;
    [SerializeField] public LayerMask obstacleMask;
    [SerializeField] public LayerMask terrainMask;
    
    [SerializeField, HideInInspector] public GridData gridData = new GridData();
    
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
        Vector3 pos = new Vector3(gridData.cellSize * x, raycastHeight, gridData.cellSize * z);
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

#if UNITY_EDITOR
    private readonly float yOffset = 0.01f;
    private readonly List<Vector3> drawPoints = new List<Vector3>();
    void OnDrawGizmos()
    {
        if (gridData.cells == null || gridData.cells.Length != gridData.xLength * gridData.zLength) {
            return;
        }
        drawPoints.Clear();
        
        Gizmos.color = Color.yellow;
        for (int x = 0; x < this.gridData.xLength + 1; x++)
        {
            for (int z = 0; z < this.gridData.zLength; ++z)
            {
                if ((x - 1 < 0 || this.gridData.cells[x - 1 + z * this.gridData.xLength].isObstacle)
                    && (x >= this.gridData.xLength || this.gridData.cells[x + z * this.gridData.xLength].isObstacle)) {
                    continue;
                }
                Vector3 start = this.GetPosition() + new Vector3(x * this.gridData.cellSize, yOffset, z * this.gridData.cellSize);
                drawPoints.Add(start);
                
                for (; z < this.gridData.zLength; ++z)
                {
                    Vector3 end = this.GetPosition() + new Vector3(x * this.gridData.cellSize, yOffset, z * this.gridData.cellSize);
                    drawPoints.Add(end);
                    if ((x - 1 >= 0 && !this.gridData.cells[x - 1 + z * this.gridData.xLength].isObstacle)
                        || (x < this.gridData.xLength && !this.gridData.cells[x + z * this.gridData.xLength].isObstacle)) {
                        continue;
                    }
                    Gizmos.DrawLine(start, end);
                    break;
                }

                if (z == this.gridData.zLength)
                {
                    Vector3 end = new Vector3(start.x, yOffset, this.gridData.zLength * this.gridData.cellSize);
                    drawPoints.Add(end);
                    Gizmos.DrawLine(start, end);
                }
            }
        }
        
        for (int z = 0; z < this.gridData.zLength + 1; z++)
        {
            for (int x = 0; x < this.gridData.xLength; ++x)
            {
                if ((z - 1 < 0 || this.gridData.cells[x + (z - 1) * this.gridData.xLength].isObstacle)
                    && (z >= this.gridData.zLength || this.gridData.cells[x + z * this.gridData.xLength].isObstacle)) {
                    continue;
                }
                Vector3 start = this.GetPosition() + new Vector3(x * this.gridData.cellSize, yOffset, z * this.gridData.cellSize);
                
                for (; x < this.gridData.xLength; ++x)
                {
                    if ((z - 1 >= 0 && !this.gridData.cells[x + (z - 1) * this.gridData.xLength].isObstacle)
                        || (z < this.gridData.zLength && !this.gridData.cells[x + z * this.gridData.xLength].isObstacle)) {
                        continue;
                    }
                    Vector3 end = this.GetPosition() + new Vector3(x * this.gridData.cellSize, yOffset, z * this.gridData.cellSize);
                    Gizmos.DrawLine(start, end);
                    break;
                }

                if (x == this.gridData.xLength)
                {
                    Vector3 end = new Vector3(this.gridData.xLength * this.gridData.cellSize, yOffset, start.z);
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