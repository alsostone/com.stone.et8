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

        public static void RunCommandPlacementDragStart(this LSGridBuilderComponent self, LSUnit lsPlayer, long targetId)
        {self.LSRoom()?.ProcessLog.LogFunction(76, self.LSParent().Id);
            if (self.PlacementType == EUnitType.None && self.PlacementTargetId == 0)
            {
                self.PlacementTargetId = targetId;
                self.PlacementDragOffset = new TSVector2(FP.MaxValue, FP.MaxValue);
                TeamType teamPlacer = lsPlayer.GetComponent<TeamComponent>().Type;
                EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementDragStart() { TeamPlacer = teamPlacer, TargetId = targetId });
            }
        }

        public static void RunCommandPlacementDrag(this LSGridBuilderComponent self, LSUnit lsPlayer, TSVector2 position)
        {self.LSRoom()?.ProcessLog.LogFunction(75, self.LSParent().Id);
            if (self.PlacementType == EUnitType.None && self.PlacementTargetId == 0)
                return;

            // 第一个拖拽消息到达，记录偏移（因为单指令携带信息有限，本应在DragStart时记录）
            if (self.PlacementTargetId > 0 && FP.MaxValue.Equals(self.PlacementDragOffset.y)) {
                LSUnitComponent lsUnitComponent = self.LSWorld().GetComponent<LSUnitComponent>();
                TransformComponent transformComponent = lsUnitComponent.GetChild<LSUnit>(self.PlacementTargetId)?.GetComponent<TransformComponent>();
                self.PlacementDragOffset = transformComponent != null
                        ? new TSVector2(transformComponent.Position.x - position.x, transformComponent.Position.z - position.y)
                        : new TSVector2(0, 0);
            }
            TeamType teamPlacer = lsPlayer.GetComponent<TeamComponent>().Type;
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementDrag() { TeamPlacer = teamPlacer, Position = position });
        }

        public static void RunCommandPlacementDragEnd(this LSGridBuilderComponent self, LSUnit lsPlayer, TSVector2 position)
        {self.LSRoom()?.ProcessLog.LogFunction(74, self.LSParent().Id);
            if (self.PlacementType == EUnitType.None && self.PlacementTargetId == 0)
                return;

            TeamType teamPlacer = lsPlayer.GetComponent<TeamComponent>().Type;
            position += self.PlacementDragOffset;
            LSWorld lsWorld = self.LSWorld();
            
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
            else
            {
                // 放置新单位时需要消耗卡包中的卡牌
                CardBagComponent bagComponent = lsPlayer.GetComponent<CardBagComponent>();
                bagComponent.RemoveItem(self.PlacementType, self.PlacementTableId, 1);
                
                TSVector pos = new(position.x, 0, position.y);
                if (self.PlacementType == EUnitType.Block) {
                    LSUnitFactory.CreateBlock(lsWorld, self.PlacementTableId, pos, self.PlacementRotation * 90, teamPlacer);
                }
                else if (self.PlacementType == EUnitType.Building) {
                    LSUnitFactory.CreateBuilding(lsWorld, self.PlacementTableId, pos, self.PlacementRotation * 90, teamPlacer);
                }
            }
            self.ClearPlacementData();
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementDragEnd() { TeamPlacer = teamPlacer, Position = position });
        }

        public static void RunCommandPlacementStart(this LSGridBuilderComponent self, LSUnit lsPlayer, EUnitType type, int tableId)
        {self.LSRoom()?.ProcessLog.LogFunction(89, self.LSParent().Id, tableId);
            // 放置新单位时需要判定卡包中是否有此卡牌
            CardBagComponent bagComponent = lsPlayer.GetComponent<CardBagComponent>();
            if (bagComponent.GetItemCount(type, tableId) <= 0)
                return;
            
            self.PlacementTargetId = 0;
            self.PlacementRotation = 0;
            self.PlacementType = type;
            self.PlacementTableId = tableId;
            self.PlacementDragOffset = new TSVector2(0, 0);
            TeamType teamPlacer = lsPlayer.GetComponent<TeamComponent>().Type;
            LSPlacementStart placementStart = new () { TeamPlacer = teamPlacer, Type = type, TableId = tableId };
            EventSystem.Instance.Publish(self.LSWorld(), placementStart);
        }

        public static void RunCommandPlacementRotate(this LSGridBuilderComponent self, TeamType teamPlacer, int rotation)
        {self.LSRoom()?.ProcessLog.LogFunction(72, self.LSParent().Id, rotation);
            if (self.PlacementType == EUnitType.None)
                return;
            
            self.PlacementRotation += rotation;
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementRotate() { TeamPlacer = teamPlacer, Rotation = rotation });
        }

        public static void RunCommandPlacementCancel(this LSGridBuilderComponent self, TeamType teamPlacer)
        {self.LSRoom()?.ProcessLog.LogFunction(71, self.LSParent().Id);
            if (self.PlacementType == EUnitType.None && self.PlacementTargetId == 0)
                return;

            self.ClearPlacementData();
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementCancel() { TeamPlacer = teamPlacer });
        }
        
        private static void ClearPlacementData(this LSGridBuilderComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(14, self.LSParent().Id);
            self.PlacementTargetId = 0;
            self.PlacementRotation = 0;
            self.PlacementType = EUnitType.None;
            self.PlacementTableId = 0;
            self.PlacementDragOffset = new TSVector2(FP.MaxValue, FP.MaxValue);
        }
    }
}