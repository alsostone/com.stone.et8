using System;
using System.Collections.Generic;
using MemoryPack;

namespace ST.GridBuilder
{
    public partial class GridData
    {
        [MemoryPackIgnore] private PriorityQueue<CellData, int> frontier = new ();

        public bool Pathfinding(FieldV2 start, FieldV2 to, List<IndexV2> results)
        {
            results.Clear();

            IndexV2 startIndex = GetValidDest(ConvertToIndex(start));
            CellData startCell = GetCell(startIndex.x, startIndex.z);
            if (startCell == null || startCell.IsFill) {
                return false;
            }
            
            IndexV2 toIndexV2 = GetValidDest(ConvertToIndex(to));
            CellData toCell = GetCell(toIndexV2.x, toIndexV2.z);
            if (toCell == null || toCell.IsFill) {
                return false;
            }
            return Pathfinding(startCell, toCell, results);
        }

        public bool Pathfinding(CellData start, CellData dest, List<IndexV2> results)
        {
            ClearAStarData();
            
            frontier.Enqueue(start, 0);
            visited.Add(start);

            start.cost = 0;
            start.prev = null;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();
                if (current == dest) {
                    break;
                }
                
                foreach (var (dx, dz) in Directions) {
                    int nx = current.index.x + dx;
                    int nz = current.index.z + dz;
                    if (nx < 0 || nx >= xLength || nz < 0 || nz >= zLength) {
                        continue;
                    }

                    CellData neighbour = cells[nx + nz * xLength];
                    if (neighbour.IsFill || visited.Contains(neighbour)) {
                        continue;
                    }

                    int cost = current.cost + ((dx == 0 || dz == 0) ? 10000 : 14142); // Orthogonal or diagonal cost
                    if (neighbour.cost > cost) {
                        neighbour.cost = cost;
                        neighbour.prev = current;
                        frontier.Enqueue(neighbour, cost + Heuristic(neighbour, dest) * 10000);
                        visited.Add(neighbour);
                    }
                }
            }

            return BacktrackToPath(dest, results);
        }

        private void ClearAStarData()
        {
            foreach (var cell in cells) {
                cell.cost = int.MaxValue;
                cell.prev = null;
            }
            frontier.Clear();
            visited.Clear();
        }
        
        private bool BacktrackToPath(CellData dest, List<IndexV2> results)
        {
            CellData current = dest;
            while (current != null)
            {
                results.Add(current.index);
                current = current.prev;
            }

            results.Reverse();
            return results.Count > 0;
        }
        
        private int Heuristic(CellData a, CellData b)
        {
            return Math.Abs(a.index.x - b.index.x) + Math.Abs(a.index.z - b.index.z);
        }
    }
}