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
        {self.LSRoom()?.ProcessLog.LogFunction(111, self.LSParent().Id);
            self.ClearPlacementData();
            EventSystem.Instance.Publish(self.LSWorld(), new LSEscape() { Id = self.LSOwner().Id });
        }
        
        public static void RunCommandTouchDown(this LSGridBuilderComponent self, long targetId)
        {self.LSRoom()?.ProcessLog.LogFunction(110, self.LSParent().Id);
            self.TouchDownTargetId = targetId;
        }

        public static void RunCommandTouchDragStart(this LSGridBuilderComponent self, TSVector2 position)
        {self.LSRoom()?.ProcessLog.LogFunction(109, self.LSParent().Id, position.x.V, position.y.V);
            LSUnit lsOwner = self.LSOwner();
            SelectionComponent selectionComponent = lsOwner.GetComponent<SelectionComponent>();
            selectionComponent.ClearSelection();
            
            self.DragStartPosition = new TSVector(position.x, 0, position.y);
            EventSystem.Instance.Publish(self.LSWorld(), new LSTouchDragStart() { Id = lsOwner.Id, Position = position });
        }

        public static void RunCommandTouchDrag(this LSGridBuilderComponent self, TSVector2 position)
        {self.LSRoom()?.ProcessLog.LogFunction(108, self.LSParent().Id, position.x.V, position.y.V);
            EventSystem.Instance.Publish(self.LSWorld(), new LSTouchDrag() { Id = self.LSOwner().Id, Position = position });
        }

        public static void RunCommandTouchDragEnd(this LSGridBuilderComponent self, TSVector2 position)
        {self.LSRoom()?.ProcessLog.LogFunction(107, self.LSParent().Id, position.x.V, position.y.V);
            LSWorld lsWorld = self.LSWorld();
            LSUnit lsOwner = self.LSOwner();

            TSVector positionV3 = new(position.x, 0, position.y);
            if (self.PlacementTargetId > 0)
            {
                LSUnit lsUnit = self.LSUnit(self.PlacementTargetId);
                if (lsUnit != null)
                {
                    PlacementComponent placementComponent = lsUnit.GetComponent<PlacementComponent>();
                    placementComponent?.PutToPosition(positionV3 + self.PlacementDragOffset);
                }
            }
            if (self.PlacementItemId > 0)
            {
                // 放置新单位时需要消耗卡包中的卡牌
                CardBagComponent bagComponent = lsOwner.GetComponent<CardBagComponent>();
                var item = bagComponent.GetItem(self.PlacementItemId);
                if (item != null)
                {
                    TeamType teamType = lsOwner.GetComponent<TeamComponent>().Type;
                    bool isOk = false;
                    if (item.Type == EUnitType.Block) {
                        isOk = null != LSUnitFactory.CreateBlock(lsOwner, item.TableId, positionV3, self.PlacementRotation * 90, teamType);
                    }
                    else if (item.Type == EUnitType.Building) {
                        isOk = null != LSUnitFactory.CreateBuilding(lsOwner, item.TableId, positionV3, self.PlacementRotation * 90, teamType);
                    }
                    else if (item.Type == EUnitType.Item) {
                        isOk = ItemExecutor.TryExecute(lsOwner, positionV3, item.TableId);
                    }
                    if (isOk) {
                        bagComponent.RemoveItem(self.PlacementItemId);
                    }
                }
            }
            if (self.PlacementTargetId == 0 && self.PlacementItemId == 0)
            {
                SelectionComponent selectionComponent = lsOwner.GetComponent<SelectionComponent>();
                if (TSVector.SqrDistance(self.DragStartPosition, positionV3) > FP.EN2)
                {
                    TSBounds bounds = new TSBounds();
                    bounds.SetMinMax(TSVector.Min(self.DragStartPosition, positionV3), TSVector.Max(self.DragStartPosition, positionV3));
                    selectionComponent.SelectUnitsInBounds(bounds);
                }
                else
                {
                    selectionComponent.SelectSingleUnit(self.TouchDownTargetId);
                }
            }
            self.ClearPlacementData();
            EventSystem.Instance.Publish(lsWorld, new LSTouchDragEnd() { Id = lsOwner.Id });
        }

        public static void RunCommandTouchDragCancel(this LSGridBuilderComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(103, self.LSParent().Id);
            self.ClearPlacementData();
            EventSystem.Instance.Publish(self.LSWorld(), new LSTouchDragCancel() { Id = self.LSOwner().Id });
        }

        public static void RunCommandPlacementDrag(this LSGridBuilderComponent self, long targetId)
        {self.LSRoom()?.ProcessLog.LogFunction(106, self.LSParent().Id);
            LSUnit lsTarget = self.LSUnit(targetId);
            if (lsTarget == null || lsTarget.GetComponent<PlacementComponent>() == null) {
                return;
            }
            self.ClearPlacementData();
            
            // 拖拽已有单位时需要计算偏移量
            TransformComponent transformComponent = lsTarget.GetComponent<TransformComponent>();
            self.PlacementDragOffset = transformComponent.Position - self.DragStartPosition;
            self.PlacementTargetId = targetId;
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementDrag() { Id = self.LSOwner().Id, TargetId = targetId });
        }

        public static void RunCommandPlacementNew(this LSGridBuilderComponent self, long itemId)
        {self.LSRoom()?.ProcessLog.LogFunction(105, self.LSParent().Id);
            self.ClearPlacementData();
            
            // 放置新单位时需要判定卡包中是否有此卡牌
            LSUnit lsOwner = self.LSOwner();
            CardBagComponent bagComponent = lsOwner.GetComponent<CardBagComponent>();
            if (bagComponent.HasItem(itemId)) {
                self.PlacementItemId = itemId;
                self.PlacementDragOffset = TSVector.zero;
            }
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementNew() { Id = lsOwner.Id, ItemId = itemId });
        }

        public static void RunCommandPlacementRotate(this LSGridBuilderComponent self, int rotation)
        {self.LSRoom()?.ProcessLog.LogFunction(104, self.LSParent().Id, rotation);
            if (self.PlacementItemId > 0) {
                self.PlacementRotation += rotation;
            }
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementRotate() { Id = self.LSOwner().Id, Rotation = rotation });
        }

        private static void ClearPlacementData(this LSGridBuilderComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(102, self.LSParent().Id);
            self.TouchDownTargetId = 0;
            self.PlacementTargetId = 0;
            self.PlacementRotation = 0;
            self.PlacementItemId = 0;
            self.PlacementDragOffset = TSVector.zero;
        }
    }
}