#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace ST.Mono
{
    public class PoissonDiskSampler : MonoBehaviour
    {
        [Header("区域设置")]
        [SerializeField] private Vector2 regionSize = new Vector2(100f, 100f);

        [Header("采样参数")]
        [SerializeField, Min(0.1f)] private float minDistance = 1f;
        [SerializeField, Range(10, 50)] private int rejectionLimit = 30;
        [SerializeField] private int seed = 0;
        
        [Header("预览设置")]
        [SerializeField] private bool drawGizmos = true;
        [SerializeField] private Color regionColor = new Color(0, 1, 0, 0.3f);
        [SerializeField] private Color pointColor = Color.red;
        [SerializeField] private float pointGizmoSize = 0.2f;
        [SerializeField] private bool drawDistanceCircles = false;
        [SerializeField] private Color circleColor = new Color(0, 0, 1, 0.2f);
        
        private readonly List<Vector2> generatedPoints = new List<Vector2>();
        public List<Vector2> Points => generatedPoints;
        public int PointCount => generatedPoints.Count;
        public Vector2 RegionSize => regionSize;
        public float MinDistance => minDistance;
        
        public void GeneratePoints(int randomSeed, float pointMinDistance, Vector2 samplerRegionSize)
        {
            seed = randomSeed;
            minDistance = pointMinDistance;
            regionSize = samplerRegionSize;
            GeneratePoints();
        }
        
        public void GeneratePoints()
        {
            Random.InitState(seed);
            PoissonDiskFloat.GeneratePoints(generatedPoints, regionSize, minDistance, rejectionLimit);
        }
        
        public void ClearPoints()
        {
            generatedPoints.Clear();
        }
        
        /// <summary>
        /// 获取点的世界坐标
        /// </summary>
        public List<Vector3> GetWorldPositions(float yPosition = 0f)
        {
            List<Vector3> worldPositions = new List<Vector3>();
            
            Vector3 halfSize = new Vector3(regionSize.x * 0.5f, 0, regionSize.y * 0.5f);
            foreach (Vector2 point in generatedPoints)
            {
                Vector3 position = new Vector3(point.x, yPosition, point.y) - halfSize;
                worldPositions.Add(transform.TransformPoint(position));
            }
            
            return worldPositions;
        }
        
        /// <summary>
        /// 在指定位置生成游戏对象
        /// </summary>
        public List<GameObject> SpawnObjectsAtPoints(GameObject prefab, Transform parent = null)
        {
            if (prefab == null)
            {
                Debug.LogError("预制体不能为空");
                return new List<GameObject>();
            }
            
            List<GameObject> spawnedObjects = new List<GameObject>();
            List<Vector3> positions = GetWorldPositions();
            
            foreach (Vector3 position in positions)
            {
                GameObject obj = Instantiate(prefab, position, Quaternion.identity, parent);
                spawnedObjects.Add(obj);
            }
            
            return spawnedObjects;
        }
        
        /// <summary>
        /// 获取统计信息
        /// </summary>
        public string GetStats()
        {
            float density = PoissonDiskFloat.CalculateDensity(generatedPoints, regionSize);
            float avgDistance = PoissonDiskFloat.CalculateAverageDistance(generatedPoints);
            (Vector2, Vector2, float) closestPair = PoissonDiskFloat.GetClosestPair(generatedPoints);
            
            return $"点数: {generatedPoints.Count}\n" +
                   $"区域大小: {regionSize}\n" +
                   $"最小距离: {minDistance}\n" +
                   $"密度: {density:F4}\n" +
                   $"平均距离: {avgDistance:F3}\n" +
                   $"最小间距: {closestPair.Item3:F3}";
        }
        
        #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (!drawGizmos) return;
            
            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = regionColor;
            Gizmos.DrawCube(Vector3.zero, new Vector3(regionSize.x, 0.1f, regionSize.y));

            // 绘制点
            Gizmos.color = pointColor;
            Vector3 halfSize = new Vector3(regionSize.x * 0.5f, 0, regionSize.y * 0.5f);
            foreach (Vector2 point in generatedPoints)
            {
                Vector3 position = new Vector3(point.x, 0, point.y) - halfSize;
                Gizmos.DrawSphere(position, pointGizmoSize);
                
                // 绘制最小距离圆
                if (drawDistanceCircles)
                {
                    Gizmos.color = circleColor;
                    Gizmos.DrawWireSphere(position, minDistance * 0.5f);
                    Gizmos.color = pointColor;
                }
            }
            Gizmos.matrix = Matrix4x4.identity;
        }
        
        void OnDrawGizmosSelected()
        {
            // 在选中时显示额外信息
            if (generatedPoints.Count > 0)
            {
                UnityEditor.Handles.Label(transform.position + Vector3.up * 2, 
                    $"泊松圆盘采样器\n" +
                    $"点数: {generatedPoints.Count}\n" +
                    $"最小距离: {minDistance:F2}\n" +
                    $"区域: {regionSize.x:F1} x {regionSize.y:F1}");
            }
        }
        #endif
    }
}
#endif