using MemoryPack;
using NPBehave;

namespace ET
{
    [AINode]
    [MemoryPackable]
    public partial class RequestAttack : ActionRequest
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
}