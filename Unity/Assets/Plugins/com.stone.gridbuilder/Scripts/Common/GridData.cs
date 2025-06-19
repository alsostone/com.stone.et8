using System;

[Serializable]
public class GridData
{
    public int xLength = 16;
    public int zLength = 16;
    public float cellSize = 1;
    public GridCell[] cells;

    public void ResetCells()
    {
        if (cells == null || xLength * zLength != cells.Length)
        {
            cells = new GridCell[xLength * zLength];
            for (int x = 0; x < xLength; x++) {
                for (int z = 0; z < zLength; z++) {
                    cells[x + z * xLength] = new GridCell(x, z);
                }
            }
        }
        else
        {
            for (int x = 0; x < xLength; x++) {
                for (int z = 0; z < zLength; z++) {
                    GridCell cell = cells[x + z * xLength];
                    cell.isObstacle = false;
                    cell.buildingId = 0;
                }
            }
        }
    }
    
    public bool CheckInside(int x, int z)
    {
        return x >= 0 && z >= 0 && x < xLength && z < zLength;
    }
    
    public bool TryPlace(Building building)
    {
        if (!CanPlace(building))
            return false;
        this.Place(building);
        return true;
    }
    
    public bool CanPlace(int x, int z)
    {
        return !cells[x + z * xLength].isFill;
    }
    
    public bool CanPlace(Building building)
    {
        int len = (int)Math.Sqrt(building.points.Length);
        if (len * len != building.points.Length) {
            throw new ArgumentException("Invalid building points length");
        }
        for (int x = 0; x < len; x++) {
            for (int z = 0; z < len; z++) {
                if (building.points[x + z * len])
                {
                    int cellX = building.x + x;
                    int cellZ = building.z + z;
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

    public void Place(Building building)
    {
        int len = (int)Math.Sqrt(building.points.Length);
        if (len * len != building.points.Length) {
            throw new ArgumentException("Invalid building points length");
        }
        for (int x = 0; x < len; x++) {
            for (int z = 0; z < len; z++) {
                if (building.points[x + z * len])
                {
                    int cellX = building.x + x;
                    int cellZ = building.z + z;
                    this.cells[cellX + cellZ * xLength].buildingId = building.Id;
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