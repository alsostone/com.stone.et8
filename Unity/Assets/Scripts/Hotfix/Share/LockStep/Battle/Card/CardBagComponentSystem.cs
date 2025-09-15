
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
        
        public static void AddItem(this CardBagComponent self, CardItem item)
        {
            self.Items.Add(item);
        }
        
        public static void RemoveItem(this CardBagComponent self, EUnitType type, int tableId, int count)
        {
            for (int i = 0; i < self.Items.Count; i++) {
                CardItem item = self.Items[i];
                if (item.Type == type && item.TableId == tableId)
                {
                    if (item.Count > count) {
                        item.Count -= count;
                        self.Items[i] = item;
                    } else {
                        self.Items.RemoveAt(i);
                    }
                    return;
                }
            }
        }
        
        public static CardItem? GetItem(this CardBagComponent self, EUnitType type, int tableId)
        {
            for (int i = 0; i < self.Items.Count; i++) {
                CardItem item = self.Items[i];
                if (item.Type == type && item.TableId == tableId) {
                    return item;
                }
            }
            return null;
        }
    }
}