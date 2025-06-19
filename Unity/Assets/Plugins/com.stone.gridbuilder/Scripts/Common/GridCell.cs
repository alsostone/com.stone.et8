using System;

[Serializable]
public class GridCell
{
    public bool isFill => this.isObstacle || this.buildingId > 0;
    
    public long buildingId;
    public bool isObstacle;
    public int x;
    public int z;

    public GridCell(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
}
