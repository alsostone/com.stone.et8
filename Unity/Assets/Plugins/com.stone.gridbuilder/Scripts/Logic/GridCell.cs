using System;

[Serializable]
public class GridCell
{
    public bool isFill => this.isObstacle;
    
    public bool isObstacle;
    public int x;
    public int z;

    public GridCell(int x, int z)
    {
        this.x = x;
        this.z = z;
    }
}
