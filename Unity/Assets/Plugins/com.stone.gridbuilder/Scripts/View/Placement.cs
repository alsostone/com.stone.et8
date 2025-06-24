using UnityEngine;

public class Placement : MonoBehaviour
{
    [SerializeField, Range(0.1f, 1.0f)] public float putHeight = 0.5f;
    [SerializeField, Range(0.1f, 1.0f)] public float takeHeight = 0.5f;
    [SerializeField, HideInInspector] public PlacementData placementData = new();

    public void Reset()
    {
        placementData.Id = 0;
        placementData.x = 0;
        placementData.z = 0;
    }
    
    public Vector3 GetPosition()
    {
        return transform.position;
    }
    
    public void SetMovePosition(Vector3 pos)
    {
        transform.position = pos + new Vector3(0, putHeight + takeHeight, 0);
    }
    
    public void SetPutPosition(Vector3 pos)
    {
        transform.position = pos + new Vector3(0, putHeight, 0);
    }

    public void Rotation(int r)
    {
        placementData.Rotation(r);
        transform.rotation = Quaternion.Euler(0, placementData.rotation * 90, 0);
    }

    public void Remove()
    {
        DestroyImmediate(gameObject);
    }
    
    public void DoShake()
    {
        BuildingShake shake = GetComponent<BuildingShake>();
        if (shake == null) {
            shake = gameObject.AddComponent<BuildingShake>();
        }
        shake.StartShake(new Vector3(1, 0, 1));
    }
}