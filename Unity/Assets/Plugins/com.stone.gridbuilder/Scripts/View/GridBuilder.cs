using UnityEngine;
using UnityEngine.EventSystems;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] public Camera rayCamera;
    [SerializeField] public GridMap gridMap;
    [SerializeField] public float raycastDistance = 1000.0f;
    
    private Building dragBuilding;
    private Vector3Int dragIndex;
    private Vector3 dragOffset;
    
#if !UNITY_EDITOR
    private int dragFingerId = -1;
#endif

    private void Awake()
    {
        if (this.rayCamera == null)
            this.rayCamera = Camera.main;
        if (gridMap == null)
            gridMap = FindObjectOfType<GridMap>();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            OnTouchBegin(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            OnTouchMove(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            this.OnTouchEnd(Input.mousePosition);
        }
#else
        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            if (dragFingerId == -1 && touch.phase == UnityEngine.TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    continue;
                if (OnTouchBegin(touch.position))
                    dragFingerId = touch.fingerId;
            }
            else if (touch.fingerId == dragFingerId && (touch.phase == UnityEngine.TouchPhase.Moved || touch.phase == UnityEngine.TouchPhase.Stationary))
            {
                OnTouchMove(touch.position);
            }
            else if (touch.fingerId == dragFingerId && touch.phase == UnityEngine.TouchPhase.Ended)
            {
                this.OnTouchEnd(touch.position);
                dragFingerId = -1;
            }
        }
        if (dragFingerId == -1)
        {
            OnTouchMove(Input.mousePosition);
        }
#endif
    }
    
    private bool OnTouchBegin(Vector3 touchPosition)
    {
        if (!this.dragBuilding)
        {
            if (RaycastTarget(touchPosition, out var pos, out var target))
            {
                this.dragBuilding = target.GetComponent<Building>();
                if (this.dragBuilding)
                {
                    Vector3 position = this.dragBuilding.transform.position;
                    this.dragIndex = gridMap.ConvertToIndex(position);
                    this.dragOffset = position - pos;
                    return true;
                }
            }
        }
        return false;
    }

    private void OnTouchMove(Vector3 touchPosition)
    {
        if (this.dragBuilding)
        {
            if (RaycastTerrain(touchPosition, out Vector3 pos))
            {
                Vector3Int index = gridMap.ConvertToIndex(pos + this.dragOffset);
                this.dragBuilding.transform.position = gridMap.GetCellPositionCenter(index.x, index.z);
            }
        }
    }

    private void OnTouchEnd(Vector3 touchPosition)
    {
        if (this.dragBuilding)
        {
            if (RaycastTerrain(touchPosition, out Vector3 pos))
            {
                Vector3Int index = gridMap.ConvertToIndex(pos + this.dragOffset);
                if (index != this.dragIndex && gridMap.gridData.CanPut(index.x, index.z, this.dragBuilding.buildingData)) {
                    gridMap.gridData.Take(this.dragIndex.x, this.dragIndex.z, this.dragBuilding.buildingData);
                    gridMap.gridData.Put(index.x, index.z, this.dragBuilding.buildingData);
                    this.dragBuilding.transform.position = gridMap.GetCellPositionCenter(index.x, index.z);
                }
                else {
                    this.dragBuilding.transform.position = gridMap.GetCellPositionCenter(this.dragIndex.x, this.dragIndex.z);
                }
                this.dragBuilding = null;
            }
        }
    }
    
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (RaycastTarget(Input.mousePosition, out Vector3 pos, out var _))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(this.rayCamera.transform.position, pos);
        }
    }
#endif
    
    public bool RaycastTerrain(Vector3 position, out Vector3 pos)
    {
        pos = default;
        
        if (this.rayCamera == null) {
            return false;
        }

        Ray ray = this.rayCamera.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, gridMap.terrainMask)) {
            pos = hit.point;
            return true;
        }

        return false;
    }

    public bool RaycastTarget(Vector3 position, out Vector3 pos, out GameObject target)
    {
        target = null;
        pos = default;
        
        if (this.rayCamera == null) {
            return false;
        }

        Ray ray = this.rayCamera.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance)) {
            pos = hit.point;
            target = hit.collider.gameObject;
            return true;
        }
        return false;
    }

}
