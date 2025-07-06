using ST.GridBuilder;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSGridBuilderComponent))]
    [FriendOf(typeof(LSGridBuilderComponent))]
    public static partial class LSGridBuilderComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSGridBuilderComponent self)
        {
            self.gridMap = UnityEngine.Object.FindObjectOfType<GridMap>();
            self.gridMapIndicator = UnityEngine.Object.FindObjectOfType<GridMapIndicator>();
        }
        
        [EntitySystem]
        private static void Update(this LSGridBuilderComponent self)
        {
            if (self.gridMap == null) {
                return;
            }
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                    return;
                self.OnTouchBegin(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                self.OnTouchEnd(Input.mousePosition);
            }
#else
            for (int i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch(i);
                if (self.dragFingerId == -1 && touch.phase == UnityEngine.TouchPhase.Began)
                {
                    if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        continue;
                    if (self.OnTouchBegin(touch.position))
                        self.dragFingerId = touch.fingerId;
                }
                else if (touch.fingerId == self.dragFingerId && (touch.phase == UnityEngine.TouchPhase.Moved || touch.phase == UnityEngine.TouchPhase.Stationary))
                {
                    self.OnTouchMove(touch.position);
                }
                else if (touch.fingerId == self.dragFingerId && touch.phase == UnityEngine.TouchPhase.Ended)
                {
                    self.OnTouchEnd(touch.position);
                    self.dragFingerId = -1;
                }
            }
#endif
            if (self.dragFingerId == -1)
            {
                self.OnTouchMove(Input.mousePosition);
            }
        }
        
        private static bool OnTouchBegin(this LSGridBuilderComponent self, Vector3 touchPosition)
        {
            if (!self.dragPlacement)
            {
                if (self.RaycastTarget(touchPosition, out GameObject target))
                {
                    Placement buiding = target.GetComponent<Placement>();
                    if (!buiding) {
                        return false;
                    }

                    Vector3 position = buiding.GetPosition();
                    if (self.gridMap.gridData.CanTake(buiding.placementData))
                    {
                        self.dragPlacement = buiding;
                        self.dragPlacement.SetPreviewMaterial();
                        self.RaycastTerrain(touchPosition, out Vector3 pos);
                        self.dragOffset = position - pos;
                        return true;
                    }
                    buiding.DoShake();
                }
            }
            return false;
        }

        private static void OnTouchMove(this LSGridBuilderComponent self, Vector3 touchPosition)
        {
            if (self.dragPlacement)
            {
                if (self.RaycastTerrain(touchPosition, out Vector3 pos))
                {
                    IndexV2 index = self.gridMap.ConvertToIndex(pos + self.dragOffset);
                    int targetLevel = self.gridMap.gridData.GetShapeLevelCount(index.x, index.z, self.dragPlacement.placementData);
                    self.dragPlacement.SetMovePosition(self.gridMap.GetLevelPosition(index.x, index.z, targetLevel));
                    if (self.gridMapIndicator) {
                        self.gridMapIndicator.GenerateIndicator(index.x, index.z, targetLevel, self.dragPlacement.placementData);
                    }
                }
            }
        }

        private static void OnTouchEnd(this LSGridBuilderComponent self, Vector3 touchPosition)
        {
            if (self.dragPlacement)
            {
                self.dragPlacement.ResetPreviewMaterial();
                if (self.RaycastTerrain(touchPosition, out Vector3 pos))
                {
                    IndexV2 index = self.gridMap.ConvertToIndex(pos + self.dragOffset);
                    if (self.gridMap.gridData.CanPut(index.x, index.z, self.dragPlacement.placementData))
                    {
                        if (self.isNewBuilding)
                        {
                            self.dragPlacement.placementData.id = self.gridMap.gridData.GetNextGuid();
                            self.gridMap.gridData.Put(index.x, index.z, self.dragPlacement.placementData);
                            self.gridMap.gridData.ResetFlowField();
                        }
                        else if (index.x != self.dragPlacement.placementData.x || index.z != self.dragPlacement.placementData.z)
                        {
                            self.gridMap.gridData.Take(self.dragPlacement.placementData);
                            self.gridMap.gridData.Put(index.x, index.z, self.dragPlacement.placementData);
                            self.gridMap.gridData.ResetFlowField();
                        }
                        self.dragPlacement.SetPutPosition(self.gridMap.GetPutPosition(self.dragPlacement.placementData));
                    }
                    else {
                        if (self.isNewBuilding) {
                            self.dragPlacement.Remove();
                        } else {
                            self.dragPlacement.SetPutPosition(self.gridMap.GetPutPosition(self.dragPlacement.placementData));
                        }
                    }
                } else {
                    if (self.isNewBuilding) {
                        self.dragPlacement.Remove();
                    } else {
                        self.dragPlacement.SetPutPosition(self.gridMap.GetPutPosition(self.dragPlacement.placementData));
                    }
                }
                self.dragPlacement = null;
                self.isNewBuilding = false;
                self.dragOffset = Vector3.zero;
                if (self.gridMapIndicator) {
                    self.gridMapIndicator.ClearIndicator();
                }
            }
        }
        
        public static void ClearPlacementObject(this LSGridBuilderComponent self)
        {
            if (self.dragPlacement)
            {
                if (self.isNewBuilding) {
                    self.dragPlacement.Remove();
                } else {
                    self.dragPlacement.SetPutPosition(self.gridMap.GetPutPosition(self.dragPlacement.placementData));
                }
                self.dragPlacement = null;
                self.isNewBuilding = false;
                self.dragOffset = Vector3.zero;
            }
        }
        
        public static void SetPlacementObject(this LSGridBuilderComponent self, Placement placement)
        {
            if (self.dragPlacement)
            {
                if (self.isNewBuilding) {
                    self.dragPlacement.Remove();
                } else {
                    self.dragPlacement.SetPutPosition(self.gridMap.GetPutPosition(self.dragPlacement.placementData));
                }
            }
            if (placement) {
                placement.Reset();
                self.dragPlacement = placement;
                self.dragPlacement.SetPreviewMaterial();
                self.isNewBuilding = true;
                self.dragOffset = Vector3.zero;
            }
        }

        public static void RotationPlacementBuilding(this LSGridBuilderComponent self)
        {
            if (self.dragPlacement)
            {
                if (self.isNewBuilding) {
                    self.dragPlacement.Rotation(1);
                }
            }
        }
        
        private static bool RaycastTerrain(this LSGridBuilderComponent self, Vector3 position, out Vector3 pos)
        {
            pos = default;
            
            LSCameraComponent cameraComponent = self.GetParent<Room>().GetComponent<LSCameraComponent>();
            Ray ray = cameraComponent.Camera.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, self.raycastDistance, self.gridMap.terrainMask)) {
                pos = hit.point;
                return true;
            }

            return false;
        }

        private static bool RaycastTarget(this LSGridBuilderComponent self, Vector3 position, out GameObject target)
        {
            target = null;
            
            LSCameraComponent cameraComponent = self.GetParent<Room>().GetComponent<LSCameraComponent>();
            Ray ray = cameraComponent.Camera.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, self.raycastDistance)) {
                target = hit.collider.gameObject;
                return true;
            }
            return false;
        }

        private static void SendCommandMeesage(this LSGridBuilderComponent self, Room room, ulong command)
        {
            C2Room_FrameMessage sendFrameMessage = C2Room_FrameMessage.Create();
            sendFrameMessage.Frame = room.PredictionFrame + 1;
            sendFrameMessage.Command = command;
            room.Root().GetComponent<ClientSenderComponent>().Send(sendFrameMessage);
            
            room.GetComponent<LSCommandsComponent>().AddCommand(room.PredictionFrame + 1, command);
        }

    }
}