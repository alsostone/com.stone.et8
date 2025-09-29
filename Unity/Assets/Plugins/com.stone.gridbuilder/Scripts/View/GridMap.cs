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
        [NonSerialized, HideInInspector] public GridData gridDataDraw = null;
        
        public Quaternion GetGridRotation()
        {
            return transform.rotation;
        }

        public IndexV2 ConvertToIndex(Vector3 position)
        {
            position = transform.InverseTransformDirection(position - transform.position);
            return gridData.ConvertToIndex(position.ToFieldV2());
        }

        public void SetDestination(Vector3 position)
        {
            position = transform.InverseTransformDirection(position - transform.position);
            gridData.SetDestination(position.ToFieldV2());
        }

        public Vector3 GetFieldVector(Vector3 position)
        {
            position = transform.InverseTransformDirection(position - transform.position);
            FieldV2 v2 = gridData.GetFieldVector(position.ToFieldV2());
            return transform.TransformDirection(v2.ToVector3());
        }
        
        public Vector3 RaycastPosition(Vector3 pos)
        {
            Transform tsf = transform;
            var origin = tsf.position + tsf.TransformDirection(new Vector3(pos.x, raycastHeight - yHeight, pos.z));
            if (Physics.Raycast(origin, -tsf.up, out RaycastHit hit, raycastHeight, terrainMask)) {
                pos = hit.point + tsf.TransformDirection(new Vector3(0, yHeight, 0));
            } else {
                pos = tsf.position + tsf.TransformDirection(new Vector3(pos.x, pos.y + yHeight, pos.z));
            }
            return pos;
        }
        
        public bool Raycast(Vector3 pos, LayerMask mask)
        {
            Transform tsf = transform;
            var origin = tsf.position + tsf.TransformDirection(new Vector3(pos.x, raycastHeight, pos.z));
            return Physics.Raycast(origin, -tsf.up, out RaycastHit hit, raycastHeight, mask);
        }
        
        public Vector3 GetPutPosition(PlacementData placementData)
        {
            CellData cellData = gridData.GetCell(placementData.x, placementData.z);
            int level = cellData?.contentIds.IndexOf(placementData.id) ?? 0 ;

            float x = gridData.cellSize * (placementData.x + 0.5f);
            float y = gridData.cellSize * level;
            float z = gridData.cellSize * (placementData.z + 0.5f);
            var pos = transform.position + transform.TransformDirection(new Vector3(x, y, z));
            return pos;
        }
        
        public Vector3 GetLevelPosition(int x, int z, int level, float height = 0)
        {
            float x1 = gridData.cellSize * (x + 0.5f);
            float y1 = gridData.cellSize * level + height;
            float z1 = gridData.cellSize * (z + 0.5f);
            var pos = transform.position + transform.TransformDirection(new Vector3(x1, y1, z1));
            return pos;
        }

        public Vector3 GetPosition(int x, int z, float height = 0)
        {
            float x1 = x * gridData.cellSize;
            float z1 = z * gridData.cellSize;
            var pos = transform.position + transform.TransformDirection(new Vector3(x1, height, z1));
            return pos;
        }
        
        public Vector3 GetCenterPosition(int x, int z, float height = 0)
        {
            float x1 = (x + 0.5f) * gridData.cellSize;
            float z1 = (z + 0.5f) * gridData.cellSize;
            var pos = transform.position + transform.TransformDirection(new Vector3(x1, height, z1));
            return pos;
        }
        
    #if UNITY_EDITOR
        private readonly List<Vector3> drawPoints = new List<Vector3>();
        void OnDrawGizmos()
        {
            if (gridDataDraw == null)
                gridDataDraw = gridData;
            if (gridDataDraw.cells == null || gridDataDraw.cells.Length != gridDataDraw.xLength * gridDataDraw.zLength) {
                return;
            }
            drawPoints.Clear();

            int xLength = gridDataDraw.xLength;
            int zLength = gridDataDraw.zLength;

            Gizmos.color = Color.yellow;
            for (int x = 0; x < xLength + 1; x++)
            for (int z = 0; z < zLength; ++z)
            {
                if ((x - 1 < 0 || gridDataDraw.cells[x - 1 + z * xLength].IsFill)
                    && (x >= xLength || gridDataDraw.cells[x + z * xLength].IsFill)) {
                    continue;
                }
                Vector3 start = GetPosition(x, z);
                drawPoints.Add(start);
                
                for (; z < zLength; ++z)
                {
                    Vector3 end = GetPosition(x, z);
                    drawPoints.Add(end);
                    if ((x - 1 >= 0 && !gridDataDraw.cells[x - 1 + z * xLength].IsFill)
                        || (x < xLength && !gridDataDraw.cells[x + z * xLength].IsFill)) {
                        continue;
                    }
                    Gizmos.DrawLine(start, end);
                    break;
                }

                if (z == zLength)
                {
                    Vector3 end = GetPosition(x, zLength);
                    drawPoints.Add(end);
                    Gizmos.DrawLine(start, end);
                }
            }

            for (int z = 0; z < zLength + 1; z++)
            for (int x = 0; x < xLength; ++x)
            {
                if ((z - 1 < 0 || gridDataDraw.cells[x + (z - 1) * xLength].IsFill)
                    && (z >= zLength || gridDataDraw.cells[x + z * xLength].IsFill)) {
                    continue;
                }
                Vector3 start = GetPosition(x, z);
                
                for (; x < xLength; ++x)
                {
                    if ((z - 1 >= 0 && !gridDataDraw.cells[x + (z - 1) * xLength].IsFill)
                        || (z < zLength && !gridDataDraw.cells[x + z * xLength].IsFill)) {
                        continue;
                    }
                    Vector3 end = GetPosition(x, z);
                    Gizmos.DrawLine(start, end);
                    break;
                }

                if (x == xLength)
                {
                    Vector3 end = GetPosition(xLength, z);
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
            int xLength = gridDataDraw.xLength;
            int zLength = gridDataDraw.zLength;
            for (int x = 0; x < xLength; ++x)
            for (int z = 0; z < zLength; z++)
            {
                CellData data = gridDataDraw.cells[x + z * xLength];
                if (data.distance == 0 || data.distance == int.MaxValue)
                    continue;

                Vector3 direction = new Vector3(data.direction.x.AsFloat(), 0, data.direction.z.AsFloat()).normalized;
                direction = transform.TransformDirection(direction);
                Vector3 from = GetCenterPosition(x, z) - direction * 0.25f;
                Vector3 to = GetCenterPosition(x, z) + direction * 0.25f;
                
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
