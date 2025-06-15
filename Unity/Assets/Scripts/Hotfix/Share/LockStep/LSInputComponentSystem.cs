using System;
using ET.Client;
using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(LSInputComponent))]
    [LSEntitySystemOf(typeof(LSInputComponent))]
    public static partial class LSInputComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSInputComponent self)
        {

        }
        
        [LSEntitySystem]
        private static void LSUpdate(this LSInputComponent self)
        {
            LSUnit unit = self.LSOwner();
            TransformComponent transformComponent = unit.GetComponent<TransformComponent>();
            transformComponent.Move(self.LSInput.V);

            if ((self.LSInput.Button & LSConstButtonValue.Jump) > 0)
            {
                //unit.GetComponent<LSJumpComponent>().Jump();
            }
            else if ((self.LSInput.Button & LSConstButtonValue.Attack) > 0)
            {
                unit.GetComponent<SkillComponent>().TryCastSkill(ESkillType.Normal);
            }
            else if ((self.LSInput.Button & LSConstButtonValue.Skill1) > 0)
            {
                unit.GetComponent<SkillComponent>().TryCastSkill(ESkillType.Active, 0);
            }
            else if ((self.LSInput.Button & LSConstButtonValue.Skill2) > 0)
            {
                unit.GetComponent<SkillComponent>().TryCastSkill(ESkillType.Active, 1);
            }

        }
    }
}