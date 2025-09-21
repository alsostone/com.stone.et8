using System;
using System.Collections.Generic;

namespace ET.Client
{
    [LSEntitySystemOf(typeof(LSViewCardSelectComponent))]
    [EntitySystemOf(typeof(LSViewCardSelectComponent))]
    [FriendOf(typeof(LSViewCardSelectComponent))]
    public static partial class LSViewCardSelectComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSViewCardSelectComponent self)
        {
            self.LSRollback();
        }
        

        [LSEntitySystem]
        private static void LSRollback(this LSViewCardSelectComponent self)
        {
            foreach (var items in self.CardsQueue) {
                items.Clear();
                ObjectPool.Instance.Recycle(items);
            }
            self.CardsQueue.Clear();
            
            CardSelectComponent selectComponent = self.LSViewOwner().GetUnit().GetComponent<CardSelectComponent>();
            foreach (var cards in selectComponent.CardsQueue) {
                var results = ObjectPool.Instance.Fetch<List<LSRandomDropItem>>();
                results.AddRange(cards);
                self.CardsQueue.Add(results);
            }
            self.Fiber().UIEvent(new OnCardSelectResetEvent() { PlayerId = self.LSViewOwner().Id }).Coroutine();
        }

        public static void AddCards(this LSViewCardSelectComponent self, List<LSRandomDropItem> cards)
        {
            // 传入的参数都是逻辑层的，这里需要拷贝一份，防止逻辑层被回收
            var results = ObjectPool.Instance.Fetch<List<LSRandomDropItem>>();
            results.AddRange(cards);
            self.CardsQueue.Add(results);
            self.Fiber().UIEvent(new OnCardSelectResetEvent() { PlayerId = self.LSViewOwner().Id }).Coroutine();
        }
        
        public static void SelectCards(this LSViewCardSelectComponent self, int index)
        {
            if (self.CardsQueue.Count > 0)
            {
                var items = self.CardsQueue[0];
                self.CardsQueue.RemoveAt(0);
                items.Clear();
                ObjectPool.Instance.Recycle(items);
            }
            self.Fiber().UIEvent(new OnCardSelectResetEvent() { PlayerId = self.LSViewOwner().Id }).Coroutine();
        }
    }
}