using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ST.Mono
{
    public static class PoissonDiskFloat
    {
        public static void GeneratePoints(List<Vector2> points, Vector2 regionSize, float radius, int rejectionLimit = 30)
        {
            points.Clear();
            
            // 计算网格大小（每个单元格的边长为radius/√2）
            float cellSize = radius / Mathf.Sqrt(2);
            int gridWidth = Mathf.CeilToInt(regionSize.x / cellSize);
            int gridHeight = Mathf.CeilToInt(regionSize.y / cellSize);

            // 初始化网格，用于快速查找邻居
            int[,] grid = new int[gridWidth, gridHeight];
            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    grid[x, y] = -1; // -1 表示空单元格
                }
            }

            // 激活列表：当前可以生成新点的点索引
            List<int> activeIndices = new List<int>();

            // 生成第一个随机点
            Vector2 firstPoint = new Vector2(Random.Range(0, regionSize.x), Random.Range(0, regionSize.y));
            points.Add(firstPoint);
            activeIndices.Add(0);

            // 将第一个点放入网格
            (int, int) gridIndex = PositionToGridIndex(firstPoint, cellSize);
            grid[gridIndex.Item1, gridIndex.Item2] = 0;

            // 主循环：当还有激活点时继续生成
            while (activeIndices.Count > 0)
            {
                // 从激活列表中随机选择一个点
                int activeIndex = activeIndices[Random.Range(0, activeIndices.Count)];
                Vector2 activePoint = points[activeIndex];
                bool foundNewPoint = false;

                // 尝试生成新点
                for (int i = 0; i < rejectionLimit; i++)
                {
                    // 在圆环区域内随机生成点
                    // 内半径：radius（保证最小距离）
                    // 外半径：2 * radius（避免过远）
                    float angle = Random.Range(0, 2 * Mathf.PI);
                    float distance = Random.Range(radius, 2 * radius);

                    Vector2 candidate = activePoint + new Vector2(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance);

                    // 检查候选点是否在区域内
                    if (!IsPointInRectangle(candidate, regionSize))
                        continue;

                    // 检查候选点是否与现有点冲突
                    if (IsPointValid(candidate, points, grid, cellSize, radius))
                    {
                        // 找到有效点
                        points.Add(candidate);
                        int newIndex = points.Count - 1;
                        activeIndices.Add(newIndex);

                        // 更新网格
                        (int, int) candidateGridIndex = PositionToGridIndex(candidate, cellSize);
                        grid[candidateGridIndex.Item1, candidateGridIndex.Item2] = newIndex;

                        foundNewPoint = true;
                        break;
                    }
                }

                // 如果无法找到新点，从激活列表中移除当前点
                if (!foundNewPoint)
                {
                    activeIndices.Remove(activeIndex);
                }
            }
        }

        /// <summary>
        /// 计算从世界坐标到网格坐标的转换
        /// </summary>
        private static (int, int) PositionToGridIndex(Vector2 position, float cellSize)
        {
            int gridX = Mathf.FloorToInt(position.x / cellSize);
            int gridY = Mathf.FloorToInt(position.y / cellSize);

            // 确保在网格范围内
            gridX = Math.Clamp(gridX, 0, int.MaxValue);
            gridY = Math.Clamp(gridY, 0, int.MaxValue);

            return (gridX, gridY);
        }

        /// <summary>
        /// 检查点是否在矩形区域内
        /// </summary>
        private static bool IsPointInRectangle(Vector2 point, Vector2 size)
        {
            return point.x >= 0 && point.x <= size.x && point.y >= 0 && point.y <= size.y;
        }

        /// <summary>
        /// 检查点是否有效（不与现有点冲突）
        /// </summary>
        private static bool IsPointValid(Vector2 candidate, List<Vector2> points, int[,] grid, float cellSize, float radius)
        {
            // 获取候选点的网格索引
            (int, int) candidateGridIndex = PositionToGridIndex(candidate, cellSize);
            int gridWidth = grid.GetLength(0);
            int gridHeight = grid.GetLength(1);

            // 搜索周围的网格单元格（5x5区域）
            int searchStartX = Math.Max(0, candidateGridIndex.Item1 - 2);
            int searchEndX = Math.Min(gridWidth - 1, candidateGridIndex.Item1 + 2);
            int searchStartY = Math.Max(0, candidateGridIndex.Item2 - 2);
            int searchEndY = Math.Min(gridHeight - 1, candidateGridIndex.Item2 + 2);

            float squaredRadius = radius * radius;
            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
                {
                    int pointIndex = grid[x, y];
                    if (pointIndex != -1)
                    {
                        Vector2 existingPoint = points[pointIndex];
                        float squared = (candidate - existingPoint).sqrMagnitude;

                        // 如果距离小于半径，点无效
                        if (squared < squaredRadius)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 计算生成点的密度
        /// </summary>
        public static float CalculateDensity(List<Vector2> points, Vector2 regionSize)
        {
            if (points.Count == 0) return 0f;
            
            float area = regionSize.x * regionSize.y;
            return points.Count / area;
        }

        /// <summary>
        /// 计算点之间的平均距离
        /// </summary>
        public static float CalculateAverageDistance(List<Vector2> points)
        {
            if (points.Count < 2) return 0;

            float totalDistance = 0f;
            int pairCount = 0;

            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    totalDistance += Vector2.Distance(points[i], points[j]);
                    pairCount++;
                }
            }

            return totalDistance / pairCount;
        }

        /// <summary>
        /// 获取最小距离的点对
        /// </summary>
        public static (Vector2, Vector2, float) GetClosestPair(List<Vector2> points)
        {
            if (points.Count < 2) return (Vector2.zero, Vector2.zero, 0f);

            Vector2 pointA = points[0];
            Vector2 pointB = points[1];
            float minDistance = (pointA - pointB).sqrMagnitude;

            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    float distance = (points[i] - points[j]).sqrMagnitude;
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        pointA = points[i];
                        pointB = points[j];
                    }
                }
            }

            return (pointA, pointB, Mathf.Sqrt(minDistance));
        }

    }
}