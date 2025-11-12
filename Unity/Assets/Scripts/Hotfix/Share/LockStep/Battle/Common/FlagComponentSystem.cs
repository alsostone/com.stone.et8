using System;

namespace ET
{
    [EntitySystemOf(typeof(FlagComponent))]
    [FriendOf(typeof(FlagComponent))]
    public static partial class FlagComponentSystem
    {
        [EntitySystem]
        private static void Awake(this FlagComponent self)
        {
        }
        
        [EntitySystem]
        private static void Awake(this FlagComponent self, int mask)
        {
            self.AddRestrict(mask);
        }

        public static void AddRestrict(this FlagComponent self, int mask)
        {self.LSRoom()?.ProcessLog.LogFunction(35, self.LSParent().Id, mask);
            foreach (FlagRestrict flagRestrict in Enum.GetValues(typeof(FlagRestrict)))
            {
                if ((mask & (int)flagRestrict) == (int)flagRestrict)
                {
                    if (self.RestrictRefrence.TryGetValue(flagRestrict, out var refCount))
                    {
                        self.RestrictRefrence[flagRestrict] = refCount + 1;
                    }
                    else
                    {
                        self.RestrictRefrence.Add(flagRestrict, 1);
                    }
                }
            }
        }
        
        public static void RemoveRestrict(this FlagComponent self, int mask)
        {self.LSRoom()?.ProcessLog.LogFunction(34, self.LSParent().Id, mask);
            foreach (FlagRestrict flagRestrict in Enum.GetValues(typeof(FlagRestrict)))
            {
                if ((mask & (int)flagRestrict) == (int)flagRestrict)
                {
                    if (self.RestrictRefrence.TryGetValue(flagRestrict, out var refCount))
                    {
                        self.RestrictRefrence[flagRestrict] = refCount - 1;
                    }
                    else
                    {
                        self.RestrictRefrence.Add(flagRestrict, -1);
                    }
                }
            }
        }
        
        public static bool HasRestrict(this FlagComponent self, FlagRestrict flagRestrict)
        {self.LSRoom()?.ProcessLog.LogFunction(33, self.LSParent().Id);
            return self.RestrictRefrence.TryGetValue(flagRestrict, out var refCount) && refCount > 0;
        }
        
        public static bool HasFlagLabel(this FlagComponent self, FlagLabel label)
        {
            if (self.FlagLabels.TryGetValue(label, out var value))
                return value > 0;
            return false;
        }
        
        public static void AddBoolFlag(this FlagComponent self, FlagLabel label, int count = 1)
        {
            if (self.FlagLabels.TryGetValue(label, out var value)) {
                self.FlagLabels[label] = value + count;
            } else {
                self.FlagLabels.Add(label, count);
            }
        }
        
        public static void RemoveBoolFlag(this FlagComponent self, FlagLabel label, int count = 1)
        {
            if (self.FlagLabels.TryGetValue(label, out var value)) {
                self.FlagLabels[label] = value - count;
            } else {
                self.FlagLabels.Add(label, -count);
            }
        }
        
    }
}