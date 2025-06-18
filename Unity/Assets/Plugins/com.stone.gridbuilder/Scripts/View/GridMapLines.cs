using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GridMapLines : MonoBehaviour
{
    public GridMap gridMap;
    
    [SerializeField] public LayerMask terrainMask;
    [SerializeField, Range(0.01f, 0.2f)] public float lineHeight = 0.1f;
}
