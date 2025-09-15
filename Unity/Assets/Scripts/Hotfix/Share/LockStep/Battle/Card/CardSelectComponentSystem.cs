
using System.Collections.Generic;

namespace ET
{
    [LSEntitySystemOf(typeof(CardSelectComponent))]
    [EntitySystemOf(typeof(CardSelectComponent))]
    [FriendOf(typeof(CardSelectComponent))]
    public static partial class CardSelectComponentSystem
    {
        [EntitySystem]
        private static void Awake(this CardSelectComponent self)
        {
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this CardSelectComponent self)
        {
            
        }
        
        public static void TrySelectCard(this CardSelectComponent self, int index)
        {
            if (self.SelectCardQueue.Count <= 0)
                return;
            List<CardItem> items = self.SelectCardQueue.Dequeue();
            if (index < 0 || index >= items.Count)
                return;
            CardBagComponent bagComponent = self.LSOwner().GetComponent<CardBagComponent>();
            bagComponent.AddItem(items[index]);
        }
        
    }
}