using System;
using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSViewCardSelectComponent))]
    [FriendOf(typeof(LSViewCardSelectComponent))]
    public static partial class LSViewCardSelectComponentSystem
    {
        public static void AddCards(this LSViewCardSelectComponent self, List<Tuple<EUnitType, int, int>> cards)
        {
            var results = ObjectPool.Instance.Fetch<List<Tuple<EUnitType, int, int>>>();
            results.AddRange(cards);
            self.CardsQueue.Add(results);
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
        }
    }
}