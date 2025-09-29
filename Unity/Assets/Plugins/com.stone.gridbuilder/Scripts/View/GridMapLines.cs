using System.Collections.Generic;
using UnityEngine;

namespace ST.GridBuilder
{
    [RequireComponent(typeof(LineRenderer))]
    public class GridMapLines : MonoBehaviour
    {
        public GridMap gridMap;
        
        private void Awake()
        {
            if (gridMap == null)
                gridMap = FindObjectOfType<GridMap>();
        }

        public void GenerateLines()
        {
            GridData gridData = gridMap.gridData;
            List<Vector3> positions = new();
            
            float size = gridData.cellSize;
            Vector3 pos = Vector3.zero;
            int dir = 1;
            for (int z = 0; z < gridData.zLength + 1; z++)
            {
                positions.Add(gridMap.RaycastPosition(pos));
                
                for(int x = 1; x < gridData.xLength + 1; x++)
                {
                    pos.x += dir * size;
                    positions.Add(gridMap.RaycastPosition(pos));
                }
                dir *= -1;
                pos.z += size;
            }
            
            pos = new Vector3(0, 0, gridData.zLength * size);
            dir = -1;
            for (int index = 0; index < gridData.xLength + 1; index++)
            {
                positions.Add(gridMap.RaycastPosition(pos));
                
                for (int j = 0; j < gridData.zLength; j++)
                {
                    pos.z += dir * size;
                    positions.Add(gridMap.RaycastPosition(pos));
                }
                dir *= -1;
                pos.x += size;
            }

            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());
        }

    }
}