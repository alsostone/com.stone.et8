using UnityEngine;
using UnityEngine.EventSystems;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] public Camera rayCamera;
    [SerializeField] public GridMap gridMap;
    [SerializeField] public float raycastDistance = 1000.0f;

    private bool isNewBuilding;
    private Building dragBuilding;
    private Vector3Int dragIndex;
    private Vector3 dragOffset;
    private int dragFingerId = -1;

    private void Awake()
    {
        if (rayCamera == null)
            rayCamera = Camera.main;
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
#endif
        if (dragFingerId == -1)
        {
            OnTouchMove(Input.mousePosition);
        }
    }
    
    private bool OnTouchBegin(Vector3 touchPosition)
    {
        if (!dragBuilding)
        {
            if (RaycastTarget(touchPosition, out var pos, out var target))
            {
                dragBuilding = target.GetComponent<Building>();
                if (dragBuilding)
                {
                    Vector3 position = dragBuilding.Take();
                    dragIndex = gridMap.ConvertToIndex(position);
                    dragOffset = position - pos;
                    return true;
                }
            }
        }
        return false;
    }

    private void OnTouchMove(Vector3 touchPosition)
    {
        if (dragBuilding)
        {
            if (RaycastTerrain(touchPosition, out Vector3 pos))
            {
                Vector3Int index = gridMap.ConvertToIndex(pos + dragOffset);
                dragBuilding.SetPosition(gridMap.GetCellPositionCenter(index.x, index.z));
            }
        }
    }

    private void OnTouchEnd(Vector3 touchPosition)
    {
        if (dragBuilding)
        {
            if (RaycastTerrain(touchPosition, out Vector3 pos))
            {
                Vector3Int index = gridMap.ConvertToIndex(pos + dragOffset);
                if (gridMap.gridData.CanPut(index.x, index.z, dragBuilding.buildingData))
                {
                    if (isNewBuilding)
                    {
                        dragBuilding.buildingData.Id = gridMap.gridData.GetNextGuid();
                        gridMap.gridData.Put(index.x, index.z, dragBuilding.buildingData);
                    }
                    else if (index != dragIndex)
                    {
                        gridMap.gridData.Take(dragIndex.x, dragIndex.z, dragBuilding.buildingData);
                        gridMap.gridData.Put(index.x, index.z, dragBuilding.buildingData);
                    }
                    dragBuilding.PutPosition(gridMap.GetCellPositionCenter(index.x, index.z));
                }
                else {
                    if (isNewBuilding) {
                        dragBuilding.Remove();
                    } else {
                        dragBuilding.PutPosition(gridMap.GetCellPositionCenter(dragIndex.x, dragIndex.z));
                    }
                }
            } else {
                if (isNewBuilding) {
                    dragBuilding.Remove();
                } else {
                    dragBuilding.PutPosition(gridMap.GetCellPositionCenter(dragIndex.x, dragIndex.z));
                }
            }
            dragBuilding = null;
            isNewBuilding = false;
        }
    }
    
    public void CanclePlacementBuilding()
    {
        if (dragBuilding)
        {
            if (isNewBuilding) {
                dragBuilding.Remove();
            } else {
                dragBuilding.PutPosition(gridMap.GetCellPositionCenter(dragIndex.x, dragIndex.z));
            }
            dragBuilding = null;
            isNewBuilding = false;
        }
    }
    
    public void SetPlacementBuilding(Building building)
    {
        if (dragBuilding) {
            dragBuilding.PutPosition(gridMap.GetCellPositionCenter(dragIndex.x, dragIndex.z));
        }
        dragBuilding = building;
        isNewBuilding = true;
        dragIndex = Vector3Int.zero;
        dragOffset = Vector3.zero;
    }
    
    public bool RaycastTerrain(Vector3 position, out Vector3 pos)
    {
        pos = default;
        
        if (rayCamera == null) {
            return false;
        }

        Ray ray = rayCamera.ScreenPointToRay(position);
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
        
        if (rayCamera == null) {
            return false;
        }

        Ray ray = rayCamera.ScreenPointToRay(position);
        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance)) {
            pos = hit.point;
            target = hit.collider.gameObject;
            return true;
        }
        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (RaycastTarget(Input.mousePosition, out Vector3 pos, out var _))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(rayCamera.transform.position, pos);
        }
    }
#endif

}
