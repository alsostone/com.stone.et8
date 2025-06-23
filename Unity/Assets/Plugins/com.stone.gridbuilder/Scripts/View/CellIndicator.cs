using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CellIndicator : MonoBehaviour
{
    [NonSerialized] public bool removeMark;
    [SerializeField] public SpriteRenderer spriteRenderer;

    private GridMapIndicator owner;
    
    public void DoAdd(GridMapIndicator mapIndicator, Vector3 pos)
    {
        owner = mapIndicator;
        transform.position = pos;
    }
    
    public void DoRemove()
    {
        owner.Recycle(this);
    }
}