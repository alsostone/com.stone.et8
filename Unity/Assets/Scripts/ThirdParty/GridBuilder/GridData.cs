using System;
using System.Collections.Generic;
using MemoryPack;

namespace ST.GridBuilder
{
    [MemoryPackable]
    [Serializable]
    public partial struct IndexV2
    {
        [MemoryPackInclude] public int x;
        [MemoryPackInclude] public int z;

        public IndexV2(int x,int z)
        {
            this.x = x;
            this.z = z;
        }
        
        public bool Equals(IndexV2 other)
        {
            return x == other.x && z == other.z;
        }
        public override bool Equals(object obj)
        {
            return obj is IndexV2 other && Equals(other);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(x, z);
        }
        public static bool operator ==(IndexV2 left, IndexV2 right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(IndexV2 left, IndexV2 right)
        {
            return !left.Equals(right);
        }
    }

    [MemoryPackable]
    [Serializable]
    public partial class GridData
    {
        [MemoryPackInclude] public int xLength = 16;
        [MemoryPackInclude] public int zLength = 16;
        [MemoryPackInclude] public int cellSize = 1;
        [MemoryPackInclude] public int blockLevelMax = 2;

        [MemoryPackInclude] public CellData[] cells;
        [MemoryPackInclude] private int currentGuid = 0;
        
        // 注意：Unity的序列化系统不支持Dictionary 所以不会持久化到场景或预制体中
        [MemoryPackInclude] public Dictionary<int, List<(long, PlacedLayer)>> indexContentsMapping = new (32);

        [MemoryPackIgnore] private HashSet<CellData> visited = new HashSet<CellData>(64);
        private static readonly (int x, int z)[] OrthogonalDirections = 
        {
            (-1, 0), (1, 0), (0, -1), (0, 1)
        };
        private static readonly (int x, int z)[] DiagonalDirections = 
        {
            (-1, -1), (1, -1), (-1, 1), (1, 1)
        };
        private static readonly (int x, int z)[] Directions = 
        {
            (-1, 0), (1, 0), (0, -1), (0, 1), (-1, -1), (1, -1), (-1, 1), (1, 1)
        };
        
        public void ResetCells()
        {
            currentGuid = 0;
            indexContentsMapping.Clear();
            
            if (cells == null || xLength * zLength != cells.Length)
            {
                cells = new CellData[xLength * zLength];
                for (int x = 0; x < xLength; x++)
                for (int z = 0; z < zLength; z++)
                {
                    CellData data = new CellData { index = new IndexV2(x, z) };
                    cells[x + z * xLength] = data;
                }
            }
            else
            {
                for (int x = 0; x < xLength; x++)
                for (int z = 0; z < zLength; z++)
                {
                    CellData cellData = cells[x + z * xLength];
                    cellData.isObstacle = false;
                }
            }
        }
        
        public IndexV2 ConvertToIndex(FieldV2 position)
        {
            return new IndexV2((int)(position.x / cellSize), (int)(position.z / cellSize));
        }
        
        public CellData GetCell(int x, int z)
        {
            if (x >= 0 && z >= 0 && x < xLength && z < zLength)
                return cells[x + z * xLength];
            return null;
        }
        
        public bool IsInside(int x, int z)
        {
            return x >= 0 && z >= 0 && x < xLength && z < zLength;
        }
        
        public List<(long, PlacedLayer)> GetContents(int x, int z)
        {
            if (x < 0 || z < 0 || x >= xLength || z >= zLength)
                return null;
            
            int index = x + z * xLength;
            if (indexContentsMapping.TryGetValue(index, out var contents))
                return contents;
            
            return null;
        }

        public bool IsFill(int x, int z)
        {
            if (x < 0 || z < 0 || x >= xLength || z >= zLength)
                return true;
            
            int index = x + z * xLength;
            if (cells[index].isObstacle)
                return true;
            
            if (indexContentsMapping.TryGetValue(index, out var contents))
            {
                return contents.Count > 0;
            }
            return false;
        }
        
        public bool IsFill(int index)
        {
            if (index < 0 || index >= cells.Length)
                return true;

            if (cells[index].isObstacle)
                return true;

            if (indexContentsMapping.TryGetValue(index, out var contents))
            {
                return contents.Count > 0;
            }
            
            return false;
        }

        public long GetNextGuid()
        {
            return ++currentGuid;
        }

        public int CalcPutLevel(int x, int z, PlacementData placementData)
        {
            CellData data = GetCell(x, z);
            if (data == null || data.isObstacle)
            {
                return -1;
            }

            var contents = GetContents(x, z);
            if (contents != null && contents.Count > 0)
            {
                if (contents[^1].Item1 == placementData.id)
                    return contents.Count - 1;
                if ((placementData.placedLayer & contents[^1].Item2) == 0)
                    return -1;
                return contents.Count;
            }

            if ((placementData.placedLayer & PlacedLayer.Map) == 0)
                return -1;
            
            return 0;
        }

        public bool CanTake(PlacementData placementData)
        {
            for (int x1 = 0; x1 < PlacementData.width; x1++)
            for (int z1 = 0; z1 < PlacementData.height; z1++)
            {
                if (placementData.points[x1 + z1 * PlacementData.width])
                {
                    int x2 = placementData.x + x1 - PlacementData.xOffset;
                    int z2 = placementData.z + z1 - PlacementData.zOffset;
                    if (!IsInside(x2, z2))
                    {
                        return false;
                    }

                    var contents = GetContents(x2, z2);
                    if (contents == null || contents.Count == 0)
                    {
                        return false;
                    }

                    if (contents[^1].Item1 != placementData.id)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void Take(PlacementData placementData)
        {
            for (int x1 = 0; x1 < PlacementData.width; x1++)
            for (int z1 = 0; z1 < PlacementData.height; z1++)
            {
                if (placementData.points[x1 + z1 * PlacementData.width])
                {
                    int x2 = placementData.x + x1 - PlacementData.xOffset;
                    int z2 = placementData.z + z1 - PlacementData.zOffset;
                    
                    int index = x2 + z2 * xLength;
                    if (indexContentsMapping.TryGetValue(index, out var contents))
                    {
                        contents.RemoveAt(contents.Count - 1);
                        if (contents.Count == 0)
                        {
                            indexContentsMapping.Remove(index);
                        }
                    }
                }
            }
        }
        
        public bool CanPut(int x, int z, PlacementData placementData)
        {
            int putLevel = -1;
            for (int x1 = 0; x1 < PlacementData.width; x1++)
            for (int z1 = 0; z1 < PlacementData.height; z1++)
            {
                if (placementData.points[x1 + z1 * PlacementData.width])
                {
                    int x2 = x + x1 - PlacementData.xOffset;
                    int z2 = z + z1 - PlacementData.zOffset;
                    
                    int level = CalcPutLevel(x2, z2, placementData);
                    if (level == -1)
                    {
                        return false;
                    }
                    if (!CanPutLevel(level, placementData))
                    {
                        return false;
                    }

                    if (putLevel == -1)
                    {
                        putLevel = level;
                    }
                    else if (level != putLevel)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void Put(int x, int z, PlacementData placementData)
        {
            for (int x1 = 0; x1 < PlacementData.width; x1++)
            for (int z1 = 0; z1 < PlacementData.height; z1++)
            {
                if (placementData.points[x1 + z1 * PlacementData.width])
                {
                    int x2 = x + x1 - PlacementData.xOffset;
                    int z2 = z + z1 - PlacementData.zOffset;
                    
                    int index = x2 + z2 * xLength;
                    if (!indexContentsMapping.TryGetValue(index, out var contents))
                    {
                        contents = new List<(long, PlacedLayer)>();
                        indexContentsMapping[index] = contents;
                    }
                    contents.Add((placementData.id, placementData.placementType));
                }
            }

            placementData.isNew = false;
            placementData.x = x;
            placementData.z = z;
        }

        public bool IsInsideShape(int xOffset, int zOffset, PlacementData placementData)
        {
            int x = xOffset + PlacementData.xOffset;
            int z = zOffset + PlacementData.zOffset;
            if (x < 0 || x >= PlacementData.width || z < 0 || z >= PlacementData.height)
            {
                return false;
            }

            return placementData.points[x + z * PlacementData.width];
        }

        public bool CanPutLevel(int level, PlacementData placementData)
        {
            if ((placementData.placementType & PlacedLayer.Building) == PlacedLayer.Building)
                return true;
            if (blockLevelMax == -1)
                return true;
            return blockLevelMax > level;
        }

        public int GetShapeLevelCount(int x, int z, PlacementData placementData)
        {
            int level = (placementData.placedLayer & PlacedLayer.Map) == PlacedLayer.Map ? 0 : 1;
            for (int x1 = 0; x1 < PlacementData.width; x1++)
            {
                for (int z1 = 0; z1 < PlacementData.height; z1++)
                {
                    if (placementData.points[x1 + z1 * PlacementData.width])
                    {
                        int x2 = x + x1 - PlacementData.xOffset;
                        int z2 = z + z1 - PlacementData.zOffset;
                        level = Math.Max(level, GetPointLevelCount(x2, z2, placementData));
                    }
                }
            }

            return level;
        }

        public int GetPointLevelCount(int x, int z, PlacementData placementData)
        {
            if (!IsInside(x, z)) return 0;

            int blockLevel = 0;
            int index = x + z * xLength;
            if (indexContentsMapping.TryGetValue(index, out var contents))
            {
                for (int i = 0; i < contents.Count; i++)
                {
                    (long contentId, PlacedLayer placedLayer) = contents[i];
                    if (contentId != placementData.id && (placedLayer & PlacedLayer.Block) == PlacedLayer.Block)
                    {
                        blockLevel++;
                    }
                }
            }

            return blockLevel;
        }

        public void SetObstacle(int x, int z, bool isObstacle)
        {
            if (IsInside(x, z))
            {
                cells[x + z * xLength].isObstacle = isObstacle;
            }
        }
        
        private IndexV2 GetValidDest(IndexV2 dest)
        {
            dest.x = Math.Clamp(dest.x, 0, xLength - 1);
            dest.z = Math.Clamp(dest.z, 0, zLength - 1);
            
            if (!IsFill(dest.x, dest.z))
                return dest;
            
            int round = 1;
            while (round < xLength || round < zLength)
            {
                for (int dx = -round; dx <= round; dx += round * 2)
                for (int dz = -round; dz <= round; dz += round * 2)
                {
                    int nx = dest.x + dx;
                    int nz = dest.z + dz;

                    if (!IsFill(nx, nz)) {
                        return new IndexV2(nx, nz);
                    }
                }
                round++;
            }
            
            return new IndexV2(-1, -1);
        }
        
        public void GetObstaclesFirstEdge(List<int> results)
        {
            results.Clear();
            visited.Clear();
            
            for (int x = 0; x < xLength; x++)
            for (int z = 0; z < zLength; z++)
            {
                int index = x + z * xLength;
                CellData current = cells[index];
                if (current.isObstacle && !visited.Contains(current))
                {
                    MarkObstacleAndNeighbors(x, z);
                    results.Add(index);
                }
            }
        }
        
        private void MarkObstacleAndNeighbors(int x, int z)
        {
            CellData cell = GetCell(x, z);
            if (cell == null || !cell.isObstacle || visited.Contains(cell))
                return;
            
            visited.Add(cell);
            foreach (var (dx, dz) in Directions)
            {
                int nx = x + dx;
                int nz = z + dz;
                MarkObstacleAndNeighbors(nx, nz);
            }
        }
    }
}