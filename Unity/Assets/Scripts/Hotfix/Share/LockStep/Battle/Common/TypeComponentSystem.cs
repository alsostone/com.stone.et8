using System.Collections.Generic;
using System.Runtime.Remoting;

namespace ET
{
    [EntitySystemOf(typeof(TypeComponent))]
    [FriendOf(typeof(TypeComponent))]
    public static partial class TypeComponentSystem
    {
        [EntitySystem]
        private static void Awake(this TypeComponent self, EUnitType type)
        {
            self.Type = type;
        }
        
        public static EUnitType GetUnitType(this TypeComponent self)
        {
            return self.Type;
        }
    }
}