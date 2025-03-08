using System.Collections.Generic;

namespace ET
{
    public partial class TbAI
    {
        public Dictionary<int, SortedDictionary<int, TbAIRow>> AIConfigs = new();

        public SortedDictionary<int, TbAIRow> GetAI(int aiConfigId)
        {
            return this.AIConfigs[aiConfigId];
        }

        partial void PostInit()
        {
            foreach (var kv in this.DataMap)
            {
                SortedDictionary<int, TbAIRow> aiNodeConfig;
                if (!this.AIConfigs.TryGetValue(kv.Value.AIConfigId, out aiNodeConfig))
                {
                    aiNodeConfig = new SortedDictionary<int, TbAIRow>();
                    this.AIConfigs.Add(kv.Value.AIConfigId, aiNodeConfig);
                }

                aiNodeConfig.Add(kv.Key, kv.Value);
            }
        }
    }
}