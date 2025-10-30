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
            self.HandleMouseMoveTo();
            
            if (Input.GetMouseButtonDown(0))
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                    return;
                self.isMouseDraging = true;
                self.mousePosition = Input.mousePosition;
                self.OnTouchBegin(Input.mousePosition);
            }
            else if (self.isMouseDraging)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    self.OnTouchEnd(Input.mousePosition);
                    self.isMouseDraging = false;
                }
                else if (self.mousePosition != Input.mousePosition)
                {
                    self.mousePosition = Input.mousePosition;
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
                    self.dragFingerId = touch.fingerId;
                    self.OnTouchBegin(touch.position);
                }
                else if (touch.fingerId == self.dragFingerId && touch.phase == TouchPhase.Moved)
                {
                    self.OnTouchMove(touch.position);
                }
                else if (touch.fingerId == self.dragFingerId && touch.phase == TouchPhase.Ended)
                {
                    self.OnTouchEnd(touch.position);
                    self.dragFingerId = -1;
                }
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

        [EntitySystem]
        private static void Destroy(this LSOperaDragComponent self)
        {
            self.longTouchPreesToken?.Cancel();
            self.longTouchPreesToken = null;
        }
        
        private static void HandleMouseMoveTo(this LSOperaDragComponent self)
        {
            if (Input.GetKeyDown(KeyCode.A)) {
                self.isKeyADown = true;
            }
            if (Input.GetKeyDown(KeyCode.Escape)) {
                self.isKeyADown = false;
            }
            if (Input.GetMouseButtonDown(1))
            {
                MovementMode movementMode = self.isKeyADown ? MovementMode.AttackMove : MovementMode.Move;
                self.isKeyADown = false;
                
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                    return;
                if (self.RaycastTerrain(Input.mousePosition, out Vector3 pos))
                {
                    var command = LSCommand.GenCommandMoveTo(0, movementMode, pos.x, pos.z);
                    self.Room().SendCommandMeesage(command);
                }
            }
        }

        private static async ETTask ScanTouchLongPress(this LSOperaDragComponent self, Vector3 touchPosition)
        {
            self.longTouchPreesToken = new ETCancellationToken();
            await self.Root().GetComponent<TimerComponent>().WaitAsync(600, self.longTouchPreesToken);
            if (self.longTouchPreesToken.IsCancel()) {
                self.longTouchPreesToken = null;
                return;
            }
            self.longTouchPreesToken.Cancel();
            self.longTouchPreesToken = null;
            if (Vector3.Distance(touchPosition, self.mousePosition) > 10f) {
                return;
            }
            if (self.RaycastTarget(touchPosition, out GameObject target))
            {
                LSUnitView lsUnitView = target.GetComponent<LSUnitViewBehaviour>()?.LSUnitView;
                if (lsUnitView != null) {
                    var command = LSCommand.GenCommandLong(0, OperateCommandType.PlacementDragStart, lsUnitView.Id);
                    self.Room().SendCommandMeesage(command);
                    self.isDraging = true;
                }
            }
        }

        private static void OnTouchBegin(this LSOperaDragComponent self, Vector3 touchPosition)
        {
            if (!self.isDraging)
            {
                if (self.RaycastTerrain(touchPosition, out Vector3 pos2))
                {
                    var command = LSCommand.GenCommandFloat2(0, OperateCommandType.TouchDragStart, pos2.x, pos2.z);
                    self.Room().SendCommandMeesage(command);
                    self.isDraging = true;
                }
                self.longTouchPreesToken?.Cancel();
                self.longTouchPreesToken = null;
                self.ScanTouchLongPress(touchPosition).Coroutine();
            }
            else if (self.RaycastTerrain(touchPosition, out Vector3 pos))
            {
                // 若已是建造状态，点击场景时就要把预览物体移动到点击位置 所以发送一个拖拽中
                var command = LSCommand.GenCommandFloat2(0, OperateCommandType.TouchDrag, pos.x, pos.z);
                self.Room().SendCommandMeesage(command);
            }
        }

        public static void OnTouchMove(this LSOperaDragComponent self, Vector3 touchPosition)
        {
            if (self.isDraging)
            {
                if (self.RaycastTerrain(touchPosition, out Vector3 pos)) {
                    var command = LSCommand.GenCommandFloat2(0, OperateCommandType.TouchDrag, pos.x, pos.z);
                    self.Room().SendCommandMeesage(command);
                }
            }
            else if (self.RaycastTarget(touchPosition, out GameObject target))
            {
                LSUnitView lsUnitView = target.GetComponent<LSUnitViewBehaviour>()?.LSUnitView;
                // TODO: 这里可以播放鼠标悬停到物体的特效 如描边高亮
            }
        }

        public static void OnTouchEnd(this LSOperaDragComponent self, Vector3 touchPosition)
        {
            if (self.isDraging)
            {
                if (self.RaycastTerrain(touchPosition, out Vector3 pos))
                {
                    var command = LSCommand.GenCommandFloat2(0, OperateCommandType.TouchDragEnd, pos.x, pos.z);
                    self.Room().SendCommandMeesage(command);
                }
                else
                {
                    var command = LSCommand.GenCommandButton(0, CommandButtonType.PlacementCancel);
                    self.Room().SendCommandMeesage(command);
                }
                self.isDraging = false;
                self.isOutsideDraging = false;
                self.longTouchPreesToken?.Cancel();
                self.longTouchPreesToken = null;
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