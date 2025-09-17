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
        
        public static void RemoveItem(this CardBagComponent self, LSRandomDropItem item)
        {
            if (self.ItemCountMap.TryGetValue((item.Type, item.TableId), out int count)) {
                if (item.Count >= count) {
                    self.ItemCountMap.Remove((item.Type, item.TableId));
                } else if (item.Count < count) {
                    self.ItemCountMap[(item.Type, item.TableId)] = count - item.Count;
                }
            }
            EventSystem.Instance.Publish(self.LSWorld(), new LSCardBagRemove() { Id = self.LSOwner().Id, Item = item });
        }
        
    }
}