using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class CellIndicator : MonoBehaviour
{
    [SerializeField] private float durationAdd = 0.1f;
    [SerializeField] private float durationRemove = 0.2f;
    [SerializeField] public SpriteRenderer spriteRenderer;

    private GridMapIndicator owner;
    
    public void DoAdd(GridMapIndicator mapIndicator, Vector3 pos)
    {
        StopAllCoroutines();
        
        owner = mapIndicator;
        transform.position = pos;
        StartCoroutine(AnimateScale(durationAdd, Vector3.zero, Vector3.one * 0.95f));
    }
    
    public void DoRemove()
    {
        StopAllCoroutines();
        
        Vector3 start = transform.localScale;
        StartCoroutine(AnimateScale(durationRemove, start, Vector3.zero, () => 
        {
            owner.Recycle(this);
        }));
    }
    
    private IEnumerator AnimateScale(float t, Vector3 start, Vector3 end, Action onComplete = null)
    {
        float time = 0;
        while (time < t)
        {
            transform.localScale = Vector3.Lerp(start, end, time / t);
            time += Time.deltaTime;
            yield return null;
        }
        transform.localScale = end;
        onComplete?.Invoke();
    }
}