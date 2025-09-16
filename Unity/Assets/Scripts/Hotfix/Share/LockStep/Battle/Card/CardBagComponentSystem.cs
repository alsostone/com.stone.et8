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
        
        public static void AddCard(this CardBagComponent self, Tuple<EUnitType, int, int> item)
        {
            self.Items.Add(item);
            EventSystem.Instance.Publish(self.LSWorld(), new LSCardBagAdd() { Id = self.LSOwner().Id, Type = item.Item1, TableId = item.Item2, Count = item.Item3 });
        }
        
        public static void RemoveCard(this CardBagComponent self, Tuple<EUnitType, int, int> item)
        {
            for (int i = 0; i < self.Items.Count; i++) {
                Tuple<EUnitType, int, int> it = self.Items[i];
                if (it.Item1 == item.Item1 && it.Item2 == item.Item2)
                {
                    if (item.Item3 > it.Item3)
                    {
                        item = new Tuple<EUnitType, int, int>(item.Item1, item.Item2, item.Item3 - it.Item3);
                        self.Items.RemoveAt(i);
                        continue;
                    }

                    if(it.Item3 > item.Item3){
                        self.Items[i] = new Tuple<EUnitType, int, int>(it.Item1, it.Item2, it.Item3 - item.Item3);
                        
                    } else {
                        self.Items.RemoveAt(i);
                    }
                    return;
                }
            }
            EventSystem.Instance.Publish(self.LSWorld(), new LSCardBagRemove() { Id = self.LSOwner().Id, Type = item.Item1, TableId = item.Item2, Count = item.Item3 });
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