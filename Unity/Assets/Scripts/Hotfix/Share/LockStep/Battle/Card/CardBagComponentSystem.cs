using System;

namespace ET
{
    [EntitySystemOf(typeof(CardBagComponent))]
    [FriendOf(typeof(CardBagComponent))]
    public static partial class CardBagComponentSystem
    {
        [EntitySystem]
        private static void Awake(this CardBagComponent self)
        {
        }
        
        public static void AddItem(this CardBagComponent self, Tuple<EUnitType, int, int> item)
        {
            self.Items.Add(item);
        }
        
        public static void RemoveItem(this CardBagComponent self, Tuple<EUnitType, int, int> item)
        {
            for (int i = 0; i < self.Items.Count; i++) {
                Tuple<EUnitType, int, int> it = self.Items[i];
                if (it.Item1 == item.Item1 && it.Item2 == item.Item2)
                {
                    self.Items.RemoveAt(i);
                    return;
                }
            }
        }
        
        public static Tuple<EUnitType, int, int> GetItem(this CardBagComponent self, EUnitType type, int tableId)
        {
            for (int i = 0; i < self.Items.Count; i++) {
                Tuple<EUnitType, int, int> item = self.Items[i];
                if (item.Item1 == type && item.Item2 == tableId) {
                    return item;
                }
            }
            return null;
        }
    }
}