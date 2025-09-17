using System;
using System.Collections.Generic;

namespace ET
{
    [EntitySystemOf(typeof(CardBagComponent))]
    [FriendOf(typeof(CardBagComponent))]
    public static partial class CardBagComponentSystem
    {
        [EntitySystem]
        private static void Awake(this CardBagComponent self, List<LSRandomDropItem> items)
        {
            foreach (LSRandomDropItem item in items) {
                self.AddItem(item);
            }
        }
        
        public static void AddItem(this CardBagComponent self, LSRandomDropItem item)
        {
            if (self.ItemCountMap.TryGetValue((item.Type, item.TableId), out int count)) {
                self.ItemCountMap[(item.Type, item.TableId)] = count + item.Count;
            } else {
                self.ItemCountMap[(item.Type, item.TableId)] = item.Count;
            }
            EventSystem.Instance.Publish(self.LSWorld(), new LSCardBagAdd() { Id = self.LSOwner().Id, Item = item });
        }
        
        public static void RemoveItem(this CardBagComponent self, EUnitType type, int tableId, int count)
        {
            if (self.ItemCountMap.TryGetValue((type, tableId), out int has)) {
                if (count >= has) {
                    self.ItemCountMap.Remove((type, tableId));
                } else if (count < has) {
                    self.ItemCountMap[(type, tableId)] = has - count;
                }
            }
            LSRandomDropItem item = new LSRandomDropItem() { Type = type, TableId = tableId, Count = count };
            EventSystem.Instance.Publish(self.LSWorld(), new LSCardBagRemove() { Id = self.LSOwner().Id, Item = item });
        }
        
        public static int GetItemCount(this CardBagComponent self, EUnitType type, int tableId)
        {
            if (self.ItemCountMap.TryGetValue((type, tableId), out int count)) {
                return count;
            }
            return 0;
        }
        
    }
}