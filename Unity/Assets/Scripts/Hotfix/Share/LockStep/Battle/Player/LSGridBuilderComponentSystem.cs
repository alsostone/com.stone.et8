using ST.GridBuilder;
using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(LSGridBuilderComponent))]
    [FriendOf(typeof(LSGridBuilderComponent))]
    public static partial class LSGridBuilderComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSGridBuilderComponent self)
        {
        }
        
        public static void RunCommandEscape(this LSGridBuilderComponent self)
        {
            self.ClearPlacementData();
            
            SelectionComponent selectionComponent = self.LSOwner().GetComponent<SelectionComponent>();
            selectionComponent.ClearSelection();
        }
        
        public static void RunCommandTouchDown(this LSGridBuilderComponent self, long targetId)
        {
            self.TouchDownTargetId = targetId;
        }

        public static void RunCommandTouchDragStart(this LSGridBuilderComponent self, TSVector2 position)
        {
            SelectionComponent selectionComponent = self.LSOwner().GetComponent<SelectionComponent>();
            selectionComponent.ClearSelection();
            
            self.DragStartPosition = new TSVector(position.x, 0, position.y);
            EventSystem.Instance.Publish(self.LSWorld(), new LSTouchDragStart() { Id = self.LSOwner().Id, Position = position });
        }

        public static void RunCommandTouchDrag(this LSGridBuilderComponent self, TSVector2 position)
        {
            EventSystem.Instance.Publish(self.LSWorld(), new LSTouchDrag() { Id = self.LSOwner().Id, Position = position });
        }

        public static void RunCommandTouchDragEnd(this LSGridBuilderComponent self, TSVector2 position)
        {
            LSWorld lsWorld = self.LSWorld();
            LSUnit lsOwner = self.LSOwner();

            TSVector positionV3 = new(position.x, 0, position.y);
            if (self.PlacementTargetId > 0)
            {
                LSGridMapComponent lsGridMapComponent = lsWorld.GetComponent<LSGridMapComponent>();
                IndexV2 index = lsGridMapComponent.ConvertToIndex(positionV3 + self.PlacementDragOffset);

                LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
                LSUnit lsUnit = lsUnitComponent.GetChild<LSUnit>(self.PlacementTargetId);
                PlacementData placementData = lsUnit?.GetComponent<PlacementComponent>()?.GetPlacementData();
                if (placementData != null && lsGridMapComponent.CanPut(index.x, index.z, placementData) && lsGridMapComponent.CanTake(placementData))
                {
                    lsGridMapComponent.Take(placementData);
                    lsGridMapComponent.Put(index.x, index.z, placementData);
                    EventSystem.Instance.Publish(self.LSWorld(), new LSUnitPlaced() { Id = lsUnit.Id, X = index.x, Z = index.z });
                    lsUnit.GetComponent<TransformComponent>().SetPosition(lsGridMapComponent.GetPutPosition(placementData));
                }
            }
            else if (self.PlacementItemId > 0)
            {
                // 放置新单位时需要消耗卡包中的卡牌
                CardBagComponent bagComponent = lsOwner.GetComponent<CardBagComponent>();
                var item = bagComponent.GetItem(self.PlacementItemId);
                if (item != null)
                {
                    TeamType teamType = lsOwner.GetComponent<TeamComponent>().Type;
                    bool isOk = false;
                    if (item.Type == EUnitType.Block) {
                        isOk = null != LSUnitFactory.CreateBlock(lsWorld, item.TableId, positionV3, self.PlacementRotation * 90, teamType);
                    }
                    else if (item.Type == EUnitType.Building) {
                        isOk = null != LSUnitFactory.CreateBuilding(lsWorld, item.TableId, positionV3, self.PlacementRotation * 90, teamType);
                    }
                    else if (item.Type == EUnitType.Item) {
                        isOk = ItemExecutor.TryExecute(lsOwner, positionV3, item.TableId);
                    }
                    if (isOk) {
                        bagComponent.RemoveItem(self.PlacementItemId);
                    }
                }
            }
            else
            {
                if (TSVector.SqrDistance(self.DragStartPosition, positionV3) > FP.EN2)
                {
                    SelectionComponent selectionComponent = lsOwner.GetComponent<SelectionComponent>();
                    TSBounds bounds = new TSBounds();
                    bounds.SetMinMax(TSVector.Min(self.DragStartPosition, positionV3), TSVector.Max(self.DragStartPosition, positionV3));
                    selectionComponent.SelectUnitsInBounds(bounds);
                }
                else
                {
                    SelectionComponent selectionComponent = lsOwner.GetComponent<SelectionComponent>();
                    selectionComponent.SelectSingleUnit(self.TouchDownTargetId);
                }
            }
            self.ClearPlacementData();
            EventSystem.Instance.Publish(lsWorld, new LSTouchDragEnd() { Id = lsOwner.Id });
        }

        public static void RunCommandPlacementDrag(this LSGridBuilderComponent self, long targetId)
        {
            self.ClearPlacementData();
            
            // 拖拽已有单位时需要计算偏移量
            LSUnitComponent lsUnitComponent = self.LSWorld().GetComponent<LSUnitComponent>();
            TransformComponent transformComponent = lsUnitComponent.GetChild<LSUnit>(targetId)?.GetComponent<TransformComponent>();
            self.PlacementDragOffset = transformComponent != null ? transformComponent.Position - self.DragStartPosition : TSVector.zero;
            self.PlacementTargetId = targetId;
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementDragStart() { Id = self.LSOwner().Id, TargetId = targetId });
        }

        public static void RunCommandPlacementNew(this LSGridBuilderComponent self, long itemId)
        {
            self.ClearPlacementData();
            
            // 放置新单位时需要判定卡包中是否有此卡牌
            LSUnit lsOwner = self.LSOwner();
            CardBagComponent bagComponent = lsOwner.GetComponent<CardBagComponent>();
            if (bagComponent.HasItem(itemId)) {
                self.PlacementItemId = itemId;
                self.PlacementDragOffset = TSVector.zero;
            }
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementStart() { Id = lsOwner.Id, ItemId = itemId });
        }

        public static void RunCommandPlacementRotate(this LSGridBuilderComponent self, int rotation)
        {self.LSRoom()?.ProcessLog.LogFunction(72, self.LSParent().Id, rotation);
            if (self.PlacementItemId > 0) {
                self.PlacementRotation += rotation;
            }
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementRotate() { Id = self.LSOwner().Id, Rotation = rotation });
        }

        public static void RunCommandPlacementCancel(this LSGridBuilderComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(71, self.LSParent().Id);
            self.ClearPlacementData();
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementCancel() { Id = self.LSOwner().Id });
        }
        
        private static void ClearPlacementData(this LSGridBuilderComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(14, self.LSParent().Id);
            self.TouchDownTargetId = 0;
            self.PlacementTargetId = 0;
            self.PlacementRotation = 0;
            self.PlacementItemId = 0;
            self.PlacementDragOffset = TSVector.zero;
        }
    }
}