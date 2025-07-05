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
                    case OperateCommandType.Move:
                    {
                        self.MoveAxis = LSUtils.ParseCommandFloat24x2(command).normalized;
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
            (CommandButtonType, int) button = LSUtils.ParseCommandButton(command);
            switch (button.Item1)
            {
                case CommandButtonType.Attack:
                {
                    unit.GetComponent<SkillComponent>().TryCastSkill(ESkillType.Normal);
                    break;
                }
                case CommandButtonType.Skill1:
                {
                    unit.GetComponent<SkillComponent>().TryCastSkill(ESkillType.Active, 0);
                    break;
                }
                case CommandButtonType.Skill2:
                {
                    unit.GetComponent<SkillComponent>().TryCastSkill(ESkillType.Active, 1);
                    break;
                }
                case CommandButtonType.Jump:
                {
                    //unit.GetComponent<LSJumpComponent>().Jump();
                    break;
                }
            }
        }
    }
}