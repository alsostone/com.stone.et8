using System;
using System.Collections.Generic;
using MemoryPack;
using TrueSync;

namespace ST.GridBuilder
{
    [Flags]
    public enum PlacedLayer
    {
        None = 0,
        Map = 1 << 0,
        Block = 1 << 1,
        Building = 1 << 2
    }

    [MemoryPackable]
    [Serializable]
    public partial class PlacementData
    {
        [MemoryPackIgnore] public static int width = 5;
        [MemoryPackIgnore] public static int height = 5;
        [MemoryPackIgnore] public static int xOffset = 2;
        [MemoryPackIgnore] public static int zOffset = 2;
        [MemoryPackIgnore] private static bool[] sTemp = new bool[width * height];

        [MemoryPackInclude] public PlacedLayer placementType = PlacedLayer.Block;
        [MemoryPackInclude] public PlacedLayer placedLayer = PlacedLayer.Map | PlacedLayer.Block;
        [MemoryPackInclude] public long id;
        [MemoryPackInclude] public int x;
        [MemoryPackInclude] public int z;
        [MemoryPackInclude] public int rotation;
        [MemoryPackInclude] public bool[] points;
        [MemoryPackInclude] public bool isNew;

        public PlacementData()
        {
            points = new bool[width * height];
            isNew = true;
        }
        
        [MemoryPackConstructor]
        public PlacementData(bool[] points)
        {
            this.points = points;
        }

        public void Rotation(bool[] from, bool[] to, int r)
        {
            rotation += r;
            rotation = (rotation % 4 + 4) % 4;

            r = (r % 4 + 4) % 4;
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
            Rotation(points, sTemp, r);
            (points, sTemp) = (sTemp, points);
        }

        public int GetFirstEdge()
        {
            int start = 0;
            for (; start < points.Length; start++)
            {
                if (points[start]) {
                    break;
                }
            }
            if (start == points.Length) {
                return -1;
            }
            return start;
        }
    }
}