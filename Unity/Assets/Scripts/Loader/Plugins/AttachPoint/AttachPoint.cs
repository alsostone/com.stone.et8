using System.Collections.Generic;

namespace ET
{
    public enum AttachPoint
    {
        None = 0,
        Head = 1,   // 头部
        Chest = 2,  // 胸部
        LHand = 3,  // 左手
        RHand = 4,  // 右手

    }
    public static class AttachPointNameMapping
    {
        public static readonly Dictionary<string, AttachPoint> Mapping = new Dictionary<string, AttachPoint>()
        {
            {"headslot", AttachPoint.Head},
            {"chestslot", AttachPoint.Chest},
            {"handslot.l", AttachPoint.LHand},
            {"handslot.r", AttachPoint.RHand},
        };
    }
}
