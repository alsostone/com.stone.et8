#if UNITY_EDITOR
using UnityEngine;

namespace ST.Mono
{
    [RequireComponent(typeof(PerlinNoiseSampler))]
    [RequireComponent(typeof(PoissonDiskSampler))]
    public class NaturalSpawnSampler : MonoBehaviour
    {
        [SerializeField] private Vector2 regionSize = new Vector2(100f, 100f);
        [SerializeField, Min(0.1f)] private float minDistance = 3f;
        [SerializeField] private int seed = 0;
        
        [SerializeField] private PerlinNoiseSampler perlinNoiseSampler;
        [SerializeField] private PoissonDiskSampler poissonDiskSampler;

        private void Reset()
        {
            perlinNoiseSampler = GetComponent<PerlinNoiseSampler>();
            poissonDiskSampler = GetComponent<PoissonDiskSampler>();
        }

        public void Generate()
        {
            perlinNoiseSampler.GenerateNoise(seed, regionSize);
            poissonDiskSampler.GeneratePoints(seed, minDistance, regionSize);
            PerlinNoiseFloat.ResetDensityWithNoise(poissonDiskSampler.Points, perlinNoiseSampler.NoiseValues, 0.5f);
        }
        
        public void GenerateRandom()
        {
            seed = Random.Range(int.MinValue, int.MaxValue);
            perlinNoiseSampler.GenerateNoise(seed, regionSize);
            poissonDiskSampler.GeneratePoints(seed, minDistance, regionSize);
            PerlinNoiseFloat.ResetDensityWithNoise(poissonDiskSampler.Points, perlinNoiseSampler.NoiseValues, 0.5f);
        }
        
        public void ClearAllData()
        {
            perlinNoiseSampler.ClearNoise();
            poissonDiskSampler.ClearPoints();
        }
        
    }
}
#endif