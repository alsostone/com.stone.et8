using System.Collections.Generic;
using TrueSync;

namespace ET
{
    public static class ItemExecutor
    {
        public static bool TryExecute(LSUnit owner, TSVector center, int tableId)
        {
            TbItemRow tbItemRow = TbItem.Instance.Get(tableId);
            
            List<SearchUnit> targets = ObjectPool.Instance.Fetch<List<SearchUnit>>();
            TargetSearcher.Search(tbItemRow.SearchTarget, owner, center, TSVector.forward, targets);
            
            bool isOk = targets.Count > 0;
            EffectExecutor.Execute(tbItemRow.EffectGroupId, owner, targets);
            
            targets.Clear();
            ObjectPool.Instance.Recycle(targets);
            return isOk;
        }
    }
}