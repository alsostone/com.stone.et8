
using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSViewPropComponent))]
    [FriendOf(typeof(LSViewPropComponent))]
    [FriendOf(typeof(PropComponent))]
    public static partial class LSViewPropComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSViewPropComponent self, LSUnit lsUnit)
        {
            PropComponent propComponent = lsUnit.GetComponent<PropComponent>();
            foreach (var kv in propComponent.NumericDic) {
                self.PropValueMapping[kv.Key] = (float)kv.Value;
            }
        }

        [EntitySystem]
        private static void Destroy(this LSViewPropComponent self)
        {
            self.PropValueMapping.Clear();
        }

        public static void ResetPropValue(this LSViewPropComponent self, NumericType type, float value)
        {
            self.PropValueMapping[type] = value;
        }
        
        public static float GetPropValue(this LSViewPropComponent self, NumericType type)
        {
            if (self.PropValueMapping.TryGetValue(type, out float value)) {
                return value;
            }
            return 0f;
        }
        
        public static bool CheckConsumeEnough(this LSViewPropComponent self, Dictionary<NumericType, int> consumes)
        {
            if (consumes == null || consumes.Count == 0)
                return true;
            foreach (var consume in consumes)
            {
                if (self.GetPropValue(consume.Key) < consume.Value)
                    return false;
            }
            return true;
        }

    }
    
}