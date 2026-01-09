using TrueSync;

namespace ET
{
    [LSEntitySystemOf(typeof(ProductComponent))]
    [EntitySystemOf(typeof(ProductComponent))]
    [FriendOf(typeof(ProductComponent))]
    [FriendOf(typeof(SkillComponent))]
    public static partial class ProductComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ProductComponent self, int productSkillId)
        {self.LSRoom()?.ProcessLog.LogFunction(101, self.LSParent().Id, productSkillId);
            SkillComponent skillComponent = self.LSOwner().GetComponent<SkillComponent>();
            Skill skill = skillComponent.AddSkill(productSkillId);

            self.ProductSkillId = productSkillId;
            self.StartTime = self.LSWorld().ElapsedTime + skill.TbSkillRow.FirstCdTime * FP.EN3;
            self.IntervalTime = skill.TbSkillRow.CdTime * FP.EN3;
        }

        [LSEntitySystem]
        private static void LSUpdate(this ProductComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(100, self.LSParent().Id);
            if (!self.LSOwner().GetComponent<FlagComponent>().HasRestrict(FlagRestrict.NotProduct))
            {
                if (self.LSWorld().ElapsedTime >= self.StartTime)
                {
                    SkillComponent skillComponent = self.LSOwner().GetComponent<SkillComponent>();
                    skillComponent.TryCastSkill(self.ProductSkillId);
                    self.StartTime = self.LSWorld().ElapsedTime + self.IntervalTime;
                }
            }
        }
        
    }
}