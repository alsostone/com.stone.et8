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
            LSUnit lsOwner = self.LSOwner();
            TeamType teamPlacer = lsOwner.GetComponent<TeamComponent>().Type;
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
                            self.LSWorld().GetComponent<LSGridBuilderComponent>().RunCommandPlacementDragStart(teamPlacer, targetId);
                            break;
                        }
                    case OperateCommandType.PlacementDrag:
                        {
                            TSVector2 pos = LSCommand.ParseCommandFloat24x2(command);
                            self.LSWorld().GetComponent<LSGridBuilderComponent>().RunCommandPlacementDrag(teamPlacer, pos);
                            break;
                        }
                    case OperateCommandType.PlacementDragEnd:
                        {
                            TSVector2 pos = LSCommand.ParseCommandFloat24x2(command);
                            self.LSWorld().GetComponent<LSGridBuilderComponent>().RunCommandPlacementDragEnd(teamPlacer, pos);
                            break;
                        }
                    case OperateCommandType.PlacementStart:
                        {
                            ulong param = LSCommand.ParseCommandLong48(command);
                            EUnitType type = (EUnitType)((param >> 32) & 0xFFFF);
                            int tableId = (int)(param & 0xFFFFFFFF);
                            self.LSWorld().GetComponent<LSGridBuilderComponent>().RunCommandPlacementStart(teamPlacer, type, tableId);
                            break;
                        }
                    case OperateCommandType.Button:
                        {
                            self.RunCommandButton(lsOwner, command);
                            break;
                        }
#if ENABLE_DEBUG
                    case OperateCommandType.Gm:
                        {
                            self.RunCommandGm(lsOwner, command);
                            break;
                        }
#endif
                }
            }
            self.Commands.Clear();

            TransformComponent transformComponent = lsOwner.GetComponent<TransformComponent>();
            transformComponent.Move(self.MoveAxis);
        }

        private static void RunCommandButton(this LSCommandsRunComponent self, LSUnit lsOwner, ulong command)
        {
            TeamType teamPlacer = lsOwner.GetComponent<TeamComponent>().Type;
            (CommandButtonType, long) button = LSCommand.ParseCommandButton(command);
            switch (button.Item1)
            {
                case CommandButtonType.PlacementRotate:
                    self.LSWorld().GetComponent<LSGridBuilderComponent>().RunCommandPlacementRotate(teamPlacer, (int)button.Item2);
                    break;
                case CommandButtonType.PlacementCancel:
                    self.LSWorld().GetComponent<LSGridBuilderComponent>().RunCommandPlacementCancel(teamPlacer);
                    break;
                case CommandButtonType.Attack:
                    lsOwner.GetComponent<SkillComponent>().TryCastSkill(ESkillType.Normal);
                    break;
                case CommandButtonType.Skill1:
                    lsOwner.GetComponent<SkillComponent>().TryCastSkill(ESkillType.Active, 0);
                    break;
                case CommandButtonType.Skill2:
                    lsOwner.GetComponent<SkillComponent>().TryCastSkill(ESkillType.Active, 1);
                    break;
                case CommandButtonType.Jump:
                    {
                        //unit.GetComponent<LSJumpComponent>().Jump();
                        break;
                    }
            }
        }
        
#if ENABLE_DEBUG
        private static void RunCommandGm(this LSCommandsRunComponent self, LSUnit lsOwner, ulong command)
        {
            (CommandGMType, long) gm = LSCommand.ParseCommandGm(command);
            switch (gm.Item1)
            {
                case CommandGMType.Victory:
                {
                    TeamType team = lsOwner.GetComponent<TeamComponent>().GetFriendTeam();
                    lsOwner.LSWorld().GetComponent<LSGameOverComponent>().SetGameOver(team);
                    break;
                }
                case CommandGMType.Failure:
                {
                    TeamType team = lsOwner.GetComponent<TeamComponent>().GetEnemyTeam();
                    lsOwner.LSWorld().GetComponent<LSGameOverComponent>().SetGameOver(team);
                    break;
                }
            }
        }
#endif
    }
}