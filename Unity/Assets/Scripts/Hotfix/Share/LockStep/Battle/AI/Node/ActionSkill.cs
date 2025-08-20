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
            
            switch (request)
            {
                case Request.START:
                {
                    if (skillComponent.TryCastSkill(ESkillType.Normal))
                        return Result.PROGRESS;
                    break;
                }
                case Request.UPDATE:
                {
                    Skill skill = skillComponent.GetRunningSkill(ESkillType.Normal);
                    if (skill != null && skill.IsRunning)
                        return skill.IsRunning ? Result.PROGRESS : Result.SUCCESS;
                    break;
                }
                case Request.CANCEL:
                {
                    Skill skill = skillComponent.GetRunningSkill(ESkillType.Normal);
                    skill?.ForceDone();
                    break;
                }
            }
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

            switch (request)
            {
                case Request.START:
                {
                    if (skillComponent.TryCastSkill(ESkillType.Active))
                        return Result.PROGRESS;
                    break;
                }
                case Request.UPDATE:
                {
                    Skill skill = skillComponent.GetRunningSkill(ESkillType.Active);
                    if (skill != null && skill.IsRunning)
                        return skill.IsRunning ? Result.PROGRESS : Result.SUCCESS;
                    break;
                }
                case Request.CANCEL:
                {
                    Skill skill = skillComponent.GetRunningSkill(ESkillType.Active);
                    skill?.ForceDone();
                    break;
                }
            }
            return Result.FAILED;
        }
    }
}