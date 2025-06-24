using System;

[Serializable]
public class GridData
{
    public int xLength = 16;
    public int zLength = 16;
    public float cellSize = 1;
    public int levelMax = 3;
    
    public CellData[] cells;
    public int currentGuid = 0;
    
    public void ResetCells()
    {
        currentGuid = 0;
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
    
    public long GetNextGuid()
    {
        return ++currentGuid;
    }
    
    public bool CanTake(BuildingData buildingData)
    {
        for (int x1 = 0; x1 < BuildingData.width; x1++) {
            for (int z1 = 0; z1 < BuildingData.height; z1++) {
                if (buildingData.points[x1 + z1 * BuildingData.width])
                {
                    int x2 = buildingData.x + x1 - BuildingData.xOffset;
                    int z2 = buildingData.z + z1 - BuildingData.zOffset;
                    if (!IsInside(x2, z2)) {
                        return false;
                    }
                    CellData data = cells[x2 + z2 * xLength];
                    if (data.contentIds.Count == 0) {
                        return false;
                    }
                    if (data.contentIds[^1] != buildingData.Id) {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    
    public void Take(BuildingData buildingData)
    {
        for (int x1 = 0; x1 < BuildingData.width; x1++) {
            for (int z1 = 0; z1 < BuildingData.height; z1++) {
                if (buildingData.points[x1 + z1 * BuildingData.width])
                {
                    int x2 = buildingData.x + x1 - BuildingData.xOffset;
                    int z2 = buildingData.z + z1 - BuildingData.zOffset;
                    CellData cellData = cells[x2 + z2 * xLength];
                    cellData.contentIds.Remove(buildingData.Id);
                }
            }
        }
    }

    public bool CanPut(int x, int z, BuildingData buildingData)
    {
        int level = -1;
        for (int x1 = 0; x1 < BuildingData.width; x1++) {
            for (int z1 = 0; z1 < BuildingData.height; z1++) {
                if (buildingData.points[x1 + z1 * BuildingData.width])
                {
                    int x2 = x + x1 - BuildingData.xOffset;
                    int z2 = z + z1 - BuildingData.zOffset;
                    if (!IsInside(x2, z2)) {
                        return false;
                    }
                    CellData data = cells[x2 + z2 * xLength];
                    if (data.isObstacle) {
                        return false;
                    }

                    int count = data.contentIds.Count;
                    if (data.contentIds.IndexOf(buildingData.Id) != -1) {
                        count = count - 1;
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
    
    public void Put(int x, int z, BuildingData buildingData)
    {
        for (int x1 = 0; x1 < BuildingData.width; x1++) {
            for (int z1 = 0; z1 < BuildingData.height; z1++) {
                if (buildingData.points[x1 + z1 * BuildingData.width])
                {
                    int x2 = x + x1 - BuildingData.xOffset;
                    int z2 = z + z1 - BuildingData.zOffset;
                    CellData cellData = cells[x2 + z2 * xLength];
                    cellData.contentIds.Add(buildingData.Id);
                }
            }
        }
        buildingData.x = x;
        buildingData.z = z;
    }
    
    public bool IsInsideShape(int xOffset, int zOffset, BuildingData buildingData)
    {
        int x = xOffset + BuildingData.xOffset;
        int z = zOffset + BuildingData.zOffset;
        if (x < 0 || x >= BuildingData.width || z < 0 || z >= BuildingData.height) {
            return false;
        }
        return buildingData.points[x + z * BuildingData.width];
    }
    
    public int GetLevel(int x, int z, BuildingData buildingData)
    {
        int level = 0;
        for (int x1 = 0; x1 < BuildingData.width; x1++) {
            for (int z1 = 0; z1 < BuildingData.height; z1++) {
                if (buildingData.points[x1 + z1 * BuildingData.width])
                {
                    int x2 = x + x1 - BuildingData.xOffset;
                    int z2 = z + z1 - BuildingData.zOffset;
                    level = Math.Max(level, GetLevel(x2, z2, buildingData.Id));
                }
            }
        }
        return level;
    }

    public int GetLevel(int x, int z, long ignoreId)
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