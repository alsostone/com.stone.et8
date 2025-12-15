using System.Collections.Generic;

namespace ET
{
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
