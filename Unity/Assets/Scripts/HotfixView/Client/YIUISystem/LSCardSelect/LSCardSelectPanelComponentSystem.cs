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
            self.ResetCardsView();
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

        private static void ResetCardsView(this LSCardSelectPanelComponent self)
        {
            Room room = self.Room();
            LSUnitViewComponent lsUnitViewComponent = room.GetComponent<LSUnitViewComponent>();
            LSUnitView lsPlayer = lsUnitViewComponent.GetChild<LSUnitView>(room.LookPlayerId);
            LSViewCardSelectComponent viewCardSelectComponent = lsPlayer.GetComponent<LSViewCardSelectComponent>();

            int indexNew = 0;
            if (viewCardSelectComponent.CardsQueue.Count > 0)
            {
                self.CachedCards = viewCardSelectComponent.CardsQueue[0];
                for (; indexNew < self.CachedCards.Count; indexNew++)
                {
                    var renderer = self.CardsView.ItemRenderers.Count > indexNew ? self.CardsView.ItemRenderers[indexNew] : self.CardsView.CreateItemRenderer();
                    renderer.SetData(self.CachedCards[indexNew], indexNew);
                    renderer.UIBase.OwnerRectTransform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    renderer.UIBase.OwnerRectTransform.DOScale(Vector3.one, 0.25f);
                }
            }
            
            for (; indexNew < self.CardsView.ItemRenderers.Count; indexNew++) {
                self.CardsView.RemoveItemRenderer(indexNew);
            }
        }
        
        [EntitySystem]
        private static async ETTask YIUIEvent(this LSCardSelectPanelComponent self, OnCardSelectClickEvent message)
        {
            await ETTask.CompletedTask;
            if (self.CachedCards == null || message.Index < 0 || message.Index >= self.CachedCards.Count)
                return;
            
            ulong cmd = LSCommand.GenCommandButton(0, CommandButtonType.CardSelect, message.Index);
            self.Room().SendCommandMeesage(cmd);

            self.IsClickDone = true;
            for (int index = 0; index < self.CardsView.ItemRenderers.Count; index++)
            {
                var transform = self.CardsView.ItemRenderers[index].UIBase.OwnerRectTransform;
                if (index == message.Index) {
                    transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.35f);
                } else {
                    transform.DOScale(new Vector3(0.5f, 0.5f, 0.5f), 0.35f);
                }
            }
            self.UIPanel.Close();
        }
        
        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}