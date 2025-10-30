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
            foreach (var command in self.Commands)
            {
                switch (LSCommand.ParseCommandType(command))
                {
                    case OperateCommandType.Move:
                    {
                        self.IsRightDownMove = false;
                        self.GetBindUnitComponent<MovePathFindingComponent>()?.Stop();
                        self.MoveAxis = LSCommand.ParseCommandFloat2(command);
                        break;
                    }
                    case OperateCommandType.MoveTo:
                    {
                        self.IsRightDownMove = true;
                        (MovementMode, TSVector2) movement = LSCommand.ParseCommandMoveTo(command);
                        self.LSOwner().GetComponent<SelectionComponent>().MoveToPosition(movement.Item2, movement.Item1);
                        break;
                    }
                    case OperateCommandType.TouchDragStart:
                    {
                        TSVector2 pos = LSCommand.ParseCommandFloat2(command);
                        self.LSOwner().GetComponent<LSGridBuilderComponent>().RunCommandTouchDragStart(pos);
                        break;
                    }
                    case OperateCommandType.TouchDrag:
                    {
                        TSVector2 pos = LSCommand.ParseCommandFloat2(command);
                        self.LSOwner().GetComponent<LSGridBuilderComponent>().RunCommandTouchDrag(pos);
                        break;
                    }
                    case OperateCommandType.TouchDragEnd:
                    {
                        TSVector2 pos = LSCommand.ParseCommandFloat2(command);
                        self.LSOwner().GetComponent<LSGridBuilderComponent>().RunCommandTouchDragEnd(pos);
                        break;
                    }
                    case OperateCommandType.PlacementDragStart:
                    {
                        long targetId = (long)LSCommand.ParseCommandLong(command);
                        self.LSOwner().GetComponent<LSGridBuilderComponent>().RunCommandPlacementDragStart(targetId);
                        break;
                    }
                    case OperateCommandType.PlacementStart:
                    {
                        long itemId = (long)LSCommand.ParseCommandLong(command);
                        self.LSOwner().GetComponent<LSGridBuilderComponent>().RunCommandPlacementStart(itemId);
                        break;
                    }
                    case OperateCommandType.Button:
                    {
                        self.RunCommandButton(command);
                        break;
                    }
#if ENABLE_DEBUG
                    case OperateCommandType.Gm:
                    {
                        self.RunCommandGm(command);
                        break;
                    }
#endif
                }
            }
            self.Commands.Clear();
            
            // 非右键点击移动时执行普通移动
            if (!self.IsRightDownMove) {
                self.GetBindUnitComponent<TransformComponent>()?.Move(self.MoveAxis);
            }
        }

        private static void RunCommandButton(this LSCommandsRunComponent self, LSCommandData command)
        {
            (CommandButtonType, long) button = LSCommand.ParseCommandButton(command);
            switch (button.Item1)
            {
                case CommandButtonType.PlacementRotate:
                    self.LSOwner().GetComponent<LSGridBuilderComponent>().RunCommandPlacementRotate((int)button.Item2);
                    break;
                case CommandButtonType.PlacementCancel:
                    self.LSOwner().GetComponent<LSGridBuilderComponent>().RunCommandPlacementCancel();
                    break;
                case CommandButtonType.CardSelect:
                    self.LSOwner().GetComponent<CardSelectComponent>().TrySelectCard((int)button.Item2);
                    break;
                case CommandButtonType.Attack:
                    self.GetBindUnitComponent<SkillComponent>()?.TryCastSkill(ESkillType.Normal);
                    break;
                case CommandButtonType.Skill1:
                    self.GetBindUnitComponent<SkillComponent>()?.TryCastSkill(ESkillType.Active, 0);
                    break;
                case CommandButtonType.Skill2:
                    self.GetBindUnitComponent<SkillComponent>()?.TryCastSkill(ESkillType.Active, 1);
                    break;
                default: break;
            }
        }
        
#if ENABLE_DEBUG
        private static void RunCommandGm(this LSCommandsRunComponent self, LSCommandData command)
        {
            (CommandGMType, long) gm = LSCommand.ParseCommandGm(command);
            switch (gm.Item1)
            {
                case CommandGMType.Victory:
                {
                    TeamType team = self.LSOwner().GetComponent<TeamComponent>().GetFriendTeam();
                    self.LSWorld().GetComponent<LSGameOverComponent>().SetGameOver(team);
                    break;
                }
                case CommandGMType.Failure:
                {
                    TeamType team = self.LSOwner().GetComponent<TeamComponent>().GetEnemyTeam();
                    self.LSWorld().GetComponent<LSGameOverComponent>().SetGameOver(team);
                    break;
                }
                case CommandGMType.CardSelect:
                {
                    CardSelectComponent selectComponent = self.LSOwner().GetComponent<CardSelectComponent>();
                    selectComponent.RandomCards((int)gm.Item2);
                    break;
                }
            }
        }
#endif
        
        private static T GetBindUnitComponent<T>(this LSCommandsRunComponent self) where T : LSEntity
        {
            PlayerComponent lsPlayerComponent = self.LSOwner().GetComponent<PlayerComponent>();
            if (lsPlayerComponent.BindEntityId == 0)
                return null;
            
            LSUnitComponent unitComponent = self.LSWorld().GetComponent<LSUnitComponent>();
            LSUnit lsUnit = unitComponent.GetChild<LSUnit>(lsPlayerComponent.BindEntityId);
            return lsUnit?.GetComponent<T>();
        }

    }
}