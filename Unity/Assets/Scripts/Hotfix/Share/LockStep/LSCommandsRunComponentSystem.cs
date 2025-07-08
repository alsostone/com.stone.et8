using ST.GridBuilder;
using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(LSCommandsRunComponent))]
    [LSEntitySystemOf(typeof(LSCommandsRunComponent))]
    [FriendOf(typeof(LSCommandsRunComponent))]
    public static partial class LSCommandsRunComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSCommandsRunComponent self)
        {
            self.MoveAxis = TSVector2.zero;
        }

        [LSEntitySystem]
        private static void LSUpdate(this LSCommandsRunComponent self)
        {
            LSUnit unit = self.LSOwner();
            foreach (ulong command in self.Commands)
            {
                switch (LSCommand.ParseCommandType(command))
                {
                    case OperateCommandType.Move:
                        {
                            self.MoveAxis = LSCommand.ParseCommandFloat24x2(command).normalized;
                            break;
                        }
                    case OperateCommandType.PlacementDragStart:
                        {
                            long targetId = (long)LSCommand.ParseCommandLong48(command);
                            self.RunCommandPlacementDragStart(targetId);
                            break;
                        }
                    case OperateCommandType.PlacementDrag:
                        {
                            TSVector2 pos = LSCommand.ParseCommandFloat24x2(command);
                            self.RunCommandPlacementDrag(pos);
                            break;
                        }
                    case OperateCommandType.PlacementDragEnd:
                        {
                            TSVector2 pos = LSCommand.ParseCommandFloat24x2(command);
                            self.RunCommandPlacementDragEnd(pos);
                            break;
                        }
                    case OperateCommandType.PlacementStart:
                        {
                            ulong param = LSCommand.ParseCommandLong48(command);
                            EUnitType type = (EUnitType)((param >> 40) & 0xFF);
                            int level = (int)((param >> 32) & 0xFF);
                            int tableId = (int)(param & 0xFFFFFFFF);
                            self.RunCommandPlacementStart(type, tableId, level);
                            break;
                        }
                    case OperateCommandType.Button:
                        {
                            self.RunCommandButton(unit, command);
                            break;
                        }
                }
            }
            self.Commands.Clear();

            TransformComponent transformComponent = unit.GetComponent<TransformComponent>();
            transformComponent.Move(self.MoveAxis);
        }

        private static void RunCommandButton(this LSCommandsRunComponent self, LSUnit unit, ulong command)
        {
            (CommandButtonType, long) button = LSCommand.ParseCommandButton(command);
            switch (button.Item1)
            {
                case CommandButtonType.PlacementRotate:
                    self.RunCommandPlacementRotate((int)button.Item2);
                    break;
                case CommandButtonType.PlacementCancel:
                    self.RunCommandPlacementCancel();
                    break;
                case CommandButtonType.Attack:
                    unit.GetComponent<SkillComponent>().TryCastSkill(ESkillType.Normal);
                    break;
                case CommandButtonType.Skill1:
                    unit.GetComponent<SkillComponent>().TryCastSkill(ESkillType.Active, 0);
                    break;
                case CommandButtonType.Skill2:
                    unit.GetComponent<SkillComponent>().TryCastSkill(ESkillType.Active, 1);
                    break;
                case CommandButtonType.Jump:
                    {
                        //unit.GetComponent<LSJumpComponent>().Jump();
                        break;
                    }
            }
        }

        private static void RunCommandPlacementDragStart(this LSCommandsRunComponent self, long targetId)
        {
            if (self.PlacementType == EUnitType.None && self.PlacementTargetId == 0)
            {
                self.ClearPlacementData();
                self.PlacementTargetId = targetId;
                self.PlacementDragOffset = new TSVector2(FP.MaxValue, FP.MaxValue);
                EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementDragStart() { Id = self.LSOwner().Id, TargetId = targetId });
            }
        }

        private static void RunCommandPlacementDrag(this LSCommandsRunComponent self, TSVector2 position)
        {
            if (self.PlacementType == EUnitType.None && self.PlacementTargetId == 0)
                return;

            if (self.PlacementTargetId > 0 && FP.MaxValue.Equals(self.PlacementDragOffset.y)) {
                LSUnitComponent lsUnitComponent = self.LSWorld().GetComponent<LSUnitComponent>();
                TransformComponent transformComponent = lsUnitComponent.GetChild<LSUnit>(self.PlacementTargetId)?.GetComponent<TransformComponent>();
                self.PlacementDragOffset = transformComponent != null
                        ? new TSVector2(transformComponent.Position.x - position.x, transformComponent.Position.z - position.y)
                        : new TSVector2(0, 0);
            }
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementDrag() { Id = self.LSOwner().Id, Position = position });
        }

        private static void RunCommandPlacementDragEnd(this LSCommandsRunComponent self, TSVector2 position)
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
                    lsUnit = LSUnitFactory.CreateBuilding(lsWorld, self.PlacementTableId, team);
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

        private static void RunCommandPlacementStart(this LSCommandsRunComponent self, EUnitType type, int tableId, int level)
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

        private static void RunCommandPlacementRotate(this LSCommandsRunComponent self, int rotation)
        {
            self.PlacementRotation += rotation;
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementRotate() { Id = self.LSOwner().Id, Rotation = rotation });
        }

        private static void RunCommandPlacementCancel(this LSCommandsRunComponent self)
        {
            self.ClearPlacementData();
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementCancel() { Id = self.LSOwner().Id });
        }
        
        private static void ClearPlacementData(this LSCommandsRunComponent self)
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