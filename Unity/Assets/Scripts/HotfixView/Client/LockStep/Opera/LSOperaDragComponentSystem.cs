using ST.GridBuilder;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSOperaDragComponent))]
    [FriendOf(typeof(LSOperaDragComponent))]
    public static partial class LSOperaDragComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSOperaDragComponent self)
        {
        }
        
        [EntitySystem]
        private static void Update(this LSOperaDragComponent self)
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                    return;
                self.isMouseDraging = true;
                self.OnTouchBegin(Input.mousePosition);
            }
            else if (self.isMouseDraging)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    self.OnTouchEnd(Input.mousePosition);
                    self.isMouseDraging = false;
                }
                else
                {
                    self.OnTouchMove(Input.mousePosition);
                }
            }
            if (!self.isOutsideDraging && !self.isMouseDraging)
            {
                self.OnTouchMove(Input.mousePosition);
            }
#else
            for (int i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch(i);
                if (self.dragFingerId == -1 && touch.phase == TouchPhase.Began)
                {
                    if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                        continue;
                    if (self.OnTouchBegin(touch.position))
                    {
                        self.dragFingerId = touch.fingerId;
                        self.OnTouchMove(touch.position);
                    }
                }
                else if (touch.fingerId == self.dragFingerId && (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary))
                {
                    self.OnTouchMove(touch.position);
                }
                else if (touch.fingerId == self.dragFingerId && touch.phase == TouchPhase.Ended)
                {
                    self.OnTouchEnd(touch.position);
                    self.dragFingerId = -1;
                }
            }
            if (!self.isOutsideDraging && self.dragFingerId == -1)
            {
                self.OnTouchMove(Input.mousePosition);
            }
#endif
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                self.CancelPlacementObject();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                self.RotatePlacementObject(1);
            }
        }
        
        private static bool OnTouchBegin(this LSOperaDragComponent self, Vector3 touchPosition)
        {
            if (!self.isDraging)
            {
                if (self.RaycastTarget(touchPosition, out GameObject target))
                {
                    Placement buiding = target.GetComponent<Placement>();
                    if (buiding != null) {
                        var command = LSCommand.GenCommandLong(0, OperateCommandType.PlacementDragStart, buiding.placementData.id);
                        self.Room().SendCommandMeesage(command);
                        self.isDraging = true;
                        return true;
                    }
                }
            }
            return false;
        }

        public static void OnTouchMove(this LSOperaDragComponent self, Vector3 touchPosition)
        {
            if (self.isDraging)
            {
                if (self.RaycastTerrain(touchPosition, out Vector3 pos)) {
                    var command = LSCommand.GenCommandFloat2(0, OperateCommandType.PlacementDrag, pos.x, pos.z);
                    self.Room().SendCommandMeesage(command);
                }
            }
        }

        public static void OnTouchEnd(this LSOperaDragComponent self, Vector3 touchPosition)
        {
            if (self.isDraging)
            {
                if (self.RaycastTerrain(touchPosition, out Vector3 pos))
                {
                    var command = LSCommand.GenCommandFloat2(0, OperateCommandType.PlacementDragEnd, pos.x, pos.z);
                    self.Room().SendCommandMeesage(command);
                }
                else
                {
                    var command = LSCommand.GenCommandButton(0, CommandButtonType.PlacementCancel);
                    self.Room().SendCommandMeesage(command);
                }
                self.isDraging = false;
                self.isOutsideDraging = false;
            }
        }
        
        public static void SetPlacementObject(this LSOperaDragComponent self, long itemId, bool disableMouseMove)
        {
            var command = LSCommand.GenCommandLong(0, OperateCommandType.PlacementStart, itemId);
            self.Room().SendCommandMeesage(command);
            self.isDraging = true;
            self.isOutsideDraging = disableMouseMove;
        }

        private static void RotatePlacementObject(this LSOperaDragComponent self, int rotation = 1)
        {
            if (self.isDraging)
            {
                var command = LSCommand.GenCommandButton(0, CommandButtonType.PlacementRotate, rotation);
                self.Room().SendCommandMeesage(command);
            }
        }
        
        private static void CancelPlacementObject(this LSOperaDragComponent self)
        {
            if (self.isDraging)
            {
                var command = LSCommand.GenCommandButton(0, CommandButtonType.PlacementCancel);
                self.Room().SendCommandMeesage(command);
                self.isDraging = false;
                self.isOutsideDraging = false;
            }
        }

        private static bool RaycastTerrain(this LSOperaDragComponent self, Vector3 position, out Vector3 pos)
        {
            pos = default;
            
            LSCameraComponent cameraComponent = self.Room().GetComponent<LSCameraComponent>();
            Ray ray = cameraComponent.Camera.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, self.raycastDistance, self.terrainMask)) {
                pos = hit.point;
                return true;
            }

            return false;
        }

        private static bool RaycastTarget(this LSOperaDragComponent self, Vector3 position, out GameObject target)
        {
            target = null;
            
            LSCameraComponent cameraComponent = self.Room().GetComponent<LSCameraComponent>();
            Ray ray = cameraComponent.Camera.ScreenPointToRay(position);
            if (Physics.Raycast(ray, out RaycastHit hit, self.raycastDistance)) {
                target = hit.collider.gameObject;
                return true;
            }
            return false;
        }

    }
}