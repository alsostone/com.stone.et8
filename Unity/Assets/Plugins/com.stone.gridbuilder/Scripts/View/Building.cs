using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField, Range(0.1f, 1.0f)] public float putHeight = 0.5f;
    [SerializeField, Range(0.1f, 1.0f)] public float takeHeight = 0.5f;
    [SerializeField, HideInInspector] public BuildingData buildingData = new();

    public void Reset()
    {
        buildingData.Id = 0;
        buildingData.x = 0;
        buildingData.z = 0;
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
        buildingData.Rotation(r);
        transform.rotation = Quaternion.Euler(0, buildingData.rotation * 90, 0);
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