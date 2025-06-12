using MemoryPack;
using NPBehave;

namespace ET
{
    [AINode]
    [MemoryPackable]
    public partial class ActionAttack : ActionRequest
    {
        protected override Result OnAction(Request request)
        {
            LSUnit lsUnit = Agent as LSUnit;
            SkillComponent skillComponent = lsUnit.GetComponent<SkillComponent>();
            if (skillComponent.TryCastSkill(ESkillType.Normal))
                return Result.SUCCESS;
            return Result.FAILED;
        }
    }
    
    [AINode]
    [MemoryPackable]
    public partial class ActionSkill : ActionRequest
    {
        protected override Result OnAction(Request request)
        {
            LSUnit lsUnit = Agent as LSUnit;
            SkillComponent skillComponent = lsUnit.GetComponent<SkillComponent>();
            if (skillComponent.TryCastSkill(ESkillType.Active))
                return Result.SUCCESS;
            return Result.FAILED;
        }
    }
}