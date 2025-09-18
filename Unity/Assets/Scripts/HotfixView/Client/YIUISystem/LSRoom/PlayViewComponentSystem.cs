using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace ET.Client
{
    /// <summary>
    /// Author  YIUI
    /// Date    2025.3.6
    /// Desc
    /// </summary>
    [FriendOf(typeof(PlayViewComponent))]
    public static partial class PlayViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this PlayViewComponent self)
        {
            self.CardsView = new YIUIListView<PlayCardItemComponent>(self, self.u_ComCardsRoot);
        }
        
        [EntitySystem]
        private static void Destroy(this PlayViewComponent self)
        {
        }
        
        [EntitySystem]
        private static void Update(this PlayViewComponent self)
        {
            Room room = self.Room();
            if (self.PredictFrame != room.PredictionFrame)
            {
                self.PredictFrame = room.PredictionFrame;
                self.u_DataPredictFrame.SetValue(room.PredictionFrame.ToString());
            }
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this PlayViewComponent self)
        {
            Room room = self.Room();
            LSUnitViewComponent lsUnitViewComponent = room.GetComponent<LSUnitViewComponent>();
            LSUnitView lsPlayer = lsUnitViewComponent.GetChild<LSUnitView>(room.LookPlayerId);
            
            LSViewCardBagComponent viewCardBagComponent = lsPlayer.GetComponent<LSViewCardBagComponent>();
            self.ResetBagCards(viewCardBagComponent.ItemCountMap);
            
            LSViewCardSelectComponent viewCardSelectComponent = lsPlayer.GetComponent<LSViewCardSelectComponent>();
            self.ResetSelectCards(viewCardSelectComponent.CardsQueue);
            
            await ETTask.CompletedTask;
            return true;
        }
        
        [EntitySystem]
        private static async ETTask YIUIEvent(this PlayViewComponent self, OnCardDragStartEvent message)
        {
            await ETTask.CompletedTask;
            self.u_ComAlphaGroup.alpha = 0.6f;
        }
        
        [EntitySystem]
        private static async ETTask YIUIEvent(this PlayViewComponent self, OnCardDragEndEvent message)
        {
            await ETTask.CompletedTask;
            self.u_ComAlphaGroup.alpha = 1f;
        }
        
        public static void ResetBagCards(this PlayViewComponent self, Dictionary<(EUnitType, int), int> bagCards)
        {
            self.CardsView.Clear();

            int index = 0;
            foreach (var pair in bagCards)
            {
                var renderer = self.CardsView.CreateItemRenderer();
                renderer.SetData(new PlayCardItemData() { Type = pair.Key.Item1, TableId = pair.Key.Item2, Index = index++ });
            }
        }

        public static void ResetSelectCards(this PlayViewComponent self, List<List<LSRandomDropItem>> selectCards)
        {
            self.SelectCardCount = selectCards.Count;
            self.u_DataSelectCount.SetValue(selectCards.Count.ToString());
        }

        #region YIUIEvent开始
        
        private static void OnEventSaveNameAction(this PlayViewComponent self, string p1)
        {
            self.SaveName = p1;
        }
        
        private static void OnEventSaveReplayAction(this PlayViewComponent self)
        {
            LSClientHelper.SaveReplay(self.Room(), self.SaveName);
        }
        
        private static void OnEventTestMoveAction(this PlayViewComponent self)
        {
            ulong cmd = LSCommand.GenCommandFloat24x2(0, OperateCommandType.Move, Random.Range(-1, 1), Random.Range(-1, 1));
            self.Room().SendCommandMeesage(cmd);
        }
        
        private static void OnEventSelectCardAction(this PlayViewComponent self)
        {
            if (self.SelectCardCount > 0) {
                ulong cmd = LSCommand.GenCommandButton(0, CommandButtonType.CardSelect, 1);
                self.Room().SendCommandMeesage(cmd);
            }
        }
        #endregion YIUIEvent结束
    }
}