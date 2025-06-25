using System;

[Serializable]
public class GridData
{
    public int xLength = 16;
    public int zLength = 16;
    public float cellSize = 1;
    public int levelCountMax = 2;
    
    public CellData[] cells;
    public int currentBlockGuid = 0;
    public int currentTowerGuid = 1000;
    
    public void ResetCells()
    {
        currentBlockGuid = 0;
        currentTowerGuid = 0;
        if (cells == null || xLength * zLength != cells.Length)
        {
            cells = new CellData[xLength * zLength];
            for (int x = 0; x < xLength; x++) {
                for (int z = 0; z < zLength; z++) {
                    cells[x + z * xLength] = new CellData();
                }
            }
        }
        else
        {
            for (int x = 0; x < xLength; x++) {
                for (int z = 0; z < zLength; z++) {
                    CellData cellData = cells[x + z * xLength];
                    cellData.isObstacle = false;
                    cellData.contentIds.Clear();
                }
            }
        }
    }
    
    public CellData GetCell(int x, int z)
    {
        return cells[x + z * xLength];
    }
    
    public bool IsInside(int x, int z)
    {
        return x >= 0 && z >= 0 && x < xLength && z < zLength;
    }
    
    public long GetNextGuid(PlacementData placementData)
    {
        switch (placementData.type)
        {
            case PlacementType.Block:
                return ++currentBlockGuid;
            case PlacementType.Tower:
                return ++currentTowerGuid;
            default:
                return 0;
        }
    }
    
    public bool CanTake(PlacementData placementData)
    {
        for (int x1 = 0; x1 < PlacementData.width; x1++) {
            for (int z1 = 0; z1 < PlacementData.height; z1++) {
                if (placementData.points[x1 + z1 * PlacementData.width])
                {
                    int x2 = placementData.x + x1 - PlacementData.xOffset;
                    int z2 = placementData.z + z1 - PlacementData.zOffset;
                    if (!IsInside(x2, z2)) {
                        return false;
                    }
                    CellData data = cells[x2 + z2 * xLength];
                    if (data.contentIds.Count == 0) {
                        return false;
                    }
                    if (data.contentIds[^1] != placementData.Id) {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    
    public void Take(PlacementData placementData)
    {
        for (int x1 = 0; x1 < PlacementData.width; x1++) {
            for (int z1 = 0; z1 < PlacementData.height; z1++) {
                if (placementData.points[x1 + z1 * PlacementData.width])
                {
                    int x2 = placementData.x + x1 - PlacementData.xOffset;
                    int z2 = placementData.z + z1 - PlacementData.zOffset;
                    CellData cellData = cells[x2 + z2 * xLength];
                    cellData.contentIds.Remove(placementData.Id);
                }
            }
        }
    }

    public bool CanPut(int x, int z, PlacementData placementData)
    {
        int level = -1;
        for (int x1 = 0; x1 < PlacementData.width; x1++) {
            for (int z1 = 0; z1 < PlacementData.height; z1++) {
                if (placementData.points[x1 + z1 * PlacementData.width])
                {
                    int x2 = x + x1 - PlacementData.xOffset;
                    int z2 = z + z1 - PlacementData.zOffset;
                    if (!IsInside(x2, z2)) {
                        return false;
                    }
                    CellData data = cells[x2 + z2 * xLength];
                    if (data.isObstacle) {
                        return false;
                    }

                    int count = data.contentIds.Count;
                    if (data.contentIds.IndexOf(placementData.Id) != -1) {
                        count = count - 1;
                    }

                    if (!CanPutLevel(count)) {
                        return false;
                    }
                    if (level == -1) {
                        level = count;
                    } else if (count != level) {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    
    public void Put(int x, int z, PlacementData placementData)
    {
        for (int x1 = 0; x1 < PlacementData.width; x1++) {
            for (int z1 = 0; z1 < PlacementData.height; z1++) {
                if (placementData.points[x1 + z1 * PlacementData.width])
                {
                    int x2 = x + x1 - PlacementData.xOffset;
                    int z2 = z + z1 - PlacementData.zOffset;
                    CellData cellData = cells[x2 + z2 * xLength];
                    cellData.contentIds.Add(placementData.Id);
                }
            }
        }
        placementData.x = x;
        placementData.z = z;
    }
    
    public bool IsInsideShape(int xOffset, int zOffset, PlacementData placementData)
    {
        int x = xOffset + PlacementData.xOffset;
        int z = zOffset + PlacementData.zOffset;
        if (x < 0 || x >= PlacementData.width || z < 0 || z >= PlacementData.height) {
            return false;
        }
        return placementData.points[x + z * PlacementData.width];
    }

    public bool CanPutLevel(int level)
    {
        return levelCountMax == -1 || levelCountMax > level;
    }
    
    public int GetLevelCount(int x, int z, PlacementData placementData)
    {
        int level = 0;
        for (int x1 = 0; x1 < PlacementData.width; x1++) {
            for (int z1 = 0; z1 < PlacementData.height; z1++) {
                if (placementData.points[x1 + z1 * PlacementData.width])
                {
                    int x2 = x + x1 - PlacementData.xOffset;
                    int z2 = z + z1 - PlacementData.zOffset;
                    level = Math.Max(level, GetLevelCount(x2, z2, placementData.Id));
                }
            }
        }
        return level;
    }

    public int GetLevelCount(int x, int z, long ignoreId)
    {
        if (!IsInside(x, z)) return 0;
        
        CellData data = cells[x + z * xLength];
        if (data.contentIds.IndexOf(ignoreId) != -1) {
            return data.contentIds.Count - 1;
        }
        return data.contentIds.Count;
    }
    
    public void SetObstacle(int x, int z, bool isObstacle)
    {
        if (IsInside(x, z))
        {
            cells[x + z * xLength].isObstacle = isObstacle;
        }
    }
}