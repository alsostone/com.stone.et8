namespace ET
{
    [FriendOf(typeof(PropComponent))]
    public static class PropComponentSystem
    {
        public static float GetAsFloat(this PropComponent self, NumericType numericType)
        {
            return (float)self.GetByKey(numericType) / 10000;
        }

        public static int GetAsInt(this PropComponent self, NumericType numericType)
        {
            return (int)self.GetByKey(numericType);
        }

        public static long GetAsLong(this PropComponent self, NumericType numericType)
        {
            return self.GetByKey(numericType);
        }

        public static void Set(this PropComponent self, NumericType nt, float value)
        {
            self.Insert(nt, (long)(value * 10000));
        }

        public static void Set(this PropComponent self, NumericType nt, int value)
        {
            self.Insert(nt, value);
        }

        public static void Set(this PropComponent self, NumericType nt, long value)
        {
            self.Insert(nt, value);
        }

        public static void SetNoEvent(this PropComponent self, NumericType numericType, long value)
        {
            self.Insert(numericType, value, false);
        }

        public static void Insert(this PropComponent self, NumericType numericType, long value, bool isPublicEvent = true)
        {
            long oldValue = self.GetByKey(numericType);
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
                EventSystem.Instance.Publish(self.LSWorld(),
                    new PropChange() { Unit = self.LSOwner(), New = value, Old = oldValue, NumericType = numericType });
            }
        }
        
        public static void Add(this PropComponent self, NumericType numericType, long value, bool isPublicEvent = true)
        {
            if (0 == value)
            {
                return;
            }

            long oldValue = self.GetByKey(numericType);
            value = oldValue + value;
            self.NumericDic[numericType] = value;

            if (numericType >= NumericType.Max)
            {
                self.Update(numericType, isPublicEvent);
                return;
            }

            if (isPublicEvent)
            {
                EventSystem.Instance.Publish(self.LSWorld(),
                    new PropChange() { Unit = self.LSOwner(), New = value, Old = oldValue, NumericType = numericType });
            }
        }

        public static long GetByKey(this PropComponent self, NumericType key)
        {
            long value = 0;
            self.NumericDic.TryGetValue(key, out value);
            return value;
        }

        public static void Update(this PropComponent self, NumericType numericType, bool isPublicEvent)
        {
            int final = (int)numericType / 10;
            var bas = (NumericType)(final * 10 + 1);
            var add = (NumericType)(final * 10 + 2);
            var pct = (NumericType)(final * 10 + 3);
            var finalAdd = (NumericType)(final * 10 + 4);
            var finalPct = (NumericType)(final * 10 + 5);

            // 一个数值可能会多种情况影响，比如速度,加个buff可能增加速度绝对值100，也有些buff增加10%速度，所以一个值可以由5个值进行控制其最终结果
            // final = (((base + add) * (100 + pct) / 100) + finalAdd) * (100 + finalPct) / 100;
            long result = (long)(((self.GetByKey(bas) + self.GetByKey(add)) * (100 + self.GetAsFloat(pct)) / 100f + self.GetByKey(finalAdd)) *
                (100 + self.GetAsFloat(finalPct)) / 100f);
            self.Insert((NumericType)final, result, isPublicEvent);
        }
    }
    
}