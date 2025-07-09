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
        {
            if (self.PlacementType == EUnitType.None && self.PlacementTargetId == 0)
            {
                self.PlacementTargetId = targetId;
                self.PlacementDragOffset = new TSVector2(FP.MaxValue, FP.MaxValue);
                EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementDragStart() { Id = self.LSOwner().Id, TargetId = targetId });
            }
        }

        public static void RunCommandPlacementDrag(this LSGridBuilderComponent self, TSVector2 position)
        {
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
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementDrag() { Id = self.LSOwner().Id, Position = position });
        }

        public static void RunCommandPlacementDragEnd(this LSGridBuilderComponent self, TSVector2 position)
        {
            if (self.PlacementType == EUnitType.None && self.PlacementTargetId == 0)
                return;
            
            LSWorld lsWorld = self.LSWorld();
            LSGridMapComponent lsGridMapComponent = lsWorld.GetComponent<LSGridMapComponent>();
            IndexV2 index = lsGridMapComponent.ConvertToIndex(position + self.PlacementDragOffset);
            GridData gridData = lsGridMapComponent.GetGridData();
            
            LSUnitComponent lsUnitComponent = lsWorld.GetComponent<LSUnitComponent>();
            if (self.PlacementTargetId > 0)
            {
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
                LSUnit lsUnit = null;
                if (self.PlacementType == EUnitType.Block)
                {
                    TeamType team = self.LSOwner().GetComponent<TeamComponent>().Type;
                    lsUnit = LSUnitFactory.CreateBlock(lsWorld, self.PlacementTableId, team);
                }
                else if (self.PlacementType == EUnitType.Building)
                {
                    TeamType team = self.LSOwner().GetComponent<TeamComponent>().Type;
                    lsUnit = LSUnitFactory.CreateBuilding(lsWorld, self.PlacementTableId, self.PlacementLevel, team);
                }
                PlacementData placementData = lsUnit?.GetComponent<PlacementComponent>()?.GetPlacementData();
                if (placementData != null)
                {
                    placementData.Rotation(self.PlacementRotation);
                    if (gridData.CanPut(index.x, index.z, placementData))
                    {
                        placementData.id = lsUnit.Id;
                        gridData.Put(index.x, index.z, placementData);

                        TSVector pos = lsGridMapComponent.GetPutPosition(placementData);
                        TSQuaternion rot = TSQuaternion.Euler(0, placementData.rotation * 90, 0);
                        lsUnit.AddComponent<TransformComponent, TSVector, TSQuaternion>(pos, rot);
                        EventSystem.Instance.Publish(lsWorld, new LSUnitCreate() {LSUnit = lsUnit});
                    }
                    else {
                        lsUnit.Dispose();
                    }
                }
                else {
                    lsUnit.Dispose();
                }
            }
            self.ClearPlacementData();
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementDragEnd() { Id = self.LSOwner().Id, Position = position });
        }

        public static void RunCommandPlacementStart(this LSGridBuilderComponent self, EUnitType type, int tableId, int level)
        {
            self.PlacementTargetId = 0;
            self.PlacementRotation = 0;
            self.PlacementType = type;
            self.PlacementTableId = tableId;
            self.PlacementLevel = level;
            self.PlacementDragOffset = new TSVector2(0, 0);
            LSPlacementStart placementStart = new () { Id = self.LSOwner().Id, Type = type, TableId = tableId, Level = level };
            EventSystem.Instance.Publish(self.LSWorld(), placementStart);
        }

        public static void RunCommandPlacementRotate(this LSGridBuilderComponent self, int rotation)
        {
            if (self.PlacementType == EUnitType.None)
                return;
            
            self.PlacementRotation += rotation;
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementRotate() { Id = self.LSOwner().Id, Rotation = rotation });
        }

        public static void RunCommandPlacementCancel(this LSGridBuilderComponent self)
        {
            if (self.PlacementType == EUnitType.None && self.PlacementTargetId == 0)
                return;

            self.ClearPlacementData();
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementCancel() { Id = self.LSOwner().Id });
        }
        
        private static void ClearPlacementData(this LSGridBuilderComponent self)
        {
            self.PlacementTargetId = 0;
            self.PlacementRotation = 0;
            self.PlacementType = EUnitType.None;
            self.PlacementTableId = 0;
            self.PlacementLevel = 0;
            self.PlacementDragOffset = new TSVector2(FP.MaxValue, FP.MaxValue);
        }
    }
}