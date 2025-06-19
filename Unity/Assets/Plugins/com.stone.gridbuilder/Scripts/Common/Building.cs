using System;

[Serializable]
public class Building
{
    public long Id;
    public int x;
    public int z;
    public bool[] points;
    public int rotation;
    
    public Building(){ }
    
    public Building(long id, int x, int z, bool[] points, int rotation = 0)
    {
        Id = id;
        this.x = x;
        this.z = z;
        this.rotation = rotation;
        
        int len = (int)Math.Sqrt(points.Length);
        if (len * len != points.Length) {
            throw new ArgumentException("Invalid building points length");
        }
        if (rotation < 0 || rotation > 3) {
            throw new ArgumentException("Invalid building rotation");
        }

        this.points = new bool[len * len];
        for (int x1 = 0; x1 < len; x1++)
        {
            for (int z1 = 0; z1 < len; z1++)
            {
                int x2 = x1;
                int z2 = z1;
                switch (this.rotation)
                {
                    case 1:
                        x2 = z1;
                        z2 = len - 1 - x1;
                        break;
                    case 2:
                        x2 = len - 1 - x1;
                        z2 = len - 1 - z1;
                        break;
                    case 3:
                        x2 = len - 1 - z1;
                        z2 = x1;
                        break;
                }
                this.points[x2 + z2 * len] = points[x1 + z1 * len];
            }
        }
    }
}