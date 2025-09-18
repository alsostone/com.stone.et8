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
            self.ItemCountMap.Clear();
            CardBagComponent bagComponent = self.LSViewOwner().GetUnit().GetComponent<CardBagComponent>();
            foreach (var pair in bagComponent.ItemCountMap) {
                self.ItemCountMap[(pair.Key.Item1, pair.Key.Item2)] = pair.Value;
            }
            self.Fiber().UIEvent(new OnCardViewResetEvent() { PlayerId = self.LSViewOwner().Id }).Coroutine();
        }
        
        public static void AddItem(this LSViewCardBagComponent self, LSRandomDropItem item)
        {
            if (self.ItemCountMap.TryGetValue((item.Type, item.TableId), out int count)) {
                self.ItemCountMap[(item.Type, item.TableId)] = count + item.Count;
            } else {
                self.ItemCountMap[(item.Type, item.TableId)] = item.Count;
            }
            self.Fiber().UIEvent(new OnCardViewResetEvent() { PlayerId = self.LSViewOwner().Id }).Coroutine();
        }
        
        public static void RemoveItem(this LSViewCardBagComponent self, LSRandomDropItem item)
        {
            if (self.ItemCountMap.TryGetValue((item.Type, item.TableId), out int count)) {
                if (item.Count >= count) {
                    self.ItemCountMap.Remove((item.Type, item.TableId));
                } else if (item.Count < count) {
                    self.ItemCountMap[(item.Type, item.TableId)] = count - item.Count;
                }
            }
            self.Fiber().UIEvent(new OnCardViewResetEvent() { PlayerId = self.LSViewOwner().Id }).Coroutine();
        }
    }
}