using System;
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
            LSWorld lsWorld = self.LSWorld();
            LSStageComponent lsStageComponent = lsWorld.GetComponent<LSStageComponent>();
            if (!lsStageComponent.CheckWaveDone(self.CurrentSelectCount + 1))
                return;
            
            TeamComponent teamComponent = self.LSOwner().GetComponent<TeamComponent>();
            LSTargetsComponent lsTargetsComponent = lsWorld.GetComponent<LSTargetsComponent>();
            if (lsTargetsComponent.GetAliveCount(teamComponent.GetEnemyTeam()) > 0)
                return;
            
            // 波次结束且敌方单位全部死亡则给于抽卡一次
            self.CurrentSelectCount++;
            int index = Math.Min(lsStageComponent.TbRow.RandomCards.Length, self.CurrentSelectCount);
            int randomSet = lsStageComponent.TbRow.RandomCards[index - 1];
            
            var results = ObjectPool.Instance.Fetch<List<LSRandomDropItem>>();
            RandomDropHelper.RandomSet(self.GetRandom(), randomSet, 3, results);
            self.CardsQueue.Add(results);
            
            EventSystem.Instance.Publish(self.LSWorld(), new LSCardSelectAdd() { Id = self.LSOwner().Id, Cards = results });
        }
        
        public static void TrySelectCard(this CardSelectComponent self, int index)
        {
            if (self.CardsQueue.Count <= 0)
                return;
            
            var items = self.CardsQueue[0];
            self.CardsQueue.RemoveAt(0);
            
            index = Math.Min(items.Count - 1, index);
            CardBagComponent bagComponent = self.LSOwner().GetComponent<CardBagComponent>();
            bagComponent.AddItem(items[index]);
            items.Clear();
            ObjectPool.Instance.Recycle(items);
            
            EventSystem.Instance.Publish(self.LSWorld(), new LSCardSelectDone() { Id = self.LSOwner().Id, Index = index });
        }
        
    }
}