using System.Collections.Generic;
using TrueSync;

namespace ST.Mono
{
    public static class PerlinNoiseFixed
    {
        public static FP OctavePerlinNoise(FP x, FP y, int octaves, FP persistence, FP lacunarity)
        {
            FP amplitude = 1;
            FP frequency = 1;
            FP total = 0;
            FP maxValue = 0;
            
            for (int i = 0; i < octaves; i++)
            {
                total += TSMathExtensions.PerlinNoise(x * frequency, y * frequency) * amplitude;
                maxValue += amplitude;
                amplitude *= persistence;
                frequency *= lacunarity;
            }
            
            return total / maxValue;
        }
        
        public static void GeneratePerlinNoiseMap(FP[,] noiseValues, TSVector2 scale, int octaves, FP persistence, FP lacunarity, TSVector2 offset)
        {
            int width = noiseValues.GetLength(0);
            int height = noiseValues.GetLength(1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    FP xCoord = (x + offset.x) / scale.x;
                    FP yCoord = (y + offset.y) / scale.y;
                    noiseValues[x, y] = OctavePerlinNoise(xCoord, yCoord, octaves, persistence, lacunarity);
                }
            }
        }
        
        public static void ResetDensityWithNoise(List<TSVector2> points, FP[,] noise, FP threshold)
        {
            List<TSVector2> points2 = new List<TSVector2>();
            foreach (TSVector2 point in points)
            {
                int x = (int)FP.Floor(point.x);
                int y = (int)FP.Floor(point.y);
                if (noise[x, y] > threshold) {
                    points2.Add(point);
                }
            }
            points.Clear();
            points.AddRange(points2);
        }
    }
}