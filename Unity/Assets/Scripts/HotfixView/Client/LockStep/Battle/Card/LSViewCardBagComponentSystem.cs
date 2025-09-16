using System;
using System.Collections.Generic;

namespace ET.Client
{
    [LSEntitySystemOf(typeof(LSViewCardBagComponent))]
    [EntitySystemOf(typeof(LSViewCardBagComponent))]
    [FriendOf(typeof(LSViewCardBagComponent))]
    public static partial class LSViewCardBagComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSViewCardBagComponent self)
        {
            self.LSRollback();
        }

        [LSEntitySystem]
        private static void LSRollback(this LSViewCardBagComponent self)
        {
            self.Items.Clear();
            CardBagComponent bagComponent = self.LSViewOwner().GetUnit().GetComponent<CardBagComponent>();
            foreach (var tuple in bagComponent.Items) {
                self.AddItem(tuple.Item1, tuple.Item2, tuple.Item3);
            }
        }
        
        public static void AddItem(this LSViewCardBagComponent self, EUnitType type, int tableId, int count)
        {
            self.Items.Add(new Tuple<EUnitType, int, int>(type, tableId, count));
        }
        
        public static void RemoveItem(this LSViewCardBagComponent self, EUnitType type, int tableId, int count)
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