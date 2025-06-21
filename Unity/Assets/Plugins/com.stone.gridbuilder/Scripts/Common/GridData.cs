using System;

[Serializable]
public class GridData
{
    public int xLength = 16;
    public int zLength = 16;
    public float cellSize = 1;
    
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
    
    public bool CheckInside(int x, int z)
    {
        return x >= 0 && z >= 0 && x < xLength && z < zLength;
    }
    
    public long GetNextGuid()
    {
        return ++currentGuid;
    }
    
    public bool TryPut(int x, int z, BuildingData buildingData)
    {
        if (!CanPut(x, z, buildingData))
            return false;
        Put(x, z, buildingData);
        return true;
    }
    
    public bool CanPut(int x, int z)
    {
        return !cells[x + z * xLength].isFill;
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
                    if (!CheckInside(x2, z2)) {
                        return false;
                    }
                    CellData data = cells[x2 + z2 * xLength];
                    if (data.isObstacle) {
                        return false;
                    }

                    if (level == -1) {
                        level = data.contentIds.Count;
                    } else if (data.contentIds.Count != level) {
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
    
    public int GetPutLevel(int x, int z, BuildingData buildingData)
    {
        int level = 0;
        for (int x1 = 0; x1 < BuildingData.width; x1++) {
            for (int z1 = 0; z1 < BuildingData.height; z1++) {
                if (buildingData.points[x1 + z1 * BuildingData.width])
                {
                    int x2 = x + x1 - BuildingData.xOffset;
                    int z2 = z + z1 - BuildingData.zOffset;
                    if (CheckInside(x2, z2)) {
                        CellData data = cells[x2 + z2 * xLength];
                        if (data.contentIds.IndexOf(buildingData.Id) != -1) {
                            level = Math.Max(level, data.contentIds.Count - 1);
                        } else {
                            level = Math.Max(level, data.contentIds.Count);
                        }
                    }
                }
            }
        }
        return level;
    }
    
    public bool CanTake(BuildingData buildingData)
    {
        for (int x1 = 0; x1 < BuildingData.width; x1++) {
            for (int z1 = 0; z1 < BuildingData.height; z1++) {
                if (buildingData.points[x1 + z1 * BuildingData.width])
                {
                    int x2 = buildingData.x + x1 - BuildingData.xOffset;
                    int z2 = buildingData.z + z1 - BuildingData.zOffset;
                    if (!CheckInside(x2, z2)) {
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
    
    public void SetObstacle(int x, int z, bool isObstacle)
    {
        if (CheckInside(x, z))
        {
            cells[x + z * xLength].isObstacle = isObstacle;
        }
    }
}