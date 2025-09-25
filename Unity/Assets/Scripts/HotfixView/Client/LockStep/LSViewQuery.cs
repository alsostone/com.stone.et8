using System.Collections.Generic;
using TrueSync;
using UnityEngine;

namespace ET.Client
{
    // 统一对逻辑层的查询接口 暂时直接调用方法，后续可以改造成专门的查询指令
    public static class LSViewQuery
    {
        public static void Search(int id, LSUnitView owner, Vector3 center, List<SearchUnit> results)
        {
            TargetSearcher.Search(id, owner.GetUnit(), center.ToTSVector(), TSVector.forward, results);
        }
        
    }
}
