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

        private static readonly HashSet<(int x, int z, int dir)> Visited = new (width * height * 4);
        private static readonly IndexV2[] RightForwardOffsets = {new(1, -1), new(1, 1), new(-1, 1), new(-1, -1) };
        private static readonly IndexV2[] ForwardOffsets = {new(1, 0), new(0, 1), new(-1, 0), new(0, -1) };
        private static readonly Func<int, int, TSVector2>[] CornerPoints = {
            (x, z) => new TSVector2(x + 1 - xOffset, z - zOffset),
            (x, z) => new TSVector2(x + 1 - xOffset , z + 1 - zOffset),
            (x, z) => new TSVector2(x - xOffset, z + 1 - zOffset),
            (x, z) => new TSVector2(x - xOffset, z - zOffset)
        };
        public void GenLocalBoundary(List<TSVector2> results)
        {
            results.Clear();
            
            int start = 0;
            for (; start < points.Length; start++)
            {
                if (points[start]) {
                    break;
                }
            }
            if (start == points.Length) {
                return;
            }
            
            int xStart = start % width;
            int zStart = start / width;
            int xCurrent = xStart;
            int zCurrent = zStart;
            
            int dir = 0;
            Visited.Clear();
            
            do
            {
                (int x, int z, int dir) vis = (xCurrent, zCurrent, dir);
                if (Visited.Contains(vis)) {
                    break;
                }
                Visited.Add(vis);
                
                int xRight = xCurrent + RightForwardOffsets[dir].x;
                int zRight = zCurrent + RightForwardOffsets[dir].z;
                if (xRight >= 0 && xRight < width && zRight >= 0 && zRight < height)
                {
                    if (points[xRight + zRight * width])
                    {
                        results.Add(CornerPoints[dir](xCurrent, zCurrent));
                        xCurrent = xRight;
                        zCurrent = zRight;
                        dir = (dir + 3) % 4;
                        continue;
                    }
                }

                int xForward = xCurrent + ForwardOffsets[dir].x;
                int zForward = zCurrent + ForwardOffsets[dir].z;
                if (xForward >= 0 && xForward < width && zForward >= 0 && zForward < height)
                {
                    if (points[xForward + zForward * width])
                    {
                        xCurrent = xForward;
                        zCurrent = zForward;
                        continue;
                    }
                }
                
                results.Add(CornerPoints[dir](xCurrent, zCurrent));
                dir = (dir + 1) % 4;
                
            } while (xStart != xCurrent || zStart != zCurrent || dir != 0);
        }
        
    }
}