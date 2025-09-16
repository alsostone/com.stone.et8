using System;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSViewCardBagComponent))]
    [FriendOf(typeof(LSViewCardBagComponent))]
    public static partial class LSViewCardBagComponentSystem
    {
        public static void AddCard(this LSViewCardBagComponent self, EUnitType type, int tableId, int count)
        {
            self.Items.Add(new Tuple<EUnitType, int, int>(type, tableId, count));
        }
        
        public static void RemoveCard(this LSViewCardBagComponent self, EUnitType type, int tableId, int count)
        {
            for (int i = 0; i < self.Items.Count; i++) {
                Tuple<EUnitType, int, int> it = self.Items[i];
                if (it.Item1 == type && it.Item2 == tableId)
                {
                    if (count > it.Item3)
                    {
                        count -= it.Item3;
                        self.Items.RemoveAt(i);
                        continue;
                    }

                    if(it.Item3 > count) {
                        self.Items[i] = new Tuple<EUnitType, int, int>(it.Item1, it.Item2, it.Item3 - count);
                    } else {
                        self.Items.RemoveAt(i);
                    }
                    return;
                }
            }
        }
    }
}