using System;
using System.Collections.Generic;
using MemoryPack;
using TrueSync;

namespace ST.GridBuilder
{
    public struct FlowFieldNode
    {
        public int distance;
        public FieldV2 direction;
    }

    public partial class GridData
    {
        [MemoryPackIgnore] private Queue<CellData> flowFieldVisit = new Queue<CellData>();
        [MemoryPackIgnore] private List<IndexV2> flowFieldDestinations = new List<IndexV2>();

        public FieldV2 GetFieldVector(FlowFieldNode[] flowField, FieldV2 position)
        {
            IndexV2 indexCurrent = ConvertToIndex(position);
            if (indexCurrent.x < 0 || indexCurrent.x >= xLength || indexCurrent.z < 0 || indexCurrent.z >= zLength) {
                return new FieldV2(0, 0);
            }
            
            FieldV2 v1 = new FieldV2(0, 0);
            FieldV2 v2 = new FieldV2(0, 0);
            FP half = (FP)cellSize / 2;
            
            int xLeft = (int)((position.x - half) / cellSize);
            int xRight = (int)((position.x + half) / cellSize);
            if (xLeft >= 0 && xLeft < xLength)
            {
                if (xRight >= 0 && xRight < xLength)
                {
                    FlowFieldNode left = flowField[xLeft + indexCurrent.z * xLength];
                    FlowFieldNode right = flowField[xRight + indexCurrent.z * xLength];
                
                    if (left.distance != int.MaxValue && right.distance != int.MaxValue)
                        v1 = left.direction.Lerp(right.direction, (position.x - (xLeft * cellSize + half)) / cellSize);
                    else if (left.distance != int.MaxValue)
                        v1 = left.direction;
                    else if (right.distance != int.MaxValue)
                        v1 = right.direction;
                }
                else
                {
                    FlowFieldNode left = flowField[xLeft + indexCurrent.z * xLength];
                    if (left.distance != int.MaxValue)
                        v1 = left.direction;
                }
            }
            else if (xRight >= 0 && xRight < xLength)
            {
                FlowFieldNode right = flowField[xRight + indexCurrent.z * xLength];
                if (right.distance != int.MaxValue)
                    v1 = right.direction;
            }
            
            
            int zTop = (int)((position.z + half) / cellSize);
            int zBottom = (int)((position.z - half) / cellSize);
            if (zTop >= 0 && zTop < zLength)
            {
                if (zBottom >= 0 && zBottom < zLength)
                {
                    FlowFieldNode top = flowField[indexCurrent.x + zTop * xLength];
                    FlowFieldNode bottom = flowField[indexCurrent.x + zBottom * xLength];
                
                    if (top.distance != int.MaxValue && bottom.distance != int.MaxValue)
                        v2 = top.direction.Lerp(bottom.direction, (position.z - (zTop * cellSize + half)) / cellSize);
                    else if (top.distance != int.MaxValue)
                        v2 = top.direction;
                    else if (bottom.distance != int.MaxValue)
                        v2 = bottom.direction;
                }
                else
                {
                    FlowFieldNode top = flowField[indexCurrent.x + zTop * xLength];
                    if (top.distance != int.MaxValue)
                        v2 = top.direction;
                }
            }
            else if (zBottom >= 0 && zBottom < zLength)
            {
                FlowFieldNode bottom = flowField[indexCurrent.x + zBottom * xLength];
                if (bottom.distance != int.MaxValue)
                    v2 = bottom.direction;
            }
            
            return v1.Lerp(v2, (position.z - (indexCurrent.z * cellSize + half)) / cellSize);
        }

        public void ResetDijkstraData(FlowFieldNode[] flowField, FieldV2 destination)
        {
            IndexV2 v2 = ConvertToIndex(destination);
            flowFieldDestinations.Clear();
            visited.Clear();
            GetNotContentNeighbors(v2.x, v2.z, flowFieldDestinations);

            Array.Fill(flowField, new FlowFieldNode { distance = int.MaxValue, direction = new FieldV2(0, 0)});
            flowFieldVisit.Clear();
            visited.Clear();
            
            foreach (IndexV2 indexV2 in flowFieldDestinations)
            {
                int index = indexV2.x + indexV2.z * xLength;
                flowField[index].distance = 0;
                flowFieldVisit.Enqueue(cells[index]);
                visited.Add(cells[index]);
            }
        }

        public void ResetDijkstraData(FlowFieldNode[] flowField, List<FieldV2> destinations)
        {
            Array.Fill(flowField, new FlowFieldNode { distance = int.MaxValue, direction = new FieldV2(0, 0)});
            flowFieldVisit.Clear();
            visited.Clear();
            
            foreach (FieldV2 dest in destinations)
            {
                IndexV2 indexV2 = ConvertToIndex(dest);
                indexV2 = GetValidDest(indexV2);
                
                int index = indexV2.x + indexV2.z * xLength;
                flowField[index].distance = 0;
                flowFieldVisit.Enqueue(cells[index]);
                visited.Add(cells[index]);
            }
        }

        public void GenerateDijkstraData(FlowFieldNode[] flowField)
        {
            while (flowFieldVisit.Count > 0)
            {
                CellData current = flowFieldVisit.Dequeue();
                int distance = flowField[current.index.x + current.index.z * xLength].distance;
                
                foreach (var (dx, dz) in OrthogonalDirections) {
                    int nx = current.index.x + dx;
                    int nz = current.index.z + dz;
                    if (nx < 0 || nx >= xLength || nz < 0 || nz >= zLength) {
                        continue;
                    }

                    int index = nx + nz * xLength;
                    if (IsFill(index)) {
                        continue;
                    }

                    int distance2 = distance + 10000;
                    if (flowField[index].distance > distance2) {
                        flowField[index] = new FlowFieldNode
                        {
                            distance = distance2,
                            direction = new FieldV2(current.index.x - nx, current.index.z - nz)
                        };
                    }

                    CellData neighbour = cells[index];
                    if (!visited.Contains(neighbour))
                    {
                        flowFieldVisit.Enqueue(neighbour);
                        visited.Add(neighbour);
                    }
                }
                
                foreach ((int dx, int dz) in DiagonalDirections) {
                    int nx = current.index.x + dx;
                    int nz = current.index.z + dz;
                    if (nx < 0 || nx >= xLength || nz < 0 || nz >= zLength) {
                        continue;
                    }

                    int index = nx + nz * xLength;
                    if (IsFill(index)) {
                        continue;
                    }
                    
                    // 若果斜对角方向有障碍物，则不允许斜线通过
                    if (IsFill(current.index.x, nz) || IsFill(nx, current.index.z)) {
                        continue;
                    }

                    int distance2 = distance + 14142;
                    if (flowField[index].distance > distance2) {
                        flowField[index] = new FlowFieldNode
                        {
                            distance = distance2,
                            direction = new FieldV2(current.index.x - nx, current.index.z - nz)
                        };
                    }
                    
                    CellData neighbour = cells[nx + nz * xLength];
                    if (!visited.Contains(neighbour))
                    {
                        flowFieldVisit.Enqueue(neighbour);
                        visited.Add(neighbour);
                    }
                }
            }
        }
        
        private void GetNotContentNeighbors(int x, int z, List<IndexV2> results)
        {
            int currentIndex = x + z * xLength;
            visited.Add(cells[currentIndex]);
            
            indexContentsMapping.TryGetValue(currentIndex, out var currentContents);
            if (currentContents != null && currentContents.Count > 0) {
                foreach (var (dx, dz) in Directions)
                {
                    int nx = x + dx;
                    int nz = z + dz;
                    if (nx < 0 || nx >= xLength || nz < 0 || nz >= zLength)
                        continue;
                    
                    int neighborIndex = nx + nz * xLength;
                    if (visited.Contains(cells[neighborIndex]))
                        continue;
                    
                    indexContentsMapping.TryGetValue(neighborIndex, out var neighborContents);
                    if (neighborContents != null && neighborContents.Count > 0 && neighborContents[0] != currentContents[0])
                        continue;
                    GetNotContentNeighbors(nx, nz, results);
                }
            }
            else {
                results.Add(new IndexV2(x, z));
            }
        }
    }
}