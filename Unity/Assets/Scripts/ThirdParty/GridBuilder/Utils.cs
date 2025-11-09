using System;
using System.Collections.Generic;

namespace ST.GridBuilder
{
    public static class Utils
    {
        private static readonly HashSet<(int x, int z, int dir)> Visited = new (32);
        private static readonly IndexV2[] RightForwardOffsets = {new(1, -1), new(1, 1), new(-1, 1), new(-1, -1) };
        private static readonly IndexV2[] ForwardOffsets = {new(1, 0), new(0, 1), new(-1, 0), new(0, -1) };
        private static readonly Func<int, int, IndexV2>[] CornerPoints = {
            (x, z) => new IndexV2(x + 1, z),
            (x, z) => new IndexV2(x + 1 , z + 1),
            (x, z) => new IndexV2(x, z + 1),
            (x, z) => new IndexV2(x, z)
        };

        public static void GenLocalContours(bool[] points, int width, int height, int start, List<IndexV2> results)
        {
            results.Clear();
            
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
        
        public static void GenLocalContours(CellData[] cells, int width, int height, int start, List<IndexV2> results)
        {
            results.Clear();
            
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
                    if (cells[xRight + zRight * width].isObstacle)
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
                    if (cells[xForward + zForward * width].isObstacle)
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
