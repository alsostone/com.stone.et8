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
        {
            SkillComponent skillComponent = self.LSOwner().GetComponent<SkillComponent>();
            Skill skill = skillComponent.AddSkill(productSkillId);

            self.ProductSkillId = productSkillId;
            self.ProductFrame = self.LSWorld().Frame + skill.TbSkillRow.FirstCdTime.Convert2Frame();
            self.IntervalFrame = skill.TbSkillRow.CdTime.Convert2Frame();
        }

        [LSEntitySystem]
        private static void LSUpdate(this ProductComponent self)
        {
            if (self.LSOwner().EnabledRef >= 0)
            {
                if (self.LSWorld().Frame >= self.ProductFrame)
                {
                    SkillComponent skillComponent = self.LSOwner().GetComponent<SkillComponent>();
                    skillComponent.TryCastSkill(self.ProductSkillId);
                    self.ProductFrame = self.LSWorld().Frame + self.IntervalFrame;
                }
            }
        }
        
    }
}