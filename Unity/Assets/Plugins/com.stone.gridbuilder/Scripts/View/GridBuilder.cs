using UnityEngine;
using UnityEngine.EventSystems;

public class GridBuilder : MonoBehaviour
{
    [SerializeField] public Camera rayCamera;
    [SerializeField] public GridMap gridMap;
    [SerializeField] public GridMapIndicator gridMapIndicator;
    [SerializeField] public float raycastDistance = 1000.0f;

    private bool isNewBuilding;
    private Placement dragPlacement;
    private Vector3 dragOffset;
    private int dragFingerId = -1;

    private void Awake()
    {
        if (rayCamera == null)
            rayCamera = Camera.main;
        if (gridMap == null)
            gridMap = FindObjectOfType<GridMap>();
        if (gridMapIndicator == null)
            gridMapIndicator = FindObjectOfType<GridMapIndicator>();
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
            OnTouchEnd(Input.mousePosition);
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
                OnTouchEnd(touch.position);
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
        if (!dragPlacement)
        {
            if (RaycastTarget(touchPosition, out var pos, out var target))
            {
                Placement buiding = target.GetComponent<Placement>();
                if (!buiding) {
                    return false;
                }

                Vector3 position = buiding.GetPosition();
                if (gridMap.gridData.CanTake(buiding.placementData))
                {
                    dragPlacement = buiding;
                    dragOffset = position - pos;
                    return true;
                }
                buiding.DoShake();
            }
        }
        return false;
    }

    private void OnTouchMove(Vector3 touchPosition)
    {
        if (dragPlacement)
        {
            if (RaycastTerrain(touchPosition, out Vector3 pos))
            {
                Vector3Int index = gridMap.ConvertToIndex(pos + dragOffset);
                int targetLevel = gridMap.gridData.GetLevel(index.x, index.z, dragPlacement.placementData);
                dragPlacement.SetMovePosition(gridMap.GetLevelPosition(index.x, index.z, targetLevel));
                if (gridMapIndicator) {
                    gridMapIndicator.GenerateIndicator(index.x, index.z, targetLevel, dragPlacement.placementData);
                }
            }
        }
    }

    private void OnTouchEnd(Vector3 touchPosition)
    {
        if (dragPlacement)
        {
            if (RaycastTerrain(touchPosition, out Vector3 pos))
            {
                Vector3Int index = gridMap.ConvertToIndex(pos + dragOffset);
                if (gridMap.gridData.CanPut(index.x, index.z, dragPlacement.placementData))
                {
                    if (isNewBuilding)
                    {
                        dragPlacement.placementData.Id = gridMap.gridData.GetNextGuid();
                        gridMap.gridData.Put(index.x, index.z, dragPlacement.placementData);
                    }
                    else if (index.x != dragPlacement.placementData.x || index.z != dragPlacement.placementData.z)
                    {
                        gridMap.gridData.Take(dragPlacement.placementData);
                        gridMap.gridData.Put(index.x, index.z, dragPlacement.placementData);
                    }
                    dragPlacement.SetPutPosition(gridMap.GetPutPosition(dragPlacement.placementData));
                }
                else {
                    if (isNewBuilding) {
                        dragPlacement.Remove();
                    } else {
                        dragPlacement.SetPutPosition(gridMap.GetPutPosition(dragPlacement.placementData));
                    }
                }
            } else {
                if (isNewBuilding) {
                    dragPlacement.Remove();
                } else {
                    dragPlacement.SetPutPosition(gridMap.GetPutPosition(dragPlacement.placementData));
                }
            }
            dragPlacement = null;
            isNewBuilding = false;
            dragOffset = Vector3.zero;
            if (gridMapIndicator) {
                gridMapIndicator.ClearIndicator();
            }
        }
    }
    
    public void ClearPlacementBuilding()
    {
        if (dragPlacement)
        {
            if (isNewBuilding) {
                dragPlacement.Remove();
            } else {
                dragPlacement.SetPutPosition(gridMap.GetPutPosition(dragPlacement.placementData));
            }
            dragPlacement = null;
            isNewBuilding = false;
            dragOffset = Vector3.zero;
        }
    }
    
    public void SetPlacementBuilding(Placement placement)
    {
        if (dragPlacement)
        {
            if (isNewBuilding) {
                dragPlacement.Remove();
            } else {
                dragPlacement.SetPutPosition(gridMap.GetPutPosition(dragPlacement.placementData));
            }
        }
        if (placement) {
            placement.Reset();
            dragPlacement = placement;
            isNewBuilding = true;
            dragOffset = Vector3.zero;
        }
    }

    public void RotationPlacementBuilding()
    {
        if (dragPlacement)
        {
            if (isNewBuilding)
            {
                dragPlacement.Rotation(1);
            }
            else
            {
                Debug.LogWarning("Cannot rotate a building that is already placed.");
            }
        }
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
