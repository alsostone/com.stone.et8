using System;
using System.Collections.Generic;
using UnityEngine;

namespace ST.GridBuilder
{
    public class GridMap : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] public float raycastHeight = 10;
        [SerializeField, Range(0.1f, 2.0f)] public float raycastFineness = 0.5f;
        [SerializeField] public LayerMask obstacleMask;
        [SerializeField] public LayerMask terrainMask;
        [SerializeField, Min(0.01f)] public float yHeight = 0.01f;

        [SerializeField, HideInInspector] public GridData gridData = new();

        public IndexV2 ConvertToIndex(Vector3 position)
        {
            FieldV2 pos = position.ToFieldV2();
            return gridData.ConvertToIndex(ref pos);
        }
        
        public Vector3 GetCellPosition(int x, int z)
        {
            return gridData.GetCellPosition(x, z).ToVector3();
        }

        public Vector3 GetPosition()
        {
            return new Vector3(gridData.xPosition, 0, gridData.zPosition);
        }
        
        public void SetDestination(Vector3 position)
        {
            gridData.SetDestination(new FieldV2(position.x, position.z));
        }

        public Vector3 GetFieldVector(Vector3 position)
        {
            FieldV2 v2 = gridData.GetFieldVector(new FieldV2(position.x, position.z));
            return new Vector3(v2.x, 0, v2.z);
        }
        
        public Vector3 RaycastPosition(int x, int z)
        {
            Vector3 pos = GetPosition() + new Vector3(gridData.cellSize * x, 0, gridData.cellSize * z);
            if (Physics.Raycast(new Vector3(pos.x, raycastHeight, pos.z), Vector3.down, out RaycastHit hit, raycastHeight, terrainMask)) {
                pos.y = hit.point.y + yHeight;
            } else {
                pos.y = yHeight;
            }
            return pos;
        }
        
        public Vector3 RaycastPosition(Vector3 pos)
        {
            if (Physics.Raycast(new Vector3(pos.x, raycastHeight, pos.z), Vector3.down, out RaycastHit hit, raycastHeight, terrainMask)) {
                pos.y = hit.point.y + yHeight;
            } else {
                pos.y = yHeight;
            }
            return pos;
        }
        
        public Vector3 GetPutPosition(PlacementData placementData)
        {
            CellData cellData = gridData.GetCell(placementData.x, placementData.z);
            int level = cellData?.contentIds.IndexOf(placementData.id) ?? 0 ;

            float x = gridData.cellSize * (placementData.x + 0.5f);
            float y = gridData.cellSize * level;
            float z = gridData.cellSize * (placementData.z + 0.5f);
            return GetPosition() + new Vector3(x, y, z);
        }
        
        public Vector3 GetLevelPosition(int x, int z, int level)
        {
            float x1 = gridData.cellSize * (x + 0.5f);
            float y1 = gridData.cellSize * level;
            float z1 = gridData.cellSize * (z + 0.5f);
            return GetPosition() + new Vector3(x1, y1, z1);
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
            for (int z = 0; z < zLength; ++z)
            {
                if ((x - 1 < 0 || gridData.cells[x - 1 + z * xLength].IsFill)
                    && (x >= xLength || gridData.cells[x + z * xLength].IsFill)) {
                    continue;
                }
                Vector3 start = GetPosition() + new Vector3(x * size, yOffset, z * size);
                drawPoints.Add(start);
                
                for (; z < zLength; ++z)
                {
                    Vector3 end = GetPosition() + new Vector3(x * size, yOffset, z * size);
                    drawPoints.Add(end);
                    if ((x - 1 >= 0 && !gridData.cells[x - 1 + z * xLength].IsFill)
                        || (x < xLength && !gridData.cells[x + z * xLength].IsFill)) {
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

            for (int z = 0; z < zLength + 1; z++)
            for (int x = 0; x < xLength; ++x)
            {
                if ((z - 1 < 0 || gridData.cells[x + (z - 1) * xLength].IsFill)
                    && (z >= zLength || gridData.cells[x + z * xLength].IsFill)) {
                    continue;
                }
                Vector3 start = GetPosition() + new Vector3(x * size, yOffset, z * size);
                
                for (; x < xLength; ++x)
                {
                    if ((z - 1 >= 0 && !gridData.cells[x + (z - 1) * xLength].IsFill)
                        || (z < zLength && !gridData.cells[x + z * xLength].IsFill)) {
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

            Gizmos.color = Color.green;
            foreach (Vector3 point in drawPoints)
            {
                Gizmos.DrawSphere(point, 0.1f);
            }

            Gizmos.color = Color.red;
            OnDrawArrow();
        }

        private void OnDrawArrow()
        {
            int xLength = gridData.xLength;
            int zLength = gridData.zLength;
            for (int x = 0; x < xLength; ++x)
            for (int z = 0; z < zLength; z++)
            {
                CellData data = gridData.cells[x + z * xLength];
                if (data.distance == 0 || data.distance == int.MaxValue)
                    continue;

                Vector3 direction = new Vector3(data.direction.x, 0, data.direction.z).normalized;
                Vector3 from = GetCellPosition(x, z) - direction * 0.25f;
                Vector3 to = GetCellPosition(x, z) + direction * 0.25f;
                
                Gizmos.DrawLine(from, to);
                Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + 20, 0) * new Vector3(0, 0, 1);
                Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - 20, 0) * new Vector3(0, 0, 1);
                Gizmos.DrawLine(to, to + right * 0.25f);
                Gizmos.DrawLine(to, to + left * 0.25f);
            }
        }
        
#endif
        
    }
}
