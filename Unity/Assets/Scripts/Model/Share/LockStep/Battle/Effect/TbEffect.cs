using System.Collections.Generic;

namespace ET
{
    public partial class TbEffect
    {
        private readonly Dictionary<int, List<TbEffectRow>> sGroupEffectRows = new();

        public List<TbEffectRow> GetGroupEffects(int groupId)
        {
            return this.sGroupEffectRows[groupId];
        }

        partial void PostInit()
        {
            foreach (var kv in this.DataList)
            {
                List<TbEffectRow> effectRows;
                if (!this.sGroupEffectRows.TryGetValue(kv.GroupId, out effectRows))
                {
                    effectRows = new List<TbEffectRow>();
                    this.sGroupEffectRows.Add(kv.GroupId, effectRows);
                }
                effectRows.Add(kv);
            }
        }
        
    }
}