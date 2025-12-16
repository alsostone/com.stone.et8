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
            if (self.isMouseDraging)
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
                self.OnEscape();
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
                if (lsUnitView != null && lsUnitView.GetComponent<LSViewPlacementComponent>() != null) {
                    var command = LSCommand.GenCommandLong(0, OperateCommandType.PlacementDrag, lsUnitView.Id);
                    self.Room().SendCommandMeesage(command);
                }
            }
        }

        public static void OnTouchBegin(this LSOperaDragComponent self, Vector3 touchPosition)
        {
            if (!self.isItemClicked)
            {
                self.longTouchPreesToken?.Cancel();
                self.longTouchPreesToken = null;
                
                // 若选框大小未达到一定范围 则选中的单位是当前点击位置的单位
                if (self.RaycastTarget(touchPosition, out GameObject target)) {
                    LSUnitView lsUnitView = target.GetComponent<LSUnitViewBehaviour>()?.LSUnitView;
                    var command = LSCommand.GenCommandLong(0, OperateCommandType.TouchDown, lsUnitView?.Id ?? 0);
                    self.Room().SendCommandMeesage(command);
                    
                    self.SetHoverTarget(lsUnitView);
                    self.ScanTouchLongPress(touchPosition).Coroutine();
                }
            }
            if (self.RaycastTerrain(touchPosition, out Vector3 pos))
            {
                var command = LSCommand.GenCommandFloat2(0, OperateCommandType.TouchDragStart, pos.x, pos.z);
                self.Room().SendCommandMeesage(command);
                self.isDraging = true;
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
            else
            {
                // 非拖拽状态下的才进行悬停检测 特别是在拖拽状态下要保持保持现状
                LSUnitView hover = null;
                if (self.RaycastTarget(touchPosition, out GameObject target))
                {
                    // 有选中单位时禁止悬停检测
                    LSUnitView lsPlayer = self.LSUnitView(self.Room().LookPlayerId);
                    if (!lsPlayer.GetComponent<LSViewSelectionComponent>().HasSelectedUnit())
                    {
                        hover = target.GetComponent<LSUnitViewBehaviour>()?.LSUnitView;
                    }
                }
                self.SetHoverTarget(hover);
            }
        }
        
        private static void SetHoverTarget(this LSOperaDragComponent self, LSUnitView lsUnitView)
        {
            LSUnitView lastHover = self.HoverUnitView;
            if (lastHover != lsUnitView)
            {
                if (lastHover != null)
                {
                    lastHover.GetComponent<ViewEffectComponent>()?.StopFx(ConstValue.FxMouseHoverResId);
                    lastHover.GetComponent<ViewIndicatorComponent>()?.HideRangeIndicator();
                }
                if (lsUnitView != null)
                {
                    lsUnitView.GetComponent<ViewEffectComponent>()?.PlayFx(ConstValue.FxMouseHoverResId, AttachPoint.None).Coroutine();
                    lsUnitView.GetComponent<ViewIndicatorComponent>()?.ShowRangeIndicator();
                }
                self.HoverUnitView = lsUnitView;
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
                    var command = LSCommand.GenCommandLong(0, OperateCommandType.TouchDragCancel, 0);
                    self.Room().SendCommandMeesage(command);
                }
                self.isDraging = false;
                self.isOutsideDraging = false;
                self.isItemClicked = false;
                self.longTouchPreesToken?.Cancel();
                self.longTouchPreesToken = null;
            }
        }
        
        public static void SetPlacementObject(this LSOperaDragComponent self, long itemId, bool disableMouseMove)
        {
            var command = LSCommand.GenCommandLong(0, OperateCommandType.PlacementNew, itemId);
            self.Room().SendCommandMeesage(command);
            self.isOutsideDraging = disableMouseMove;
            self.isItemClicked = true;
        }

        private static void RotatePlacementObject(this LSOperaDragComponent self, int rotation = 1)
        {
            if (self.isItemClicked)
            {
                var command = LSCommand.GenCommandButton(0, CommandButtonType.PlacementRotate, rotation);
                self.Room().SendCommandMeesage(command);
            }
        }
        
        private static void OnEscape(this LSOperaDragComponent self)
        {
            self.isDraging = false;
            self.isOutsideDraging = false;
            self.isItemClicked = false;
            var command = LSCommand.GenCommandLong(0, OperateCommandType.Escape, 0);
            self.Room().SendCommandMeesage(command);
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

        public static bool IsOperaAllDone(this LSOperaDragComponent self)
        {
            return !self.isDraging && !self.isItemClicked;
        }
    }
}