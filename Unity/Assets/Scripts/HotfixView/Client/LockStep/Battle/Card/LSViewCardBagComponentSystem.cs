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
            for (int i = 0; i < self.Items.Count; i++) {
                ObjectPool.Instance.Recycle(self.Items[i]);
            }
            self.Items.Clear();
            self.IdItemMap.Clear();
            
            CardBagComponent bagComponent = self.LSViewOwner().GetUnit().GetComponent<CardBagComponent>();
            foreach (var item in bagComponent.Items) {
                var bagItem = ObjectPool.Instance.Fetch<CardBagItem>();
                bagItem.Id = item.Id;
                bagItem.Type = item.Type;
                bagItem.TableId = item.TableId;
                self.Items.Add(bagItem);
                self.IdItemMap[bagItem.Id] = bagItem;
            }
            self.Fiber().UIEvent(new OnCardViewResetEvent() { PlayerId = self.LSViewOwner().Id }).Coroutine();
        }
        
        public static void AddItem(this LSViewCardBagComponent self, CardBagItem item)
        {
            var bagItem = ObjectPool.Instance.Fetch<CardBagItem>();
            bagItem.Id = item.Id;
            bagItem.Type = item.Type;
            bagItem.TableId = item.TableId;
            self.Items.Add(bagItem);
            self.IdItemMap[bagItem.Id] = bagItem;
            self.Fiber().UIEvent(new OnCardViewResetEvent() { PlayerId = self.LSViewOwner().Id }).Coroutine();
        }
        
        public static void RemoveItem(this LSViewCardBagComponent self, long itemId)
        {
            for (int i = 0; i < self.Items.Count; i++) {
                if (self.Items[i].Id == itemId) {
                    ObjectPool.Instance.Recycle(self.Items[i]);
                    self.Items.RemoveAt(i);
                    self.IdItemMap.Remove(itemId);
                    break;
                }
            }
            self.Fiber().UIEvent(new OnCardViewResetEvent() { PlayerId = self.LSViewOwner().Id }).Coroutine();
        }
        
        public static CardBagItem GetItem(this LSViewCardBagComponent self, long itemId)
        {
            self.IdItemMap.TryGetValue(itemId, out CardBagItem bagItem);
            return bagItem;
        }
    }
}