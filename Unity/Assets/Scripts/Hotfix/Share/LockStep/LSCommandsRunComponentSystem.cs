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
                            self.MoveAxis = LSCommand.ParseCommandFloat24x2(command);
                            break;
                        }
                    case OperateCommandType.PlacementDragStart:
                        {
                            long targetId = (long)LSCommand.ParseCommandLong48(command);
                            unit.GetComponent<LSGridBuilderComponent>().RunCommandPlacementDragStart(targetId);
                            break;
                        }
                    case OperateCommandType.PlacementDrag:
                        {
                            TSVector2 pos = LSCommand.ParseCommandFloat24x2(command);
                            unit.GetComponent<LSGridBuilderComponent>().RunCommandPlacementDrag(pos);
                            break;
                        }
                    case OperateCommandType.PlacementDragEnd:
                        {
                            TSVector2 pos = LSCommand.ParseCommandFloat24x2(command);
                            unit.GetComponent<LSGridBuilderComponent>().RunCommandPlacementDragEnd(pos);
                            break;
                        }
                    case OperateCommandType.PlacementStart:
                        {
                            ulong param = LSCommand.ParseCommandLong48(command);
                            EUnitType type = (EUnitType)((param >> 32) & 0xFFFF);
                            int tableId = (int)(param & 0xFFFFFFFF);
                            unit.GetComponent<LSGridBuilderComponent>().RunCommandPlacementStart(type, tableId);
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
                    unit.GetComponent<LSGridBuilderComponent>().RunCommandPlacementRotate((int)button.Item2);
                    break;
                case CommandButtonType.PlacementCancel:
                    unit.GetComponent<LSGridBuilderComponent>().RunCommandPlacementCancel();
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

    }
}