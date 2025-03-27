using System.Collections.Generic;

namespace ET
{
    [EntitySystemOf(typeof(SkillComponent))]
    [FriendOf(typeof(SkillComponent))]
    public static partial class SkillComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SkillComponent self, int[] skillIds)
        {
            foreach (int skillId in skillIds)
            {
                self.AddSkill(skillId);
            }
        }

        [EntitySystem]
        private static void Destroy(this SkillComponent self)
        {
            self.IdSkillMap.Clear();
            self.TypeSkillsMap.Clear();
        }

        public static Skill AddSkill(this SkillComponent self, int skillId, bool isOnlyOnce = false)
        {
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
        {
            // 技能移除不会维护该数组 获取不到技能也不会有问题
            foreach (var id in self.mRunningSkills) {
                var skill = self.GetChild<Skill>(id);
                skill?.ForceDone();
            }
            self.mRunningSkills.Clear();
        }

        public static void TryCastSkill(this SkillComponent self, ESkillType type)
        {
            // if (!Enable) { return; }
            // if (entity.ComStatus.HasStatus(kStatusType.RestrictSkill)) { return; }
            
            if (self.TypeSkillsMap.TryGetValue(type, out List<long> skillIds))
            {
                for (var i = skillIds.Count - 1; i >= 0; i--) {
                    var skill = self.GetChild<Skill>(skillIds[i]);
                    if (skill.TryCast()) {
                        self.mRunningSkills.Add(skill.Id);
                        break;
                    }
                }
            }
        }
        
        public static bool TryCastSkill(this SkillComponent self, ESkillType type, int index)
        {
            // if (!Enable) { return false; }
            // if (entity.ComStatus.HasStatus(kStatusType.RestrictSkill)) { return false; }

            if (self.TypeSkillsMap.TryGetValue(type, out List<long> skillIds))
            {
                if (skillIds.Count > index) {
                    var skill = self.GetChild<Skill>(skillIds[index]);
                    if (skill.TryCast()) {
                        self.mRunningSkills.Add(skill.Id);
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool CastAttachSkill(this SkillComponent self, int id)
        {
            // if (!Enable) { return false; }
            // if (entity.ComStatus.HasStatus(kStatusType.RestrictSkill)) { return false; }

            var skill = self.AddSkill(id, true);
            if (skill == null) { return false; }
            if (skill.TryCast()) {
                self.mRunningSkills.Add(skill.Id);
                return true;
            }
            skill.Dispose();
            return false;
        }

    }
}