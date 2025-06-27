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
            
            Vector3 pos = gridMap.GetPosition();
            int dir = 1;
            for (int z = 0; z < gridData.zLength + 1; z++)
            {
                pos = gridMap.RaycastPosition(pos);
                positions.Add(pos);
                
                for(int x = 1; x < gridData.xLength + 1; x++)
                {
                    pos.x += dir * gridData.cellSize;
                    pos = gridMap.RaycastPosition(pos);
                    positions.Add(pos);
                }
                dir *= -1;
                pos.z += gridData.cellSize;
            }
            
            pos = gridMap.GetPosition() + new Vector3(0, 0, gridData.cellSize * gridData.zLength);
            dir = -1;
            for (int index = 0; index < gridData.xLength + 1; index++)
            {
                pos = gridMap.RaycastPosition(pos);
                positions.Add(pos);
                
                for (int j = 0; j < gridData.zLength; j++)
                {
                    pos.z += dir * gridData.cellSize;
                    pos = gridMap.RaycastPosition(pos);
                    positions.Add(pos);
                }
                dir *= -1;
                pos.x += gridData.cellSize;
            }

            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.positionCount = positions.Count;
            lineRenderer.SetPositions(positions.ToArray());
        }

    }
}