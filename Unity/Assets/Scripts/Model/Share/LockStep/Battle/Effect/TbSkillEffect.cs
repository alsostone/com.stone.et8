using System.Collections.Generic;

namespace ET
{
    public partial class TbSkillEffect
    {
        private readonly Dictionary<int, List<TbSkillEffectRow>> sGroupEffectRows = new();

        public List<TbSkillEffectRow> GetGroupEffects(int aiConfigId)
        {
            return this.sGroupEffectRows[aiConfigId];
        }

        partial void PostInit()
        {
            foreach (var kv in this.DataList)
            {
                List<TbSkillEffectRow> effectRows;
                if (!this.sGroupEffectRows.TryGetValue(kv.GroupId, out effectRows))
                {
                    effectRows = new List<TbSkillEffectRow>();
                    this.sGroupEffectRows.Add(kv.GroupId, effectRows);
                }
                effectRows.Add(kv);
            }
        }
        
    }
}