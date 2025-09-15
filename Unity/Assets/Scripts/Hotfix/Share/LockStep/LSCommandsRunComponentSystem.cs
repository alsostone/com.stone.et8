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
            LSUnit lsPlayer = self.LSOwner();
            TeamType teamPlacer = lsPlayer.GetComponent<TeamComponent>().Type;
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
                            self.RunCommandButton(lsPlayer, command);
                            break;
                        }
#if ENABLE_DEBUG
                    case OperateCommandType.Gm:
                        {
                            self.RunCommandGm(lsPlayer, command);
                            break;
                        }
#endif
                }
            }
            self.Commands.Clear();
            self.GetBindUnitComponent<TransformComponent>(lsPlayer)?.Move(self.MoveAxis);
        }

        private static void RunCommandButton(this LSCommandsRunComponent self, LSUnit lsPlayer, ulong command)
        {
            TeamType teamPlacer = lsPlayer.GetComponent<TeamComponent>().Type;
            (CommandButtonType, long) button = LSCommand.ParseCommandButton(command);
            switch (button.Item1)
            {
                case CommandButtonType.PlacementRotate:
                    self.LSWorld().GetComponent<LSGridBuilderComponent>().RunCommandPlacementRotate(teamPlacer, (int)button.Item2);
                    break;
                case CommandButtonType.PlacementCancel:
                    self.LSWorld().GetComponent<LSGridBuilderComponent>().RunCommandPlacementCancel(teamPlacer);
                    break;
                case CommandButtonType.CardSelect:
                    lsPlayer.GetComponent<CardSelectComponent>().TrySelectCard((int)button.Item2);
                    break;
                case CommandButtonType.Attack:
                    self.GetBindUnitComponent<SkillComponent>(lsPlayer)?.TryCastSkill(ESkillType.Normal);
                    break;
                case CommandButtonType.Skill1:
                    self.GetBindUnitComponent<SkillComponent>(lsPlayer)?.TryCastSkill(ESkillType.Active, 0);
                    break;
                case CommandButtonType.Skill2:
                    self.GetBindUnitComponent<SkillComponent>(lsPlayer)?.TryCastSkill(ESkillType.Active, 1);
                    break;
                default: break;
            }
        }
        
#if ENABLE_DEBUG
        private static void RunCommandGm(this LSCommandsRunComponent self, LSUnit lsPlayer, ulong command)
        {
            (CommandGMType, long) gm = LSCommand.ParseCommandGm(command);
            switch (gm.Item1)
            {
                case CommandGMType.Victory:
                {
                    TeamType team = lsPlayer.GetComponent<TeamComponent>().GetFriendTeam();
                    self.LSWorld().GetComponent<LSGameOverComponent>().SetGameOver(team);
                    break;
                }
                case CommandGMType.Failure:
                {
                    TeamType team = lsPlayer.GetComponent<TeamComponent>().GetEnemyTeam();
                    self.LSWorld().GetComponent<LSGameOverComponent>().SetGameOver(team);
                    break;
                }
            }
        }
#endif
        
        private static T GetBindUnitComponent<T>(this LSCommandsRunComponent self, LSUnit player) where T : LSEntity
        {
            PlayerComponent lsPlayerComponent = player.GetComponent<PlayerComponent>();
            if (lsPlayerComponent.BindEntityId == 0)
                return null;
            
            LSUnitComponent unitComponent = self.LSWorld().GetComponent<LSUnitComponent>();
            LSUnit lsUnit = unitComponent.GetChild<LSUnit>(lsPlayerComponent.BindEntityId);
            return lsUnit?.GetComponent<T>();
        }

    }
}