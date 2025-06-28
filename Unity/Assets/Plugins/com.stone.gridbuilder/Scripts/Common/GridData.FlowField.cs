using System;
using System.Collections.Generic;
using MemoryPack;
using TrueSync;

namespace ST.GridBuilder
{
    public partial class GridData
    {
        [MemoryPackIgnore] private Queue<CellData> visit = new Queue<CellData>();
        [MemoryPackInclude] private IndexV2 destination = new IndexV2(-1, -1);

        public void SetDestination(FieldV2 position)
        {
            IndexV2 indexCurrent = ConvertToIndex(ref position);
            destination = GetValidDestination(indexCurrent);
            ResetFlowField();
        }
        
        public void ResetFlowField()
        {
            if (destination.x < 0 || destination.x >= xLength || destination.z < 0 || destination.z >= zLength) {
                return;
            }
            ClearDijkstraData();
            GenerateDijkstraData(destination);
            GenerateFlowField();
        }
        
        public FieldV2 GetFieldVector(FieldV2 position)
        {
            IndexV2 indexCurrent = ConvertToIndex(ref position);
            if (indexCurrent.x < 0 || indexCurrent.x >= xLength || indexCurrent.z < 0 || indexCurrent.z >= zLength) {
                return new FieldV2(0, 0);
            }
            FP half = cellSize / 2;
            
            FieldV2 v1 = new FieldV2(0, 0);
            int xLeft = (int)((position.x - half) / cellSize);
            int xRight = (int)((position.x + half) / cellSize);
            if (xLeft >= 0 && xRight < xLength)
            {
                CellData cellLeft = cells[xLeft + indexCurrent.z * xLength];
                CellData cellRight = cells[xRight + indexCurrent.z * xLength];
                
                if (cellLeft.distance != int.MaxValue && cellRight.distance != int.MaxValue)
                    v1 = cellLeft.direction.Lerp(cellRight.direction, (position.x - (cellLeft.index.x * cellSize + half)) / cellSize);
                else if (cellLeft.distance != int.MaxValue)
                    v1 = cellLeft.direction;
                else if (cellRight.distance != int.MaxValue)
                    v1 = cellRight.direction;
            }
            else if (xLeft < 0)
            {
                CellData cellRight = cells[xRight + indexCurrent.z * xLength];
                if (cellRight.distance != int.MaxValue)
                    v1 = cellRight.direction;
            }
            else if (xRight >= xLength)
            {
                CellData cellLeft = cells[xLeft + indexCurrent.z * xLength];
                if (cellLeft.distance != int.MaxValue)
                    v1 = cellLeft.direction;
            }
            
            FieldV2 v2 = new FieldV2(0, 0);
            int zTop = (int)((position.z + half) / cellSize);
            int zBottom = (int)((position.z - half) / cellSize);
            if (zTop >= 0 && zBottom < zLength)
            {
                CellData cellTop = cells[indexCurrent.x + zTop * xLength];
                CellData cellBottom = cells[indexCurrent.x + zBottom * xLength];
                
                if (cellTop.distance != int.MaxValue && cellBottom.distance != int.MaxValue)
                    v2 = cellTop.direction.Lerp(cellBottom.direction, (position.z - (cellTop.index.z * cellSize + half)) / cellSize);
                else if (cellTop.distance != int.MaxValue)
                    v2 = cellTop.direction;
                else if (cellBottom.distance != int.MaxValue)
                    v2 = cellBottom.direction;
            }
            else if (zTop < 0)
            {
                CellData cellBottom = cells[indexCurrent.x + zBottom * xLength];
                if (cellBottom.distance != int.MaxValue)
                    v2 = cellBottom.direction;
            }
            else if (zBottom >= zLength)
            {
                CellData cellTop = cells[indexCurrent.x + zTop * xLength];
                if (cellTop.distance != int.MaxValue)
                    v2 = cellTop.direction;
            }
            return v1.Lerp(v2, (position.z - (indexCurrent.z * cellSize + half)) / cellSize).Normalize();
        }

        private void ClearDijkstraData()
        {
            foreach (var cell in cells)
            {
                cell.distance = int.MaxValue;
            }
            visit.Clear();
        }
        
        private void GenerateDijkstraData(IndexV2 dest)
        {
            CellData cellData = GetCell(dest.x, dest.z);
            if (cellData == null || cellData.IsFill) {
                return;
            }

            cellData.distance = 0;
            visit.Enqueue(cellData);
            
            while (visit.Count > 0)
            {
                CellData current = visit.Dequeue();
                
                if (current.index.x > 0) {
                    CellData neighbour = cells[current.index.x - 1 + current.index.z * xLength];
                    if (!neighbour.IsFill && neighbour.distance == int.MaxValue) {
                        neighbour.distance = current.distance + 1;
                        visit.Enqueue(neighbour);
                    }
                }
                if (current.index.x < xLength - 1) {
                    CellData neighbour = cells[current.index.x + 1 + current.index.z * xLength];
                    if (!neighbour.IsFill && neighbour.distance == int.MaxValue) {
                        neighbour.distance = current.distance + 1;
                        visit.Enqueue(neighbour);
                    }
                }
                if (current.index.z > 0) {
                    CellData neighbour = cells[current.index.x + (current.index.z - 1) * xLength];
                    if (!neighbour.IsFill && neighbour.distance == int.MaxValue) {
                        neighbour.distance = current.distance + 1;
                        visit.Enqueue(neighbour);
                    }
                }
                if (current.index.z < zLength - 1) {
                    CellData neighbour = cells[current.index.x + (current.index.z + 1) * xLength];
                    if (!neighbour.IsFill && neighbour.distance == int.MaxValue) {
                        neighbour.distance = current.distance + 1;
                        visit.Enqueue(neighbour);
                    }
                }
            }
        }

        private void GenerateFlowField()
        {
            for (int x = 0; x < xLength; x++)
            for (int z = 0; z < zLength; z++)
            {
                CellData cell = cells[x + z * xLength];
                if (cell.distance == 0 || cell.distance == int.MaxValue)
                    continue;
                
                int distance = int.MaxValue;
                for (int dx = -1; dx <= 1; dx++)
                for (int dz = -1; dz <= 1; dz++)
                {
                    if (dx == 0 && dz == 0)
                        continue;
                    
                    int nx = x + dx;
                    int nz = z + dz;
                    if (nx < 0 || nz < 0 || nx >= xLength || nz >= zLength)
                        continue;

                    if (dx * dz != 0) {
                        CellData diagonal1 = cells[nx + z * xLength];
                        if (diagonal1.distance == int.MaxValue) 
                            continue;
                        CellData diagonal2 = cells[x + nz * xLength];
                        if (diagonal2.distance == int.MaxValue) 
                            continue;
                    }
                    
                    CellData neighbour = cells[nx + nz * xLength];
                    if (neighbour.distance >= distance)
                        continue;

                    distance = neighbour.distance;
                    cell.direction = new FieldV2(neighbour.index.x - cell.index.x, neighbour.index.z - cell.index.z);
                }
            }
        }
        
        private IndexV2 GetValidDestination(IndexV2 dest)
        {
            CellData cellData = GetCell(dest.x, dest.z);
            if (cellData == null || cellData.IsFill)
            {
                // Find the nearest valid cell
                int minDistance = int.MaxValue;
                IndexV2 nearest = new IndexV2(-1, -1);
                for (int x = 0; x < xLength; x++)
                {
                    for (int z = 0; z < zLength; z++)
                    {
                        CellData cell = GetCell(x, z);
                        if (cell != null && !cell.IsFill)
                        {
                            int distance = Math.Abs(x - dest.x) + Math.Abs(z - dest.z);
                            if (distance < minDistance)
                            {
                                minDistance = distance;
                                nearest = new IndexV2(x, z);
                            }
                        }
                    }
                }
                return nearest;
            }
            return dest;
        }
    }
}