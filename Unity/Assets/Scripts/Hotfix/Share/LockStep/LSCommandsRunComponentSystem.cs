using System;
using ET.Client;
using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(LSCommandsRunComponent))]
    [LSEntitySystemOf(typeof(LSCommandsRunComponent))]
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
                switch (LSUtils.ParseCommandType(command))
                {
                    case OperateCommandType.Move: {
                        self.MoveAxis = LSUtils.ParseCommandFloat24x2(command).normalized;
                        break;
                    }
                    case OperateCommandType.PlacementDragStart: {
                        long targetId = LSUtils.ParseCommandLong48(command);
                        self.RunCommandPlacementDragStart(targetId);
                        break;
                    }
                    case OperateCommandType.PlacementDrag: {
                        TSVector2 pos = LSUtils.ParseCommandFloat24x2(command);
                        self.RunCommandPlacementDrag(pos);
                        break;
                    }
                    case OperateCommandType.PlacementDragEnd: {
                        TSVector2 pos = LSUtils.ParseCommandFloat24x2(command);
                        self.RunCommandPlacementDragEnd(pos);
                        break;
                    }
                    case OperateCommandType.Button: {
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
            (CommandButtonType, int) button = LSUtils.ParseCommandButton(command);
            switch (button.Item1)
            {
                case CommandButtonType.PlacementStart:
                    self.RunCommandPlacementStart(button.Item2);
                    break;
                case CommandButtonType.PlacementRotate:
                    self.RunCommandPlacementRotate(button.Item2);
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
                case CommandButtonType.Jump: {
                    //unit.GetComponent<LSJumpComponent>().Jump();
                    break;
                }
            }
        }

        private static void RunCommandPlacementDragStart(this LSCommandsRunComponent self, long targetId)
        {
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementDragStart() {Id = self.LSOwner().Id, TargetId = targetId});
        }

        private static void RunCommandPlacementDrag(this LSCommandsRunComponent self, TSVector2 position)
        {
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementDrag() {Id = self.LSOwner().Id, Position = position});
        }

        private static void RunCommandPlacementDragEnd(this LSCommandsRunComponent self, TSVector2 position)
        {
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementDragEnd() {Id = self.LSOwner().Id, Position = position});
        }

        private static void RunCommandPlacementStart(this LSCommandsRunComponent self, int tableId)
        {
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementStart() {Id = self.LSOwner().Id, TableId = tableId });
        }
        
        private static void RunCommandPlacementRotate(this LSCommandsRunComponent self, int rotation)
        {
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementRotate() {Id = self.LSOwner().Id, Rotation = rotation });
        }
        
        private static void RunCommandPlacementCancel(this LSCommandsRunComponent self)
        {
            EventSystem.Instance.Publish(self.LSWorld(), new LSPlacementCancel() {Id = self.LSOwner().Id});
        }
    }
}