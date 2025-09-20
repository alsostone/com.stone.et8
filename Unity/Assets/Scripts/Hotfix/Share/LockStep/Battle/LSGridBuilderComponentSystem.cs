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

        public static void RunCommandPlacementDragStart(this LSGridBuilderComponent self, long targetId)
        {self.LSRoom()?.ProcessLog.LogFunction(76, self.LSParent().Id);
            self.ClearPlacementData();
            
            self.PlacementTargetId = targetId;
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementDragStart() { Id = self.LSOwner().Id, TargetId = targetId });
        }

        public static void RunCommandPlacementDrag(this LSGridBuilderComponent self, TSVector2 position)
        {self.LSRoom()?.ProcessLog.LogFunction(75, self.LSParent().Id);
            // 第一个拖拽消息到达，记录偏移（因为单指令携带信息有限，本应在DragStart时记录）
            if (self.PlacementTargetId > 0 && FP.MaxValue.Equals(self.PlacementDragOffset.y)) {
                LSUnitComponent lsUnitComponent = self.LSWorld().GetComponent<LSUnitComponent>();
                TransformComponent transformComponent = lsUnitComponent.GetChild<LSUnit>(self.PlacementTargetId)?.GetComponent<TransformComponent>();
                self.PlacementDragOffset = transformComponent != null
                        ? new TSVector2(transformComponent.Position.x - position.x, transformComponent.Position.z - position.y)
                        : new TSVector2(0, 0);
            }
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementDrag() { Id = self.LSOwner().Id, Position = position });
        }

        public static void RunCommandPlacementDragEnd(this LSGridBuilderComponent self, TSVector2 position)
        {self.LSRoom()?.ProcessLog.LogFunction(74, self.LSParent().Id);
            LSWorld lsWorld = self.LSWorld();
            LSUnit lsOwner = self.LSOwner();
            position += self.PlacementDragOffset;
            
            if (self.PlacementTargetId > 0)
            {
                LSGridMapComponent lsGridMapComponent = lsWorld.GetComponent<LSGridMapComponent>();
                IndexV2 index = lsGridMapComponent.ConvertToIndex(position);
                GridData gridData = lsGridMapComponent.GetGridData();
                
                LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
                LSUnit lsUnit = lsUnitComponent.GetChild<LSUnit>(self.PlacementTargetId);
                PlacementData placementData = lsUnit?.GetComponent<PlacementComponent>()?.GetPlacementData();
                if (placementData != null && gridData.CanPut(index.x, index.z, placementData) && gridData.CanTake(placementData))
                {
                    gridData.Take(placementData);
                    gridData.Put(index.x, index.z, placementData);
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
                    TSVector pos = new(position.x, 0, position.y);
                    if (item.Type == EUnitType.Block) {
                        LSUnitFactory.CreateBlock(lsWorld, item.TableId, pos, self.PlacementRotation * 90, teamType);
                    }
                    else if (item.Type == EUnitType.Building) {
                        LSUnitFactory.CreateBuilding(lsWorld, item.TableId, pos, self.PlacementRotation * 90, teamType);
                    }
                    bagComponent.RemoveItem(self.PlacementItemId);
                }
            }
            self.ClearPlacementData();
            EventSystem.Instance.Publish(lsWorld, new LSPlacementDragEnd() { Id = lsOwner.Id, Position = position });
        }

        public static void RunCommandPlacementStart(this LSGridBuilderComponent self, long itemId)
        {
            self.ClearPlacementData();
            
            // 放置新单位时需要判定卡包中是否有此卡牌
            LSUnit lsOwner = self.LSOwner();
            CardBagComponent bagComponent = lsOwner.GetComponent<CardBagComponent>();
            if (bagComponent.HasItem(itemId)) {
                self.PlacementItemId = itemId;
                self.PlacementDragOffset = new TSVector2(0, 0);
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
            self.PlacementTargetId = 0;
            self.PlacementRotation = 0;
            self.PlacementItemId = 0;
            self.PlacementDragOffset = new TSVector2(FP.MaxValue, FP.MaxValue);
        }
    }
}