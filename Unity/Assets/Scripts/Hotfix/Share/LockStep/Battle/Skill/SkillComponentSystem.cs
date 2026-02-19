using System.Collections.Generic;

namespace ET
{
    [EntitySystemOf(typeof(SkillComponent))]
    [FriendOf(typeof(SkillComponent))]
    [FriendOf(typeof(Skill))]
    public static partial class SkillComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SkillComponent self, int[] normalIds, int[] activeIds)
        {self.LSRoom()?.ProcessLog.LogFunction(125, self.LSParent().Id);
            self.IdSkillMap = new Dictionary<long, long>();
            self.TypeSkillsMap = new Dictionary<ESkillType, List<long>>();
            self.RunningSkills = new List<long>();
            if (normalIds != null) {
                foreach (int skillId in normalIds) {
                    self.AddSkill(skillId);
                }
            }
            if (activeIds != null) {
                foreach (int skillId in activeIds) {
                    self.AddSkill(skillId);
                }
            }
        }

        [EntitySystem]
        private static void Destroy(this SkillComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(124, self.LSParent().Id);
            self.IdSkillMap.Clear();
            self.TypeSkillsMap.Clear();
        }

        [EntitySystem]
        private static void LSUpdate(this SkillComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(123, self.LSParent().Id);
            if (self.RemovedSkills != null)
            {
                for (int i = 0; i < self.RemovedSkills.Count; i++)
                {
                    long instanceId = self.RemovedSkills[i];
                    Skill skill = self.GetChild<Skill>(instanceId);
                    if (skill == null) { continue; }
                    
                    var type = skill.TbSkillRow.SkillType;
                    if (self.TypeSkillsMap.TryGetValue(type, out List<long> skills))
                    {
                        skills.Remove(instanceId);
                        if (skills.Count == 0)
                        {
                            self.TypeSkillsMap.Remove(type);
                            ObjectPool.Instance.Recycle(skills);
                        }
                    }
                    self.IdSkillMap.Remove(skill.SkillId);
                    skill.Dispose();
                }
                self.RemovedSkills.Clear();
                ObjectPool.Instance.Recycle(self.RemovedSkills);
            }
            
            for (int index = self.RunningSkills.Count - 1; index >= 0; index--)
            {
                long instanceId = self.RunningSkills[index];
                Skill skill = self.GetChild<Skill>(instanceId);
                if (skill == null || !skill.IsRunning) {
                    self.RunningSkills.RemoveAt(index);
                } else {
                    skill.StepRunning();
                }
            }
            self.TryCastCdSkills();
        }

        public static bool HasRunningSkill(this SkillComponent self)
        {
            return self.RunningSkills.Count > 0;
        }

        public static Skill AddSkill(this SkillComponent self, int skillId, bool isRemoveOnDone = false)
        {self.LSRoom()?.ProcessLog.LogFunction(122, self.LSParent().Id, skillId, isRemoveOnDone ? 1 : 0);
            if (!self.IdSkillMap.TryGetValue(skillId, out long instanceId))
            {
                Skill skill = self.AddChild<Skill, int, bool>(skillId, isRemoveOnDone);
                self.IdSkillMap.Add(skillId, skill.Id);

                var type = skill.TbSkillRow.SkillType;
                if (!self.TypeSkillsMap.TryGetValue(type, out List<long> skills))
                {
                    skills = ObjectPool.Instance.Fetch<List<long>>();
                    self.TypeSkillsMap.Add(type, skills);
                }
                skills.Add(skill.Id);
                return skill;
            }
            return self.GetChild<Skill>(instanceId);
        }

        public static void RemoveSkill(this SkillComponent self, int skillId, bool force = false)
        {self.LSRoom()?.ProcessLog.LogFunction(178, self.LSParent().Id, skillId, force ? 1 : 0);
            if (self.IdSkillMap.TryGetValue(skillId, out long instanceId))
            {
                Skill skill = self.GetChild<Skill>(instanceId);
                skill.RemoveSelf(force);
            }
        }

        public static void ForceAllDone(this SkillComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(121, self.LSParent().Id);
            for (int index = self.RunningSkills.Count - 1; index >= 0; index--)
            {
                long instanceId = self.RunningSkills[index];
                Skill skill = self.GetChild<Skill>(instanceId);
                skill.ForceDone();
            }
            self.RunningSkills.Clear();
        }
        
        public static Skill GetRunningSkill(this SkillComponent self, ESkillType type)
        {
            for (int index = self.RunningSkills.Count - 1; index >= 0; index--)
            {
                long instanceId = self.RunningSkills[index];
                Skill skill = self.GetChild<Skill>(instanceId);
                if (skill != null && skill.TbSkillRow.SkillType == type)
                    return skill;
            }
            return null;
        }

        private static bool TryCastCdSkills(this SkillComponent self)
        {
            self.LSRoom()?.ProcessLog.LogIgnore();
            if (self.LSOwner().DeadMark > 0) { return false; }
            if (self.CheckRestrict(ESkillType.CountDown)) { return false; }
            if (!self.TypeSkillsMap.TryGetValue(ESkillType.CountDown, out List<long> skillIds)) { return false; }
            
            for (int i = skillIds.Count - 1; i >= 0; i--)
            {
                Skill skill = self.GetChild<Skill>(skillIds[i]);
                if (skill.TryCast())
                {
                    if (skill.IsRunning)
                        self.RunningSkills.Add(skill.Id);
                }
            }
            return true;
        }

        public static bool TryCastSkill(this SkillComponent self, ESkillType type)
        {self.LSRoom()?.ProcessLog.LogFunction(120, self.LSParent().Id);
            if (self.LSOwner().DeadMark > 0 && type != ESkillType.Dead) { return false; }
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
                            self.RunningSkills.Add(skill.Id);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool TryCastSkill(this SkillComponent self, ESkillType type, int index)
        {self.LSRoom()?.ProcessLog.LogFunction(119, self.LSParent().Id, index);
            if (self.LSOwner().DeadMark > 0) { return false; }
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
                            self.RunningSkills.Add(skill.Id);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool TryCastSkill(this SkillComponent self, int skillId)
        {self.LSRoom()?.ProcessLog.LogFunction(118, self.LSParent().Id, skillId);
            if (self.LSOwner().DeadMark > 0) { return false; }
            // if (!Enable) { return false; }
            
            if (self.IdSkillMap.TryGetValue(skillId, out long instanceId))
            {
                Skill skill = self.GetChild<Skill>(instanceId);
                if (self.CheckRestrict(skill.TbSkillRow.SkillType)) { return false; }
            
                if (skill.TryCast())
                {
                    if (skill.IsRunning)
                        self.RunningSkills.Add(skill.Id);
                    return true;
                }
            }
            return false;
        }

        public static bool CastAttachSkill(this SkillComponent self, int skillId)
        {self.LSRoom()?.ProcessLog.LogFunction(117, self.LSParent().Id, skillId);
            if (self.LSOwner().DeadMark > 0) { return false; }
            // if (!Enable) { return false; }
            if (self.CheckRestrict(ESkillType.Manual)) { return false; }

            Skill skill = self.AddSkill(skillId, true);
            if (skill == null) { return false; }
            if (skill.TryCast())
            {
                if (skill.IsRunning)
                    self.RunningSkills.Add(skill.Id);
                return true;
            }
            else
            {
                skill.Dispose();
            }
            return false;
        }

        private static bool CheckRestrict(this SkillComponent self, ESkillType type)
        {self.LSRoom()?.ProcessLog.LogFunction(116, self.LSParent().Id);
            var flagComponent = self.LSOwner().GetComponent<FlagComponent>();
            switch (type)
            {
                case ESkillType.Normal:
                    return flagComponent.HasRestrict(FlagRestrict.NotAttack);
                case ESkillType.Manual:
                    return flagComponent.HasRestrict(FlagRestrict.NotManual);
                default:
                    return false;
            }
        }

    }
}