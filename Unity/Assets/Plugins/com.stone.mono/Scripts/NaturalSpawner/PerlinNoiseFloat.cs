using System.Collections.Generic;
using UnityEngine;

namespace ST.Mono
{
    public static class PerlinNoiseFloat
    {
        public static float OctavePerlinNoise(float x, float y, int octaves, float persistence = 0.5f, float lacunarity = 2)
        {
            float amplitude = 1;
            float frequency = 1;
            float total = 0;
            float maxValue = 0;
            
            for (int i = 0; i < octaves; i++)
            {
                total += Mathf.PerlinNoise(x * frequency, y * frequency) * amplitude;
                maxValue += amplitude;
                amplitude *= persistence;
                frequency *= lacunarity;
            }
            
            return total / maxValue;
        }
        
        public static void GeneratePerlinNoiseMap(float[,] noiseValues, Vector2 scale, int octaves, float persistence, float lacunarity, Vector2 offset)
        {
            int width = noiseValues.GetLength(0);
            int height = noiseValues.GetLength(1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float xCoord = (x + offset.x) / scale.x;
                    float yCoord = (y + offset.y) / scale.y;
                    noiseValues[x, y] = PerlinNoiseFloat.OctavePerlinNoise(xCoord, yCoord, octaves, persistence, lacunarity);
                }
            }
        }
        
        public static void GeneratePerlinNoiseMapFP(float[,] noiseValues, Vector2 scale, int octaves, float persistence, float lacunarity, Vector2 offset)
        {
            int width = noiseValues.GetLength(0);
            int height = noiseValues.GetLength(1);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float xCoord = (x + offset.x) / scale.x;
                    float yCoord = (y + offset.y) / scale.y;
                    noiseValues[x, y] = PerlinNoiseFixed.OctavePerlinNoise(xCoord, yCoord, octaves, persistence, lacunarity);
                }
            }
        }
        
        public static void ResetDensityWithNoise(List<Vector2> points, float[,] noise, float threshold)
        {
            List<Vector2> points2 = new List<Vector2>();
            foreach (Vector2 point in points)
            {
                int x = Mathf.FloorToInt(point.x);
                int y = Mathf.FloorToInt(point.y);
                if (noise[x, y] > threshold) {
                    points2.Add(point);
                }
            }
            points.Clear();
            points.AddRange(points2);
        }
        
    }
}