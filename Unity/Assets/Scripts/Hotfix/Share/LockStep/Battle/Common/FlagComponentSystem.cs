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

        public static void AddRestrict(this FlagComponent self, int mask)
        {
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
        {
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
        {
            return self.RestrictRefrence.TryGetValue(flagRestrict, out var refCount) && refCount > 0;
        }
    }
}