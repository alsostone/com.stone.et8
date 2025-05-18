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
            PropComponent numericComponent = unit.GetComponent<PropComponent>();
            TSVector2 v2 = self.LSInput.V * numericComponent.Get(NumericType.Speed) * LSConstValue.UpdateInterval / LSConstValue.Milliseconds;
            if (v2.LengthSquared() > FP.EN4)
            {
                TSVector oldPos = unit.Position;
                unit.Position += new TSVector(v2.x, 0, v2.y);
                unit.Forward = unit.Position - oldPos;
            }
            
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
                unit.GetComponent<SkillComponent>().TryCastSkill(ESkillType.Active);
            }

        }
    }
}