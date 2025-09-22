using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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
            self.ResetSelectCards();
            self.ResetBagCards();
            await ETTask.CompletedTask;
            return true;
        }
        
        [EntitySystem]
        private static async ETTask YIUIEvent(this PlayViewComponent self, OnCardDragStartEvent message)
        {
            await ETTask.CompletedTask;
            if (message.PlayerId == self.Room().LookPlayerId)
            {
                TweenerCore<float, float, FloatOptions> t = DOTween.To(() => self.u_ComAlphaGroup.alpha, x => self.u_ComAlphaGroup.alpha = x, 0.3f, 0.2f);
                t.SetTarget(self.UIBase.OwnerRectTransform);
            }
        }
        
        [EntitySystem]
        private static async ETTask YIUIEvent(this PlayViewComponent self, OnCardDragEndEvent message)
        {
            await ETTask.CompletedTask;
            if (message.PlayerId == self.Room().LookPlayerId)
            {
                TweenerCore<float, float, FloatOptions> t = DOTween.To(() => self.u_ComAlphaGroup.alpha, x => self.u_ComAlphaGroup.alpha = x, 1.0f, 0.2f);
                t.SetTarget(self.UIBase.OwnerRectTransform);
            }
        }
        
        [EntitySystem]
        private static async ETTask YIUIEvent(this PlayViewComponent self, OnCardViewResetEvent message)
        {
            await ETTask.CompletedTask;
            if (message.PlayerId == self.Room().LookPlayerId)
                self.ResetBagCards();
        }
        
        
        [EntitySystem]
        private static async ETTask YIUIEvent(this PlayViewComponent self, OnCardSelectResetEvent message)
        {
            await ETTask.CompletedTask;
            if (message.PlayerId == self.Room().LookPlayerId)
                self.ResetSelectCards();
        }

        
        private static void ResetBagCards(this PlayViewComponent self)
        {
            Room room = self.Room();
            LSUnitViewComponent lsUnitViewComponent = room.GetComponent<LSUnitViewComponent>();
            LSUnitView lsPlayer = lsUnitViewComponent.GetChild<LSUnitView>(room.LookPlayerId);
            LSViewCardBagComponent viewCardBagComponent = lsPlayer.GetComponent<LSViewCardBagComponent>();
            
            float width = self.u_ComCardsRoot.rect.width;
            float itemWidth = Math.Min(180, width / viewCardBagComponent.Items.Count);
            
            int indexOld = 0;
            while (indexOld < self.CardsView.ItemRenderers.Count)
            {
                var renderer = self.CardsView.ItemRenderers[indexOld];
                var bagItem = viewCardBagComponent.Items.Count > indexOld ? viewCardBagComponent.Items[indexOld] : null;
                if (bagItem == null || renderer.ItemData.Id != bagItem.Id) {
                    self.CardsView.RemoveItemRenderer(indexOld);
                } else {
                    renderer.UIBase.OwnerRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemWidth);
                    renderer.SetPosition(new Vector3(itemWidth * indexOld, 0));
                    ++indexOld;
                }
            }

            for (int indexNew = indexOld; indexNew < viewCardBagComponent.Items.Count; indexNew++) {
                var bagItem = viewCardBagComponent.Items[indexNew];
                PlayCardItemComponent renderer = self.CardsView.CreateItemRenderer();
                renderer.UIBase.OwnerRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemWidth);
                renderer.SetData(bagItem, new Vector3(itemWidth * indexNew, 0));
            }
        }

        private static void ResetSelectCards(this PlayViewComponent self)
        {
            Room room = self.Room();
            LSUnitViewComponent lsUnitViewComponent = room.GetComponent<LSUnitViewComponent>();
            LSUnitView lsPlayer = lsUnitViewComponent.GetChild<LSUnitView>(room.LookPlayerId);
            LSViewCardSelectComponent viewCardSelectComponent = lsPlayer.GetComponent<LSViewCardSelectComponent>();
            
            self.CachedCards = viewCardSelectComponent.CardsQueue;
            self.u_DataSelectCount.SetValue(self.CachedCards.Count.ToString());
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
            if (self.CachedCards.Count > 0) {
                YIUIRootComponent yiuiRootComponent = self.Room().GetComponent<YIUIRootComponent>();
                yiuiRootComponent.OpenPanelAsync<LSCardSelectPanelComponent>().Coroutine();
            }
        }
        #endregion YIUIEvent结束
    }
}