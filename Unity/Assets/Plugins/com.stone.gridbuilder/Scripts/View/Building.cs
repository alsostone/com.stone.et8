using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField, Range(0.1f, 1.0f)] public float takeHeight = 0.5f;
    [SerializeField, HideInInspector] public BuildingData buildingData = new();

    public Vector3 Take()
    {
        Transform transform1 = transform;
        Vector3 pos = transform1.position;
        transform1.position = pos + new Vector3(0, takeHeight, 0);
        return pos;
    }
    
    public void SetPosition(Vector3 pos)
    {
        transform.position = pos + new Vector3(0, takeHeight, 0);
    }
    
    public void PutPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void Remove()
    {
        DestroyImmediate(gameObject);
    }
}