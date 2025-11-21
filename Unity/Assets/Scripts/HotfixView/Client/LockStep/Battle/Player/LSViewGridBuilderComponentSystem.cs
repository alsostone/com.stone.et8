using System.Collections.Generic;
using ST.GridBuilder;
using TrueSync;
using UnityEngine;

namespace ET.Client
{
    [LSEntitySystemOf(typeof(LSViewGridBuilderComponent))]
    [EntitySystemOf(typeof(LSViewGridBuilderComponent))]
    [FriendOf(typeof(LSViewGridBuilderComponent))]
    [FriendOf(typeof(LSViewPlacementComponent))]
    public static partial class LSViewGridBuilderComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSViewGridBuilderComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this LSViewGridBuilderComponent self)
        {
            self.OnEscape();
        }
        
        [LSEntitySystem]
        private static void LSRollback(this LSViewGridBuilderComponent self)
        {
            // 拖拽过程中逻辑层回滚可能会导致拖拽开始被取消（真实拖拽指令晚于预测指令），但被延后的拖拽开始指令终会被重新执行。
            // 重新执行时由于判定了DragPlacement是否存在，表现层不会重复创建DragPlacement实例，不会有问题。
            // 预测拖拽开始正常执行，若权威拖拽开始莫名丢失导致回滚，表现层有拖拽表现，但逻辑层没有拖拽状态，拖拽无效，表现层拖拽结束时会取消拖拽表现，问题不大。
            // 而且由于表现层拖拽表现未发生改变也就不会闪烁，不处理回滚更优。
        }

        public static void OnEscape(this LSViewGridBuilderComponent self)
        {
            self.IsDragging = false;
            self.StopAndClear();
        }
        
        private static void StopAndClear(this LSViewGridBuilderComponent self)
        {
            if (self.DragPlacement == null && self.DragItemRow == null)
            {
                self.Fiber().UIEvent(new UISelectDragEndEvent() { PlayerId = self.LSViewOwner().Id }).Coroutine();
            }
            if (self.DragPlacement)
            {
                self.DragPlacement.ResetPreviewMaterial();
                if (self.DragPlacement.placementData.isNew) {
                    ResourcesPoolComponent poolComponent = self.Room().GetComponent<ResourcesPoolComponent>();
                    poolComponent.Recycle(self.DragPlacement.gameObject);
                } else {
                    LSUnitView lsUnitView = self.DragUnitView;
                    lsUnitView?.GetComponent<LSViewTransformComponent>().SetTransformEnabled(true);
                }
                self.DragPlacement = null;
                self.DragUnitView = null;
                
                LSViewGridMapComponent gridMapComponent = self.Room().GetComponent<LSViewGridMapComponent>();
                if (gridMapComponent.GridMapIndicator) {
                    gridMapComponent.GridMapIndicator.ClearIndicator();
                }
                self.Fiber().UIEvent(new UICardDragEndEvent() { PlayerId = self.LSViewOwner().Id }).Coroutine();
            }
            if (self.DragItemRow != null)
            {
                // TODO: 取消物品使用目标高亮
                self.DragItemRow = null;
                self.Fiber().UIEvent(new UICardDragEndEvent() { PlayerId = self.LSViewOwner().Id }).Coroutine();
                self.Fiber().UIEvent(new UIArrowDragEndEvent() { PlayerId = self.LSViewOwner().Id }).Coroutine();
            }
        }

        public static void OnTouchDragStart(this LSViewGridBuilderComponent self, TSVector2 position)
        {
            self.IsDragging = true;
            self.DragStartPosition = self.DragPosition = new(position.x.AsFloat(), 0, position.y.AsFloat());
            if (self.DragPlacement)
            {
                self.SetPlacementDragPosition(self.DragStartPosition);
            }
            if (self.DragItemRow != null)
            {
                self.SetPlacementItemPosition(self.DragStartPosition);
            }
            if (self.DragPlacement == null && self.DragItemRow == null)
            {
                self.Fiber().UIEvent(new UISelectDragStartEvent() { PlayerId = self.LSViewOwner().Id, Position = self.DragStartPosition }).Coroutine();
            }
        }

        public static void OnTouchDrag(this LSViewGridBuilderComponent self, TSVector2 position)
        {
            self.DragPosition = new(position.x.AsFloat(), 0, position.y.AsFloat());
            if (self.DragPlacement)
            {
                self.SetPlacementDragPosition(self.DragPosition);
            }
            if (self.DragItemRow != null)
            {
                self.SetPlacementItemPosition(self.DragPosition);
            }
            if (self.DragPlacement == null && self.DragItemRow == null)
            {
                self.Fiber().UIEvent(new UISelectDragEvent() { PlayerId = self.LSViewOwner().Id, Position = self.DragPosition }).Coroutine();
            }
        }

        public static void OnTouchDragEnd(this LSViewGridBuilderComponent self)
        {
            self.OnEscape();
        }

        public static void OnTouchDragCancel(this LSViewGridBuilderComponent self)
        {
            self.OnEscape();
        }

        public static void OnPlacementDrag(this LSViewGridBuilderComponent self, long targetId)
        {
            self.StopAndClear();
            
            LSUnitViewComponent unitViewComponent = self.Room().GetComponent<LSUnitViewComponent>();
            LSUnitView lsUnitView = unitViewComponent.GetChild<LSUnitView>(targetId);
            Placement placement = lsUnitView?.GetComponent<LSViewPlacementComponent>()?.Placement;
            if (placement == null)
                return;
            
            LSViewGridMapComponent gridMapComponent = self.Room().GetComponent<LSViewGridMapComponent>();
            if (!gridMapComponent.CanTake(placement.placementData)) {
                placement.DoShake();
                return;
            }
            
            lsUnitView.GetComponent<LSViewTransformComponent>().SetTransformEnabled(false);
            self.DragPlacement = placement;
            self.DragUnitView = lsUnitView;
            self.DragPlacement.SetPreviewMaterial();
            self.DragOffset = self.DragPlacement.GetPosition() - self.DragStartPosition;
            self.SetPlacementDragPosition(self.DragPosition);
            
            self.Fiber().UIEvent(new UICardDragStartEvent() { PlayerId = self.LSViewOwner().Id }).Coroutine();
        }

        public static void OnPlacementNew(this LSViewGridBuilderComponent self, long itemId)
        {
            self.StopAndClear();
            
            LSViewCardBagComponent viewCardBagComponent = self.LSViewOwner().GetComponent<LSViewCardBagComponent>();
            CardBagItem item = viewCardBagComponent.GetItem(itemId);
            if (item == null)
                return;
            
            switch (item.Type)
            {
                case EUnitType.Block:
                {
                    TbBlockRow row = TbBlock.Instance.Get(item.TableId);
                    if (self.CreatePlacementPreview(row.Model))
                        self.Fiber().UIEvent(new UICardDragStartEvent() { PlayerId = self.LSViewOwner().Id }).Coroutine();
                    break;
                }
                case EUnitType.Building:
                {
                    TbBuildingRow row = TbBuilding.Instance.Get(item.TableId);
                    if (self.CreatePlacementPreview(row.Model))
                        self.Fiber().UIEvent(new UICardDragStartEvent() { PlayerId = self.LSViewOwner().Id }).Coroutine();
                    break;
                }
                case EUnitType.Item:
                {
                    self.DragItemRow = TbItem.Instance.Get(item.TableId);
                    self.Fiber().UIEvent(new UICardDragStartEvent() { PlayerId = self.LSViewOwner().Id }).Coroutine();
                    self.Fiber().UIEvent(new UIArrowDragStartEvent() { PlayerId = self.LSViewOwner().Id, ItemId = itemId }).Coroutine();
                    break;
                }
            }
        }

        private static bool CreatePlacementPreview(this LSViewGridBuilderComponent self, int targetModel)
        {
            TbResourceRow resourceRow = TbResource.Instance.Get(targetModel);
            if (resourceRow == null) {
                return false;
            }

            ResourcesPoolComponent poolComponent = self.Room().GetComponent<ResourcesPoolComponent>();
            GlobalComponent globalComponent = self.Root().GetComponent<GlobalComponent>();
            GameObject go = poolComponent.Fetch(resourceRow.Url, globalComponent.Unit, true);
            
            Placement placement = go.GetComponent<Placement>();
            if (placement == null) {
                poolComponent.Recycle(go);
                return false;
            }
            
            LSViewGridMapComponent gridMapComponent = self.Room().GetComponent<LSViewGridMapComponent>();
            self.DragPlacement = placement;
            self.DragPlacement.ResetRotation(gridMapComponent.GetGridRotation());
            self.DragPlacement.SetPreviewMaterial();
            self.DragOffset = Vector3.zero;
            
            // 拖拽还没有开始时，把它放到屏幕外
            if (!self.IsDragging) {
                LSCameraComponent cameraComponent = self.Room().GetComponent<LSCameraComponent>();
                self.DragPlacement.SetPosition(cameraComponent.Camera.transform.up * 1000f);
            } else {
                self.SetPlacementDragPosition(self.DragPosition);
            }
            return true;
        }

        public static void OnPlacementRotate(this LSViewGridBuilderComponent self, int rotation)
        {
            if (self.DragPlacement)
            {
                if (self.DragPlacement.placementData.isNew) {
                    LSViewGridMapComponent gridMapComponent = self.Room().GetComponent<LSViewGridMapComponent>();
                    self.DragPlacement.Rotation(rotation, gridMapComponent.GetGridRotation());
                }
            }
        }

        private static void SetPlacementItemPosition(this LSViewGridBuilderComponent self, Vector3 position)
        {
            List<SearchUnit> targets = ObjectPool.Instance.Fetch<List<SearchUnit>>();
            LSViewQuery.Search(self.DragItemRow.SearchTarget, self.LSViewOwner(), position, targets);
            // TODO: 设置物品使用目标高亮
                
            targets.Clear();
            ObjectPool.Instance.Recycle(targets);
                
            self.Fiber().UIEvent(new UIArrowDragEvent() { PlayerId = self.LSViewOwner().Id, Position = position }).Coroutine();
        }
        
        private static void SetPlacementDragPosition(this LSViewGridBuilderComponent self, Vector3 position)
        {
            LSViewGridMapComponent gridMapComponent = self.Room().GetComponent<LSViewGridMapComponent>();

            IndexV2 index = gridMapComponent.ConvertToIndex(position + self.DragOffset);
            int targetLevel = gridMapComponent.GetShapeLevelCount(index.x, index.z, self.DragPlacement.placementData);
            self.DragPlacement.SetPosition(gridMapComponent.GetLevelPosition(index.x, index.z, targetLevel, self.DragPlacement.takeHeight));
            
            if (gridMapComponent.GridMapIndicator) {
                gridMapComponent.GridMapIndicator.GenerateIndicator(index.x, index.z, targetLevel, self.DragPlacement.placementData);
            }
        }
    }
}