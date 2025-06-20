using System;

[Serializable]
public class BuildingData
{
    public static int width = 5;
    public static int height = 5;
    public static int xOffset = 2;
    public static int zOffset = 2;

    public long Id;
    public bool[] points = new bool[width * height];
    private bool[] points2 = new bool[width * height];

    public BuildingData(){ }
    
    public BuildingData(long id, bool[] points)
    {
        Id = id;
        Rotation(points, this.points, 0);
    }

    public void Rotation(bool[] from, bool[] to, int r)
    {
        r = ((r % 4) + 4) % 4;
        for (int x1 = 0; x1 < width; x1++)
        {
            for (int z1 = 0; z1 < height; z1++)
            {
                int x2 = x1;
                int z2 = z1;
                switch (r)
                {
                    case 1:
                        x2 = z1;
                        z2 = height - 1 - x1;
                        break;
                    case 2:
                        x2 = width - 1 - x1;
                        z2 = height - 1 - z1;
                        break;
                    case 3:
                        x2 = width - 1 - z1;
                        z2 = x1;
                        break;
                }
                to[x2 + z2 * width] = from[x1 + z1 * width];
            }
        }
    }
    
    public void Rotation(int r)
    {
        Rotation(points, points2, r);
        (points, points2) = (points2, points);
    }
}