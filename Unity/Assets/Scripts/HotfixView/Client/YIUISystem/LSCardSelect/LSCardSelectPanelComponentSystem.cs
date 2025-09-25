using UnityEngine;
using YIUIFramework;
using DG.Tweening;

namespace ET.Client
{
    /// <summary>
    /// Author  alsostone
    /// Date    2025.9.20
    /// Desc
    /// </summary>
    [FriendOf(typeof(LSCardSelectPanelComponent))]
    public static partial class LSCardSelectPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this LSCardSelectPanelComponent self)
        {
            self.CardsView = new YIUIListView<LSCardSelectItemComponent>(self, self.u_ComCardSelectRoot);
        }

        [EntitySystem]
        private static void Destroy(this LSCardSelectPanelComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this LSCardSelectPanelComponent self)
        {
            await ETTask.CompletedTask;
            self.ResetCardsView(true);
            return true;
        }
        
        [EntitySystem]
        private static async ETTask YIUIOpenTween(this LSCardSelectPanelComponent self)
        {
            self.u_ComBackground.color = new Color(0, 0, 0, 0);
            await self.u_ComBackground.DOFadeAsync(0.6f, 0.25f);
        }

        [EntitySystem]
        private static async ETTask YIUICloseTween(this LSCardSelectPanelComponent self)
        {
            self.u_ComBackground.color = new Color(0, 0, 0, 0.6f);
            await self.u_ComBackground.DOFadeAsync(0f, 0.35f);
        }
        
        [EntitySystem]
        private static async ETTask YIUIEvent(this LSCardSelectPanelComponent self, OnCardSelectResetEvent message)
        {
            await ETTask.CompletedTask;
            if (!self.IsClickDone && message.PlayerId == self.Room().LookPlayerId)
                self.ResetCardsView();
        }

        private static void ResetCardsView(this LSCardSelectPanelComponent self, bool force = false)
        {
            LSViewCardSelectComponent viewCardSelectComponent = self.Room().GetLookPlayerComponent<LSViewCardSelectComponent>();
            self.CachedCards = viewCardSelectComponent.CardsQueue;

            int indexNew = 0;
            if (self.CachedCards.Count > 0)
            {
                var cards = self.CachedCards[0];
                for (; indexNew < cards.Count; indexNew++)
                {
                    LSCardSelectItemComponent renderer = null;
                    if (self.CardsView.ItemRenderers.Count > indexNew) {
                        renderer = self.CardsView.ItemRenderers[indexNew];
                    } else {
                        renderer = self.CardsView.CreateItemRenderer();
                        force = true;
                    }
                    if (force) {
                        renderer.UIBase.OwnerRectTransform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                        renderer.UIBase.OwnerRectTransform.DOScale(Vector3.one, 0.25f);
                    }
                    renderer.SetData(cards[indexNew], indexNew);
                }
            }
            
            for (; indexNew < self.CardsView.ItemRenderers.Count; indexNew++) {
                self.CardsView.RemoveItemRenderer(indexNew);
            }
        }
        
        [EntitySystem]
        private static async ETTask YIUIEvent(this LSCardSelectPanelComponent self, OnCardSelectClickEvent message)
        {
            LSCommandData cmd = LSCommand.GenCommandButton(0, CommandButtonType.CardSelect, message.Index);
            self.Room().SendCommandMeesage(cmd);
            self.IsClickDone = true;
            
            // 播放抽卡选中动画 (放大选中卡牌，缩小其他卡牌)
            for (int index = 0; index < self.CardsView.ItemRenderers.Count; index++)
            {
                var transform = self.CardsView.ItemRenderers[index].UIBase.OwnerRectTransform;
                if (index == message.Index) {
                    transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.35f);
                } else {
                    transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.35f);
                }
            }

            // 如果还有抽卡机会，继续抽卡 否则关闭面板
            if (self.CachedCards.Count > 1)
            {
                await self.Root().GetComponent<TimerComponent>().WaitAsync(350);
                self.CardsView.Clear();
                await self.Root().GetComponent<TimerComponent>().WaitAsync(350);
                self.ResetCardsView(true);
            }
            else {
                self.UIPanel.Close();
            }
            self.IsClickDone = false;
        }
        
        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}