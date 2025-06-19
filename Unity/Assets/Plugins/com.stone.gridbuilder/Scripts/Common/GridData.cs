using System;

[Serializable]
public class GridData
{
    public int xLength = 16;
    public int zLength = 16;
    public float cellSize = 1;
    public CellData[] cells;

    public void ResetCells()
    {
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
    
    public bool TryPlace(BuildingData buildingData)
    {
        if (!CanPlace(buildingData))
            return false;
        this.Place(buildingData);
        return true;
    }
    
    public bool CanPlace(int x, int z)
    {
        return !cells[x + z * xLength].isFill;
    }
    
    public bool CanPlace(BuildingData buildingData)
    {
        int len = (int)Math.Sqrt(buildingData.points.Length);
        if (len * len != buildingData.points.Length) {
            throw new ArgumentException("Invalid building points length");
        }
        for (int x = 0; x < len; x++) {
            for (int z = 0; z < len; z++) {
                if (buildingData.points[x + z * len])
                {
                    int cellX = buildingData.x + x;
                    int cellZ = buildingData.z + z;
                    if (!this.CheckInside(cellX, cellZ)) {
                        return false;
                    }
                    if (!CanPlace(cellX, cellZ)) {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    public void Place(BuildingData buildingData)
    {
        int len = (int)Math.Sqrt(buildingData.points.Length);
        if (len * len != buildingData.points.Length) {
            throw new ArgumentException("Invalid building points length");
        }
        for (int x = 0; x < len; x++) {
            for (int z = 0; z < len; z++) {
                if (buildingData.points[x + z * len])
                {
                    int cellX = buildingData.x + x;
                    int cellZ = buildingData.z + z;
                    this.cells[cellX + cellZ * xLength].buildingId = buildingData.Id;
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