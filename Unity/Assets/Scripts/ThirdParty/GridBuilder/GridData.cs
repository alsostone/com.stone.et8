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
                    cellData.contentIds.Clear();
                    cellData.contentTypes.Clear();
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

        public long GetNextGuid()
        {
            return ++currentGuid;
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

                    CellData data = cells[x2 + z2 * xLength];
                    if (data.contentIds.Count == 0)
                    {
                        return false;
                    }

                    if (data.contentIds[^1] != placementData.id)
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
                    CellData cellData = cells[x2 + z2 * xLength];
                    cellData.contentIds.RemoveAt(cellData.contentIds.Count - 1);
                    cellData.contentTypes.RemoveAt(cellData.contentTypes.Count - 1);
                }
            }
        }

        public bool CanPut(int x, int z, PlacementData placementData)
        {
            int level = -1;
            for (int x1 = 0; x1 < PlacementData.width; x1++)
            for (int z1 = 0; z1 < PlacementData.height; z1++)
            {
                if (placementData.points[x1 + z1 * PlacementData.width])
                {
                    int x2 = x + x1 - PlacementData.xOffset;
                    int z2 = z + z1 - PlacementData.zOffset;
                    CellData data = GetCell(x2, z2);
                    if (data == null || !data.CanPut(placementData))
                    {
                        return false;
                    }

                    int count = data.contentIds.Count;
                    if (data.contentIds.IndexOf(placementData.id) != -1)
                    {
                        count -= 1;
                    }

                    if (!CanPutLevel(count, placementData))
                    {
                        return false;
                    }

                    if (level == -1)
                    {
                        level = count;
                    }
                    else if (count != level)
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
                    CellData cellData = cells[x2 + z2 * xLength];
                    cellData.contentIds.Add(placementData.id);
                    cellData.contentTypes.Add(placementData.placementType);
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
            CellData data = cells[x + z * xLength];
            for (int i = 0; i < data.contentTypes.Count; i++)
            {
                PlacedLayer placedLayer = data.contentTypes[i];
                if (data.contentIds[i] != placementData.id && (placedLayer & PlacedLayer.Block) == PlacedLayer.Block)
                {
                    blockLevel++;
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
            
            CellData cellData = GetCell(dest.x, dest.z);
            if (!cellData.IsFill)
                return dest;
            
            int round = 1;
            while (round < xLength || round < zLength)
            {
                for (int dx = -round; dx <= round; dx += round * 2)
                for (int dz = -round; dz <= round; dz += round * 2)
                {
                    int nx = dest.x + dx;
                    int nz = dest.z + dz;
                    if (!IsInside(nx, nz))
                        continue;
                    
                    CellData cell = GetCell(nx, nz);
                    if (!cell.IsFill) {
                        return new IndexV2(nx, nz);
                    }
                }
                round++;
            }
            
            return new IndexV2(-1, -1);
        }
        
    }
}