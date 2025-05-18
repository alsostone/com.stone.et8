using TrueSync;

namespace ET
{
    [FriendOf(typeof(PropComponent))]
    public static class PropComponentSystem
    {
        public static void Set(this PropComponent self, NumericType numericType, FP value, bool isPublicEvent = true)
        {
            FP oldValue = self.Get(numericType);
            if (oldValue == value)
            {
                return;
            }

            self.NumericDic[numericType] = value;

            if (numericType >= NumericType.Max)
            {
                self.Update(numericType, isPublicEvent);
                return;
            }

            if (isPublicEvent)
            {
                EventSystem.Instance.Publish(self.LSWorld(), new PropChange() { Id = self.LSOwner().Id, New = value, Old = oldValue, NumericType = numericType });
            }
        }
        
        public static void Add(this PropComponent self, NumericType numericType, FP value, bool isPublicEvent = true)
        {
            if (0 == value)
            {
                return;
            }

            FP oldValue = self.Get(numericType);
            value = oldValue + value;
            self.NumericDic[numericType] = value;

            if (numericType >= NumericType.Max)
            {
                self.Update(numericType, isPublicEvent);
                return;
            }

            if (isPublicEvent)
            {
                EventSystem.Instance.Publish(self.LSWorld(), new PropChange() { Id = self.LSOwner().Id, New = value, Old = oldValue, NumericType = numericType });
            }
        }

        public static FP Get(this PropComponent self, NumericType key)
        {
            FP value = 0;
            self.NumericDic.TryGetValue(key, out value);
            return value;
        }
        
        public static bool Contains(this PropComponent self, NumericType key)
        {
            return self.NumericDic.ContainsKey(key);
        }

        private static void Update(this PropComponent self, NumericType numericType, bool isPublicEvent)
        {
            int final = (int)numericType / 10;
            NumericType bas = (NumericType)(final * 10 + 1);
            NumericType add = (NumericType)(final * 10 + 2);
            NumericType pct = (NumericType)(final * 10 + 3);
            NumericType finalAdd = (NumericType)(final * 10 + 4);
            NumericType finalPct = (NumericType)(final * 10 + 5);

            // 一个数值可能会多种情况影响，比如速度,加个buff可能增加速度绝对值100，也有些buff增加10%速度，所以一个值可以由5个值进行控制其最终结果
            // final = (((base + add) * (1 + pct)) + finalAdd) * (1 + finalPct);
            FP result = ((self.Get(bas) + self.Get(add)) * (1 + self.Get(pct)) + self.Get(finalAdd)) * (1 + self.Get(finalPct));
            self.Set((NumericType)final, result, isPublicEvent);
        }
    }
    
}