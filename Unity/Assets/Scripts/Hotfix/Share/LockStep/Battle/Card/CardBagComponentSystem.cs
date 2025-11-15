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
        {self.LSRoom()?.ProcessLog.LogFunction(50, self.LSParent().Id);
            foreach (LSRandomDropItem item in items) {
                self.AddItem(item);
            }
        }
        
        [EntitySystem]
        private static void Deserialize(this CardBagComponent self)
        {
            self.IdItemMap.Clear();
            foreach (CardBagItem bagItem in self.Items) {
                self.IdItemMap[bagItem.Id] = bagItem;
            }
        }
        
        public static void AddItem(this CardBagComponent self, LSRandomDropItem dropItem)
        {self.LSRoom()?.ProcessLog.LogFunction(49, self.LSParent().Id);
            for (int i = 0; i < dropItem.Count; i++) {
                CardBagItem bagItem = ObjectPool.Instance.Fetch<CardBagItem>();
                bagItem.Id = self.LSWorld().GetId();
                bagItem.Type = dropItem.Type;
                bagItem.TableId = dropItem.TableId;
                self.Items.Add(bagItem);
                self.IdItemMap[bagItem.Id] = bagItem;
                EventSystem.Instance.Publish(self.LSWorld(), new LSCardBagAdd() { Id = self.LSOwner().Id, Item = bagItem });
            }
        }
        
        public static void RemoveItem(this CardBagComponent self, long itemId)
        {self.LSRoom()?.ProcessLog.LogFunction(48, self.LSParent().Id);
            if (self.IdItemMap.TryGetValue(itemId, out CardBagItem bagItem)) {
                self.Items.Remove(bagItem);
                self.IdItemMap.Remove(itemId);
                ObjectPool.Instance.Recycle(bagItem);
                EventSystem.Instance.Publish(self.LSWorld(), new LSCardBagRemove() { Id = self.LSOwner().Id, ItemId = itemId });
            }
        }
        
        public static bool HasItem(this CardBagComponent self, long itemId)
        {
            return self.IdItemMap.ContainsKey(itemId);
        }
        
        public static CardBagItem GetItem(this CardBagComponent self, long itemId)
        {
            self.IdItemMap.TryGetValue(itemId, out CardBagItem bagItem);
            return bagItem;
        }
        
    }
}