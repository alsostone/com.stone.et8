using System.Collections.Generic;
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
            self.Commands = new List<LSCommandData>();
        }

        [LSEntitySystem]
        private static void LSUpdate(this LSCommandsRunComponent self)
        {
            foreach (var command in self.Commands)
            {
                switch (LSCommand.ParseCommandType(command))
                {
                    case OperateCommandType.Escape:
                    {
                        self.LSOwner().GetComponent<LSGridBuilderComponent>().RunCommandEscape();
                        self.LSOwner().GetComponent<SelectionComponent>().ClearSelection();
                        break;
                    }
                    case OperateCommandType.Move:
                    {
                        self.IsRightDownMove = false;
                        self.GetPlayerBindHeroComponent<MovePathFindingComponent>()?.Stop();
                        self.GetPlayerBindHeroComponent<MoveFlowFieldComponent>()?.Stop();
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
                    case OperateCommandType.TouchDownTarget:
                    {
                        long targetId = LSCommand.ParseCommandLong(command);
                        self.LSOwner().GetComponent<LSGridBuilderComponent>().RunCommandTouchDown(targetId);
                        break;
                    }
                    case OperateCommandType.TouchDragStart:
                    {
                        TSVector2 pos = LSCommand.ParseCommandFloat2(command);
                        switch (self.LSWorld().OperationMode)
                        {
                            case OperationMode.Dragging:
                                self.LSOwner().GetComponent<LSGridBuilderComponent>().RunCommandTouchDragStart(pos);
                                break;
                            case OperationMode.Shooting:
                            {
                                TSVector targetPosition = LSCommand.ParseCommandFloat2(command).ToXZ();
                                TransformComponent transformComponent = self.GetPlayerBindCampComponent<TransformComponent>();
                                TSVector position = transformComponent.GetAttachPoint(AttachPoint.Head);
                                //LSUnitFactory.CreateBulletToDirection2(self.LSWorld(), 30000002, position, transformComponent.LSOwner(), targetPosition);
                                LSUnitFactory.CreateBulletToPosition(self.LSWorld(), 30000003, position, transformComponent.LSOwner(), targetPosition);
                                break;
                            }
                        }
                        break;
                    }
                    case OperateCommandType.TouchDrag:
                    {
                        TSVector2 pos = LSCommand.ParseCommandFloat2(command);
                        switch (self.LSWorld().OperationMode)
                        {
                            case OperationMode.Dragging:
                                self.LSOwner().GetComponent<LSGridBuilderComponent>().RunCommandTouchDrag(pos);
                                break;
                        }
                        break;
                    }
                    case OperateCommandType.TouchDragEnd:
                    {
                        TSVector2 pos = LSCommand.ParseCommandFloat2(command);
                        switch (self.LSWorld().OperationMode)
                        {
                            case OperationMode.Dragging:
                                self.LSOwner().GetComponent<LSGridBuilderComponent>().RunCommandTouchDragEnd(pos);
                                break;
                        }
                        break;
                    }
                    case OperateCommandType.TouchDragCancel:
                    {
                        self.LSOwner().GetComponent<LSGridBuilderComponent>().RunCommandTouchDragCancel();
                        break;
                    }
                    case OperateCommandType.PlacementDrag:
                    {
                        long targetId = LSCommand.ParseCommandLong(command);
                        self.LSOwner().GetComponent<LSGridBuilderComponent>().RunCommandPlacementDrag(targetId);
                        break;
                    }
                    case OperateCommandType.PlacementNew:
                    {
                        long itemId = LSCommand.ParseCommandLong(command);
                        self.LSOwner().GetComponent<LSGridBuilderComponent>().RunCommandPlacementNew(itemId);
                        break;
                    }
                    case OperateCommandType.Button:
                    {
                        self.RunCommandButton(command);
                        break;
                    }
                    case OperateCommandType.TimeScale:
                    {
                        self.RunCommandTimeScale(command);
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
                self.GetPlayerBindHeroComponent<TransformComponent>()?.RVOMove(self.MoveAxis);
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
                case CommandButtonType.CardSelect:
                    self.LSOwner().GetComponent<CardSelectComponent>().TrySelectCard((int)button.Item2);
                    break;
                case CommandButtonType.OpreationMode:
                    self.LSWorld().OperationMode = (OperationMode)button.Item2;
                    EventSystem.Instance.Publish(self.LSWorld(), new LSOprationModeChanged() { Mode = (OperationMode)button.Item2 });
                    break;
                case CommandButtonType.Attack:
                    self.GetPlayerBindHeroComponent<SkillComponent>()?.TryCastSkill(ESkillType.Normal);
                    break;
                case CommandButtonType.Skill:
                    self.GetPlayerBindHeroComponent<SkillComponent>()?.TryCastSkill(ESkillType.Active, (int)button.Item2);
                    break;
                default: break;
            }
        }
        
        private static void RunCommandTimeScale(this LSCommandsRunComponent self, LSCommandData command)
        {
            long scale = LSCommand.ParseCommandLong(command);
            self.LSWorld().TimeScale = FP.FromRaw(scale);
            EventSystem.Instance.Publish(self.LSWorld(), new LSTimeScaleChanged());
        }
        
#if ENABLE_DEBUG
        private static void RunCommandGm(this LSCommandsRunComponent self, LSCommandData command)
        {
            (CommandGMType, long) gm = LSCommand.ParseCommandGm(command);
            switch (gm.Item1)
            {
                case CommandGMType.Victory:
                {
                    TeamType team = self.LSOwner().GetComponent<TeamComponent>().GetOwnerTeam();
                    self.LSWorld().GetComponent<LSGameOverComponent>().SetGameOver(team);
                    break;
                }
                case CommandGMType.Failure:
                {
                    TeamType team = self.LSOwner().GetComponent<TeamComponent>().GetOppositeTeam();
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
        
        private static T GetPlayerBindCampComponent<T>(this LSCommandsRunComponent self) where T : LSEntity
        {
            PlayerComponent lsPlayerComponent = self.LSOwner().GetComponent<PlayerComponent>();
            if (lsPlayerComponent.BindCampId == 0)
                return null;
            
            LSUnitComponent unitComponent = self.LSWorld().GetComponent<LSUnitComponent>();
            LSUnit lsUnit = unitComponent.GetChild<LSUnit>(lsPlayerComponent.BindCampId);
            return lsUnit?.GetComponent<T>();
        }

        private static T GetPlayerBindHeroComponent<T>(this LSCommandsRunComponent self) where T : LSEntity
        {
            PlayerComponent lsPlayerComponent = self.LSOwner().GetComponent<PlayerComponent>();
            if (lsPlayerComponent.BindHeroId == 0)
                return null;
            
            LSUnitComponent unitComponent = self.LSWorld().GetComponent<LSUnitComponent>();
            LSUnit lsUnit = unitComponent.GetChild<LSUnit>(lsPlayerComponent.BindHeroId);
            return lsUnit?.GetComponent<T>();
        }
    }
}