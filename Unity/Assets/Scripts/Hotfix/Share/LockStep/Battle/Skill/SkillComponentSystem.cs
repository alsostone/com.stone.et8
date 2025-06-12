using System.Collections.Generic;

namespace ET
{
    [EntitySystemOf(typeof(SkillComponent))]
    [FriendOf(typeof(SkillComponent))]
    [FriendOf(typeof(Skill))]
    public static partial class SkillComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SkillComponent self, int[] skillIds)
        {self.LSRoom()?.ProcessLog.LogFunction(54, self.LSParent().Id);
            foreach (int skillId in skillIds)
            {
                self.AddSkill(skillId);
            }
        }

        [EntitySystem]
        private static void Destroy(this SkillComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(53, self.LSParent().Id);
            self.IdSkillMap.Clear();
            self.TypeSkillsMap.Clear();
        }

        [EntitySystem]
        private static void LSUpdate(this SkillComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(52, self.LSParent().Id);
            self.TryCastSkill(ESkillType.Normal);
            for (int index = self.mRunningSkills.Count - 1; index >= 0; index--)
            {
                long id = self.mRunningSkills[index];
                Skill skill = self.GetChild<Skill>(id);
                if (skill == null)
                {
                    self.mRunningSkills.RemoveAt(index);
                }
                else
                {
                    skill.StepRunning();
                    if (!skill.IsRunning)
                    {
                        self.mRunningSkills.RemoveAt(index);
                    }
                }
            }
        }

        public static bool HasRunningSkill(this SkillComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(51, self.LSParent().Id);
            return self.mRunningSkills.Count > 0;
        }

        public static Skill AddSkill(this SkillComponent self, int skillId, bool isOnlyOnce = false)
        {self.LSRoom()?.ProcessLog.LogFunction(50, self.LSParent().Id, skillId, isOnlyOnce ? 1 : 0);
            if (!self.IdSkillMap.TryGetValue(skillId, out long eid))
            {
                Skill skill = self.AddChild<Skill, int, bool>(skillId, isOnlyOnce);
                self.IdSkillMap.Add(skillId, skill.Id);

                TbSkillRow tbSkillRow = TbSkill.Instance.Get(skillId);
                if (!self.TypeSkillsMap.TryGetValue(tbSkillRow.SkillType, out List<long> skills))
                {
                    skills = new List<long>();
                    self.TypeSkillsMap[tbSkillRow.SkillType] = skills;
                }
                skills.Add(skill.Id);
                return skill;
            }
            return self.GetChild<Skill>(eid);
        }

        public static void ForceAllDone(this SkillComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(49, self.LSParent().Id);
            // 技能移除不会维护该数组 获取不到技能也不会有问题
            for (int index = self.mRunningSkills.Count - 1; index >= 0; index--)
            {
                long id = self.mRunningSkills[index];
                Skill skill = self.GetChild<Skill>(id);
                skill?.ForceDone();
            }
            self.mRunningSkills.Clear();
        }

        public static bool TryCastSkill(this SkillComponent self, ESkillType type)
        {self.LSRoom()?.ProcessLog.LogFunction(48, self.LSParent().Id);
            if (self.LSOwner().DeadMark) { return false; }
            // if (!Enable) { return; }
            if (self.CheckRestrict(type)) { return false; }
            
            if (self.TypeSkillsMap.TryGetValue(type, out List<long> skillIds))
            {
                for (int i = skillIds.Count - 1; i >= 0; i--)
                {
                    Skill skill = self.GetChild<Skill>(skillIds[i]);
                    if (skill.TryCast())
                    {
                        if (skill.IsRunning)
                            self.mRunningSkills.Add(skill.Id);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool TryCastSkill(this SkillComponent self, ESkillType type, int index)
        {self.LSRoom()?.ProcessLog.LogFunction(47, self.LSParent().Id, index);
            if (self.LSOwner().DeadMark) { return false; }
            // if (!Enable) { return false; }
            if (self.CheckRestrict(type)) { return false; }

            if (self.TypeSkillsMap.TryGetValue(type, out List<long> skillIds))
            {
                if (skillIds.Count > index)
                {
                    Skill skill = self.GetChild<Skill>(skillIds[index]);
                    if (skill.TryCast())
                    {
                        if (skill.IsRunning)
                            self.mRunningSkills.Add(skill.Id);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool CastAttachSkill(this SkillComponent self, int id)
        {self.LSRoom()?.ProcessLog.LogFunction(46, self.LSParent().Id, id);
            if (self.LSOwner().DeadMark) { return false; }
            // if (!Enable) { return false; }
            if (self.CheckRestrict(ESkillType.Active)) { return false; }

            Skill skill = self.AddSkill(id, true);
            if (skill == null) { return false; }
            if (skill.TryCast())
            {
                if (skill.IsRunning)
                    self.mRunningSkills.Add(skill.Id);
                return true;
            }
            skill.Dispose();
            return false;
        }

        private static bool CheckRestrict(this SkillComponent self, ESkillType type)
        {self.LSRoom()?.ProcessLog.LogFunction(45, self.LSParent().Id);
            var flagComponent = self.LSOwner().GetComponent<FlagComponent>();
            switch (type)
            {
                case ESkillType.Normal:
                    return flagComponent.HasRestrict(FlagRestrict.NotAttack);
                default:
                    return flagComponent.HasRestrict(FlagRestrict.NotSkill);
            }
        }

    }
}