#if UNITY_EDITOR
using UnityEngine;
using Random = UnityEngine.Random;

namespace ST.Mono
{
    public enum NoiseType
    {
        MathfPerlin,
        TSMathPerlin,
    }
    
    public class PerlinNoiseSampler : MonoBehaviour
    {
        [Header("区域设置")]
        [SerializeField] private Vector2 regionSize = new Vector2(100f, 100f);

        [Header("噪声设置")]
        [SerializeField] private NoiseType noiseType = NoiseType.MathfPerlin;
        [SerializeField] private Vector2 coordinateOffset = Vector2.zero;
        [SerializeField] private Vector2 coordinateScale = new Vector2(10f, 10f);
        [SerializeField] private int octaves = 4;
        [SerializeField] private float persistence = 0.5f;
        [SerializeField] private float lacunarity = 2f;
        [SerializeField] private int seed = 0;
        
        [Header("贴图设置")]
        [SerializeField] private AnimationCurve noiseCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] private bool useColorGradient = false;
        [SerializeField] private Gradient colorGradient = new Gradient();
        
        [SerializeField, HideInInspector] private float[,] noiseValues;  // 注意该数据格子未能序列化
        [SerializeField, HideInInspector] private Texture2D noiseTexture;
        [SerializeField] private MeshRenderer perlinMeshRenderer;
        
        private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");
        public float[,] NoiseValues => noiseValues;
        public Texture2D NoiseTexture => noiseTexture;
        public Vector2 RegionSize => regionSize;

        private void Reset()
        {
            GradientColorKey[] colorKeys = new GradientColorKey[5];
            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];

            colorKeys[0].color = Color.blue;
            colorKeys[0].time = 0.0f;
            colorKeys[1].color = Color.green;
            colorKeys[1].time = 0.3f;
            colorKeys[2].color = Color.yellow;
            colorKeys[2].time = 0.6f;
            colorKeys[3].color = new Color(0.5f, 0.3f, 0.1f); // 棕色
            colorKeys[3].time = 0.8f;
            colorKeys[4].color = Color.white;
            colorKeys[4].time = 1.0f;

            alphaKeys[0].alpha = 1.0f;
            alphaKeys[0].time = 0.0f;
            alphaKeys[1].alpha = 1.0f;
            alphaKeys[1].time = 1.0f;

            colorGradient.SetKeys(colorKeys, alphaKeys);
        }
        
        public void GenerateNoise(int randomSeed, Vector2 samplerRegionSize)
        {
            seed = randomSeed;
            regionSize = samplerRegionSize;
            GenerateNoise();
        }

        public void GenerateNoise()
        {
            int textureWidth = Mathf.CeilToInt(regionSize.x);
            int textureHeight = Mathf.CeilToInt(regionSize.y);
            if (textureWidth <= 0 || textureHeight <= 0)
            {
                Debug.LogError("区域尺寸必须大于0");
                return;
            }
            
            if (perlinMeshRenderer == null)
            {
                perlinMeshRenderer = CreatePerlinNoiseRenderer();
                perlinMeshRenderer.sharedMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            }

            if (noiseValues == null || noiseValues.GetLength(0) != textureWidth || noiseValues.GetLength(1) != textureHeight)
            {
                noiseValues = new float[textureWidth, textureHeight];
            }
            if (noiseTexture == null || noiseTexture.width != textureWidth || noiseTexture.height != textureHeight)
            {
                if (noiseTexture != null)
                {
                    DestroyImmediate(noiseTexture);
                }
                noiseTexture = new Texture2D(textureWidth, textureHeight);
                noiseTexture.filterMode = FilterMode.Bilinear;
                noiseTexture.wrapMode = TextureWrapMode.Repeat;
                perlinMeshRenderer.sharedMaterial.SetTexture(BaseMap, noiseTexture);
            }

            Vector2 offset = coordinateOffset;
            if (seed != 0)
            {
                Random.InitState(seed);
                offset.x += Random.Range(-10000f, 10000f);
                offset.y += Random.Range(-10000f, 10000f);
            }
            
            switch (noiseType)
            {
                case NoiseType.MathfPerlin:
                {
                    PerlinNoiseFloat.GeneratePerlinNoiseMap(noiseValues, coordinateScale, octaves, persistence, lacunarity, offset);
                    for (int x = 0; x < textureWidth; x++)
                    for (int y = 0; y < textureHeight; y++)
                    {
                        float value = noiseValues[x, y];
                        value = Mathf.Clamp01(value);
                        value = noiseCurve.Evaluate(value);
                        noiseValues[x, y] = value;
                        noiseTexture.SetPixel(x, y, useColorGradient ? colorGradient.Evaluate(value) : Color.Lerp(Color.black, Color.white, value));
                    }
                    break;
                }
                case NoiseType.TSMathPerlin:
                {
                    PerlinNoiseFloat.GeneratePerlinNoiseMapFP(noiseValues, coordinateScale, octaves, persistence, lacunarity, offset);
                    for (int x = 0; x < textureWidth; x++)
                    for (int y = 0; y < textureHeight; y++)
                    {
                        float value = noiseValues[x, y];
                        value = Mathf.Clamp01(value);
                        value = noiseCurve.Evaluate(value);
                        noiseValues[x, y] = value;
                        noiseTexture.SetPixel(x, y, useColorGradient ? colorGradient.Evaluate(value) : Color.Lerp(Color.black, Color.white, value));
                    }
                    break;
                }
            }
            noiseTexture.Apply();
        }
        
        private MeshRenderer CreatePerlinNoiseRenderer()
        {
            Transform tsf = transform;
            GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            quad.name = "PerlinNoiseQuad";
            quad.transform.SetParent(tsf);
            quad.transform.localPosition = new Vector3(0, 0.01f, 0);
            quad.transform.localRotation = Quaternion.Euler(90, 0, 0);
            quad.transform.localScale = new Vector3(regionSize.x, regionSize.y, 1);
            return quad.GetComponent<MeshRenderer>();
        }
        
        public void ClearNoise()
        {
            if (perlinMeshRenderer != null)
            {
                DestroyImmediate(perlinMeshRenderer.gameObject);
                perlinMeshRenderer = null;
            }
            if (noiseTexture != null)
            {
                DestroyImmediate(noiseTexture);
                noiseTexture = null;
            }
        }
        
    }
}
#endif