using System.Collections.Generic;
using UnityEngine;

public class GridMapIndicator : MonoBehaviour
{
    [SerializeField] public GridMap gridMap;
    [SerializeField, Min(5)] public int indicatorSize = 11;
    [SerializeField] private GameObject indicatorPrefab;
    
    private readonly Dictionary<(int, int), CellIndicator> indicators = new ();
    private readonly List<CellIndicator> indicatorPool = new ();
    private Transform poolGameObject;
    
    private readonly HashSet<CellIndicator> keepIndicators = new ();
    private readonly Dictionary<(int, int), CellIndicator> removeIndicators = new ();
    
    private void Awake()
    {
        if (gridMap == null)
            gridMap = FindObjectOfType<GridMap>();
        
        GameObject go = new GameObject("IndicatorPool");
        poolGameObject = go.transform;
        go.SetActive(false);
    }

    public void ClearIndicator()
    {
        foreach (var kv in indicators)
        {
            kv.Value.DoRemove();
        }
        indicators.Clear();
    }
    
    public void Recycle(CellIndicator indicator)
    {
        indicator.transform.parent = poolGameObject;
        indicatorPool.Add(indicator);
    }

    public void GenerateIndicator(int x, int z, int targetLevel, PlacementData placementData)
    {
        GridData gridData = gridMap.gridData;
        int halfSize = indicatorSize / 2;
        
        keepIndicators.Clear();
        for(int x1 = x - halfSize; x1 < x + halfSize; x1++)
        {
            for (int z1 = z - halfSize; z1 < z + halfSize; z1++)
            {
                int indicatorLevel = gridData.GetLevel(x1, z1, placementData.Id);
                if (!indicators.TryGetValue((x1, z1), out var indicator))
                {
                    if (indicatorPool.Count > 0)
                    {
                        indicator = indicatorPool[0];
                        indicatorPool.RemoveAt(0);
                        indicator.transform.parent = transform;
                    }
                    else
                    {
                        GameObject go = Instantiate(indicatorPrefab, transform);
                        indicator = go.GetComponent<CellIndicator>();
                    }
                    indicators[(x1, z1)] = indicator;
                    indicator.DoAdd(this, gridMap.GetLevelPosition(x1, z1, indicatorLevel) + new Vector3(0, gridMap.yHeight, 0));
                }
                keepIndicators.Add(indicator);
                
                float alpha = 0.25f;
                if (gridMap.gridData.IsInsideShape(x1 - x, z1 - z, placementData)) {
                    alpha = 1.0f;
                }

                Color color = new Color(1f, 0.0f, 0.0f, alpha);
                if (!gridData.CanPutLevel(indicatorLevel) || indicatorLevel != targetLevel) {
                    // keep red
                }
                else if (gridData.IsInside(x1, z1) && !gridData.GetCell(x1, z1).isObstacle) {
                    color = new Color(0.0f, 1f, 0.0f, alpha);
                }

                indicator.spriteRenderer.color = color;
            }
        }
        
        removeIndicators.Clear();
        foreach (var kv in indicators) {
            if (!keepIndicators.Contains(kv.Value)) {
                removeIndicators.Add(kv.Key, kv.Value);
            }
        }
        foreach (var kv in removeIndicators) {
            indicators.Remove(kv.Key);
            kv.Value.DoRemove();
        }
    }
}
