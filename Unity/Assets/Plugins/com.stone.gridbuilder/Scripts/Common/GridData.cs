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
                    cells[x + z * xLength] = new CellData(x, z);
                }
            }
        }
        else
        {
            for (int x = 0; x < xLength; x++) {
                for (int z = 0; z < zLength; z++) {
                    CellData cellData = cells[x + z * xLength];
                    cellData.isObstacle = false;
                    cellData.buildingId = 0;
                }
            }
        }
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
        if (!this.CanPut(x, z, buildingData))
            return false;
        this.Put(x, z, buildingData);
        return true;
    }
    
    public bool CanPut(int x, int z)
    {
        return !cells[x + z * xLength].isFill;
    }
    
    public bool CanPut(int x, int z, BuildingData buildingData)
    {
        for (int x1 = 0; x1 < BuildingData.width; x1++) {
            for (int z1 = 0; z1 < BuildingData.height; z1++) {
                if (buildingData.points[x1 + z1 * BuildingData.width])
                {
                    int x2 = x + x1 - BuildingData.xOffset;
                    int z2 = z + z1 - BuildingData.zOffset;
                    if (!this.CheckInside(x2, z2)) {
                        return false;
                    }
                    CellData data = cells[x2 + z2 * xLength];
                    if (data.isObstacle || (data.buildingId > 0 && data.buildingId != buildingData.Id)) {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    
    public void Take(int x, int z, BuildingData buildingData)
    {
        for (int x1 = 0; x1 < BuildingData.width; x1++) {
            for (int z1 = 0; z1 < BuildingData.height; z1++) {
                if (buildingData.points[x1 + z1 * BuildingData.width])
                {
                    int x2 = x + x1 - BuildingData.xOffset;
                    int z2 = z + z1 - BuildingData.zOffset;
                    this.cells[x2 + z2 * xLength].buildingId = 0;
                }
            }
        }
    }
    
    public void Put(int x, int z, BuildingData buildingData)
    {
        for (int x1 = 0; x1 < BuildingData.width; x1++) {
            for (int z1 = 0; z1 < BuildingData.height; z1++) {
                if (buildingData.points[x1 + z1 * BuildingData.width])
                {
                    int x2 = x + x1 - BuildingData.xOffset;
                    int z2 = z + z1 - BuildingData.zOffset;
                    this.cells[x2 + z2 * xLength].buildingId = buildingData.Id;
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