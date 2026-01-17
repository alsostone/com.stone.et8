using UnityEngine;
using UnityEngine.EventSystems;

namespace ET.Client
{
    [LSEntitySystemOf(typeof(LSOperaDragComponent))]
    [EntitySystemOf(typeof(LSOperaDragComponent))]
    [FriendOf(typeof(LSOperaDragComponent))]
    public static partial class LSOperaDragComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSOperaDragComponent self)
        {
            self.lineShooting = GameObject.Find("RaycastShooting").GetComponent<LineRenderer>();
            self.lineShooting.positionCount = 2;
            self.ResetOprationMode(self.Room().LSWorld.OperationMode);
        }
        
        [LSEntitySystem]
        private static void LSRollback(this LSOperaDragComponent self)
        {
            self.ResetOprationMode(self.Room().LSWorld.OperationMode);
        }
        
        [EntitySystem]
        private static void Update(this LSOperaDragComponent self)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                self.OnEscape();
            }
            if (Input.GetKeyDown(KeyCode.R)) {
                self.RotatePlacementObject(1);
            }
            
#if UNITY_STANDALONE || UNITY_EDITOR
            self.HandleMouseMoveTo();
            
            if (Input.GetMouseButtonDown(0))
            {
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(PointerInputModule.kMouseLeftId))
                {
                    self.isMouseDraging = true;
                    self.mousePosition = Input.mousePosition;
                    self.OnTouchBegin(Input.mousePosition);
                }
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
                    if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        self.dragFingerId = touch.fingerId;
                        self.OnTouchBegin(touch.position);
                    }
                }
                if (touch.fingerId == self.dragFingerId)
                {
                    if (touch.phase == TouchPhase.Moved)
                    {
                        self.OnTouchMove(touch.position);
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        self.OnTouchEnd(touch.position);
                        self.dragFingerId = -1;
                    }
                }
            }
#endif
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
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(PointerInputModule.kMouseRightId))
            {
                if (Input.GetMouseButtonDown(1))
                {
                    MovementMode movementMode = self.isKeyADown ? MovementMode.AttackMove : MovementMode.Move;
                    self.isKeyADown = false;
                    if (self.RaycastTerrain(Input.mousePosition, out Vector3 pos))
                    {
                        var command = LSCommand.GenCommandMoveTo(0, movementMode, pos.x, pos.z);
                        self.Room().SendCommandMeesage(command);
                    }
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
        
        public static void ResetOprationMode(this LSOperaDragComponent self, OperationMode operationMode)
        {
            if (self.operationMode == operationMode) {
                return;
            }
            switch (self.operationMode)
            {
                case OperationMode.Dragging:
                {
                    self.isDragging = false;
                    self.isOutsideDraging = false;
                    self.isItemClicked = false;
                    self.longTouchPreesToken?.Cancel();
                    self.longTouchPreesToken = null;
                    break;
                }
                case OperationMode.Shooting:
                {
                    self.isDragging = false;
                    self.lineShooting.enabled = false;
                    break;
                }
            }

            self.operationMode = operationMode;
            switch (self.operationMode)
            {
                case OperationMode.Shooting:
                {
                    self.lineShooting.enabled = true;
                    break;
                }
            }
        }

        public static void OnTouchBegin(this LSOperaDragComponent self, Vector3 touchPosition)
        {
            switch (self.operationMode)
            {
                case OperationMode.Dragging:
                {
                    if (!self.isItemClicked)
                    {
                        self.longTouchPreesToken?.Cancel();
                        self.longTouchPreesToken = null;
                    
                        // 若选框大小未达到一定范围 则选中的单位是当前点击位置的单位
                        if (self.RaycastTarget(touchPosition, out GameObject target)) {
                            LSUnitView lsUnitView = target.GetComponent<LSUnitViewBehaviour>()?.LSUnitView;
                            var command = LSCommand.GenCommandLong(0, OperateCommandType.TouchDownTarget, lsUnitView?.Id ?? 0);
                            self.Room().SendCommandMeesage(command);
                        
                            self.SetHoverTarget(lsUnitView);
                            self.ScanTouchLongPress(touchPosition).Coroutine();
                        }
                    }
                    if (self.RaycastTerrain(touchPosition, out Vector3 pos))
                    {
                        var command = LSCommand.GenCommandFloat2(0, OperateCommandType.TouchDragStart, pos.x, pos.z);
                        self.Room().SendCommandMeesage(command);
                        self.isDragging = true;
                    }
                    break;
                }
                case OperationMode.Shooting:
                {
                    if (self.RaycastShoting(touchPosition, out Vector3 _, out Vector3 to))
                    {
                        var command = LSCommand.GenCommandFloat2(0, OperateCommandType.TouchDragStart, to.x, to.z);
                        self.Room().SendCommandMeesage(command);
                        self.isDragging = true;
                    }
                    break;
                }
            }
        }

        public static void OnTouchMove(this LSOperaDragComponent self, Vector3 touchPosition)
        {
            switch (self.operationMode)
            {
                case OperationMode.Dragging:
                {
                    if (self.isDragging)
                    {
                        if (self.RaycastTerrain(touchPosition, out Vector3 pos))
                        {
                            var command = LSCommand.GenCommandFloat2(0, OperateCommandType.TouchDrag, pos.x, pos.z);
                            self.Room().SendCommandMeesage(command);
                        }
                    }
                    else
                    {
                        // 非拖拽状态下的才进行悬停检测 特别是在拖拽状态下要保持现状
                        LSUnitView hover = null;
                        if (self.RaycastTarget(touchPosition, out GameObject target))
                        {
                            // 有选中单位时禁止悬停检测
                            LSUnitView lsPlayer = self.Room().GetLookPlayerView();
                            if (!lsPlayer.GetComponent<LSViewSelectionComponent>().HasSelectedUnit())
                            {
                                hover = target.GetComponent<LSUnitViewBehaviour>()?.LSUnitView;
                            }
                        }

                        self.SetHoverTarget(hover);
                    }
                    break;
                }
                case OperationMode.Shooting:
                {
                    if (self.RaycastShoting(touchPosition, out Vector3 from, out Vector3 to))
                    {
                        var command = LSCommand.GenCommandFloat2(0, OperateCommandType.TouchDrag, to.x, to.z);
                        self.Room().SendCommandMeesage(command);
                        self.lineShooting.SetPosition(0, from);
                        self.lineShooting.SetPosition(1, to);
                    }
                    break;
                }
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
                    lsUnitView.GetComponent<ViewEffectComponent>()?.PlayFx(ConstValue.FxMouseHoverResId, AttachPoint.None);
                    lsUnitView.GetComponent<ViewIndicatorComponent>()?.ShowRangeIndicator();
                }
                self.HoverUnitView = lsUnitView;
            }
        }

        public static void OnTouchEnd(this LSOperaDragComponent self, Vector3 touchPosition)
        {
            if (!self.isDragging) return;
            self.isDragging = false;
            
            switch (self.operationMode)
            {
                case OperationMode.Dragging:
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
                    self.isOutsideDraging = false;
                    self.isItemClicked = false;
                    self.longTouchPreesToken?.Cancel();
                    self.longTouchPreesToken = null;
                    break;
                }
                case OperationMode.Shooting:
                {
                    if (self.RaycastShoting(touchPosition, out Vector3 _, out Vector3 to))
                    {
                        var command = LSCommand.GenCommandFloat2(0, OperateCommandType.TouchDragEnd, to.x, to.z);
                        self.Room().SendCommandMeesage(command);
                    }
                    break;
                }
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
            self.isDragging = false;
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
        
        private static bool RaycastShoting(this LSOperaDragComponent self, Vector3 position, out Vector3 from, out Vector3 to)
        {
            from = to = default;
            LSUnitView lsView = self.Room().GetLookCampView();
            if (lsView != null)
            {
                LSCameraComponent cameraComponent = self.Room().GetComponent<LSCameraComponent>();
                Ray ray = cameraComponent.Camera.ScreenPointToRay(position);
                if (Physics.Raycast(ray, out RaycastHit hit, self.raycastDistance, self.terrainMask2))
                {
                    // 二次射线检测 避免地形、障碍等遮挡情形
                    LSViewTransformComponent viewTransformComponent = lsView.GetComponent<LSViewTransformComponent>();
                    from = viewTransformComponent.GetAttachPoint(AttachPoint.Head);
                    Ray ray2 = new Ray(from, hit.point - from);
                    if (Physics.Raycast(ray2, out RaycastHit hit2, self.raycastDistance, self.terrainMask2)) {
                        to = hit2.point;
                        return true;
                    }
                }
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
            return !self.isDragging && !self.isItemClicked;
        }
    }
}