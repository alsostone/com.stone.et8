using System;
using UnityEngine;
using YIUIFramework;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TrueSync;
using UnityEngine.UI;

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
            self.u_ComArrowIndicator.localScale = Vector3.zero;
            self.u_ComSelectIndicator.localScale = Vector3.zero;
            
            self.CachedBodyImages.Clear();
            for (int i = 0; i < self.u_ComArrowBodyView.childCount; i++) {
                Transform childTfm = self.u_ComArrowBodyView.GetChild(i);
                self.CachedBodyImages.Add(childTfm, childTfm.GetComponent<Image>());
            }
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

            if (self.IsArrowDragging)
            {
                self.ExecuteArrowBodyAnimationStep();
            }
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this PlayViewComponent self)
        {
            self.u_DataLocalMode.SetValue(self.Room().LockStepMode == LockStepMode.Local);
            self.ResetSelectCards();
            self.ResetBagCards();
            await ETTask.CompletedTask;
            return true;
        }

        #region CardDragEvent
        [EntitySystem]
        private static async ETTask YIUIEvent(this PlayViewComponent self, UICardDragStartEvent message)
        {
            await ETTask.CompletedTask;
            if (message.PlayerId == self.GetLookPlayerId())
            {
                TweenerCore<float, float, FloatOptions> t = DOTween.To(() => self.u_ComAlphaGroup.alpha, x => self.u_ComAlphaGroup.alpha = x, 0.3f, 0.2f);
                t.SetTarget(self.u_ComAlphaGroup);
            }
        }
        
        [EntitySystem]
        private static async ETTask YIUIEvent(this PlayViewComponent self, UICardDragEndEvent message)
        {
            await ETTask.CompletedTask;
            if (message.PlayerId == self.GetLookPlayerId())
            {
                TweenerCore<float, float, FloatOptions> t = DOTween.To(() => self.u_ComAlphaGroup.alpha, x => self.u_ComAlphaGroup.alpha = x, 1.0f, 0.2f);
                t.SetTarget(self.u_ComAlphaGroup);
            }
        }
        #endregion CardDragEvent
        
        #region ArrowDragEvent
        [EntitySystem]
        private static async ETTask YIUIEvent(this PlayViewComponent self, UIArrowDragStartEvent message)
        {
            await ETTask.CompletedTask;
            if (message.PlayerId == self.GetLookPlayerId())
            {
                PlayCardItemComponent itemRenderer = null;
                foreach (PlayCardItemComponent renderer in self.CardsView.ItemRenderers)
                {
                    if (renderer.ItemData.Id == message.ItemId) {
                        itemRenderer = renderer;
                        break;
                    }
                }
                if (itemRenderer != null) {
                    self.IsArrowDragging = true;
                    self.u_ComArrowIndicator.position = itemRenderer.UIBase.OwnerRectTransform.GetWorldCenter();
                    self.ArrowDragStartPosition = self.u_ComArrowIndicator.localPosition;
                }
            }
        }
        
        [EntitySystem]
        private static async ETTask YIUIEvent(this PlayViewComponent self, UIArrowDragEvent message)
        {
            await ETTask.CompletedTask;
            if (self.IsArrowDragging && message.PlayerId == self.GetLookPlayerId())
            {
                LSCameraComponent cameraComponent = self.Room().GetComponent<LSCameraComponent>();
                Vector3 screenPos = cameraComponent.Camera.WorldToScreenPoint(message.Position);
                RectTransformUtility.ScreenPointToWorldPointInRectangle (self.UIBase.OwnerRectTransform, screenPos, YIUIMgrComponent.Inst.UICamera, out Vector3 pos);
                self.u_ComArrowIndicator.position = pos;
                
                Vector3 localPosition = self.u_ComArrowIndicator.localPosition;
                self.u_ComArrowIndicator.rotation = Quaternion.LookRotation(Vector3.forward, localPosition - self.ArrowDragStartPosition);
                self.u_ComArrowIndicator.localScale = Vector3.one;
                self.ResetArrowBodyLength(self.ArrowDragStartPosition, localPosition);
            }
        }
        
        [EntitySystem]
        private static async ETTask YIUIEvent(this PlayViewComponent self, UIArrowDragEndEvent message)
        {
            await ETTask.CompletedTask;
            if (self.IsArrowDragging && message.PlayerId == self.GetLookPlayerId())
            {
                self.IsArrowDragging = false;
                self.u_ComArrowIndicator.localScale = Vector3.zero;
            }
        }
        #endregion ArrowDragEvent
        
        #region SelectDragEvent
        [EntitySystem]
        private static async ETTask YIUIEvent(this PlayViewComponent self, UISelectDragStartEvent message)
        {
            await ETTask.CompletedTask;
            if (message.PlayerId == self.GetLookPlayerId())
            {
                LSCameraComponent cameraComponent = self.Room().GetComponent<LSCameraComponent>();
                Vector3 screenPos = cameraComponent.Camera.WorldToScreenPoint(message.Position);
                RectTransformUtility.ScreenPointToWorldPointInRectangle(self.UIBase.OwnerRectTransform, screenPos, YIUIMgrComponent.Inst.UICamera, out Vector3 pos);
                self.SelectDragStartPosition = self.UIBase.OwnerRectTransform.InverseTransformPoint(pos);
            }
        }
        
        [EntitySystem]
        private static async ETTask YIUIEvent(this PlayViewComponent self, UISelectDragEvent message)
        {
            await ETTask.CompletedTask;
            if (message.PlayerId == self.GetLookPlayerId())
            {
                LSCameraComponent cameraComponent = self.Room().GetComponent<LSCameraComponent>();
                Vector3 screenPos = cameraComponent.Camera.WorldToScreenPoint(message.Position);
                RectTransformUtility.ScreenPointToWorldPointInRectangle(self.UIBase.OwnerRectTransform, screenPos, YIUIMgrComponent.Inst.UICamera, out Vector3 pos);
                Vector3 point = self.UIBase.OwnerRectTransform.InverseTransformPoint(pos);
                Vector3 sizeDelta = point - self.SelectDragStartPosition;
                if (sizeDelta.sqrMagnitude > 25)
                {
                    self.u_ComSelectIndicator.localPosition = self.SelectDragStartPosition + sizeDelta / 2;
                    self.u_ComSelectIndicator.sizeDelta = sizeDelta.Abs();
                    self.u_ComSelectIndicator.localScale = Vector3.one;
                }
            }
        }
        
        [EntitySystem]
        private static async ETTask YIUIEvent(this PlayViewComponent self, UISelectDragEndEvent message)
        {
            await ETTask.CompletedTask;
            if (message.PlayerId == self.GetLookPlayerId())
            {
                self.u_ComSelectIndicator.localScale = Vector3.zero;
            }
        }
        #endregion SelectDragEvent
        
        [EntitySystem]
        private static async ETTask YIUIEvent(this PlayViewComponent self, OnCardViewResetEvent message)
        {
            await ETTask.CompletedTask;
            if (message.PlayerId == self.GetLookPlayerId())
                self.ResetBagCards();
        }
        
        [EntitySystem]
        private static async ETTask YIUIEvent(this PlayViewComponent self, OnCardSelectResetEvent message)
        {
            await ETTask.CompletedTask;
            if (message.PlayerId == self.GetLookPlayerId())
                self.ResetSelectCards();
        }

        private static void ResetArrowBodyLength(this PlayViewComponent self, Vector3 start, Vector3 end)
        {
            Vector2 sizeDelta = self.u_ComArrowBody.sizeDelta;
            sizeDelta.y = Math.Max(0, Vector3.Distance(start, end) + self.u_ComArrowBody.localPosition.y);
            self.u_ComArrowBody.sizeDelta = sizeDelta;
        }
        
        private static void ExecuteArrowBodyAnimationStep(this PlayViewComponent self)
        {
            Vector2 sizeDelta = self.u_ComArrowBody.sizeDelta;
            for (int i = 0; i < self.u_ComArrowBodyView.childCount; i++) {
                RectTransform childTfm = self.u_ComArrowBodyView.GetChild(i) as RectTransform;
                float height = childTfm.sizeDelta.y;
                
                Vector3 position = childTfm.localPosition;
                position.y += Time.deltaTime * 200;
                childTfm.localPosition = position;
                
                Image image = self.CachedBodyImages[childTfm];
                float alphaHead = Math.Clamp(-(position.y - height), 0, height * 4) / (height * 4);
                float alphaTail = Math.Clamp(sizeDelta.y + position.y, 0, height * 3) / (height * 3);
                image.color = new Color(1, 1, 1, Math.Min(alphaHead, alphaTail));

                if (position.y > height) {
                    Transform lastTfm = self.u_ComArrowBodyView.GetChild(self.u_ComArrowBodyView.childCount - 1);
                    position.y = lastTfm.localPosition.y - height - 5;
                    childTfm.localPosition = position;
                    childTfm.SetAsLastSibling();
                }
            }
        }
        
        private static void ResetBagCards(this PlayViewComponent self)
        {
            LSViewCardBagComponent viewCardBagComponent = self.Room().GetLookPlayerComponent<LSViewCardBagComponent>();
            
            float halfCellWidth = 90;   // 从PlayCardItem预制体中获取卡牌宽度
            float width = self.u_ComCardsRoot.rect.width;
            float itemWidth = Math.Min(halfCellWidth * 2, width / viewCardBagComponent.Items.Count);

            int indexOld = 0;
            while (indexOld < self.CardsView.ItemRenderers.Count)
            {
                var renderer = self.CardsView.ItemRenderers[indexOld];
                var bagItem = viewCardBagComponent.Items.Count > indexOld ? viewCardBagComponent.Items[indexOld] : null;
                if (bagItem == null || renderer.ItemData.Id != bagItem.Id) {
                    self.CardsView.RemoveItemRenderer(indexOld);
                } else {
                    renderer.UIBase.OwnerRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemWidth);
                    renderer.SetPosition(new Vector3(halfCellWidth + itemWidth * indexOld, 0));
                    renderer.ResetSiblingIndex(indexOld);
                    ++indexOld;
                }
            }

            for (int indexNew = indexOld; indexNew < viewCardBagComponent.Items.Count; indexNew++) {
                var bagItem = viewCardBagComponent.Items[indexNew];
                PlayCardItemComponent renderer = self.CardsView.CreateItemRenderer();
                renderer.UIBase.OwnerRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, itemWidth);
                renderer.SetData(bagItem, new Vector3(halfCellWidth + itemWidth * indexNew, 0));
                renderer.ResetSiblingIndex(indexNew);
            }
        }

        private static void ResetSelectCards(this PlayViewComponent self)
        {
            LSViewCardSelectComponent viewCardSelectComponent = self.Room().GetLookPlayerComponent<LSViewCardSelectComponent>();
            
            self.CachedCards = viewCardSelectComponent.CardsQueue;
            self.u_DataSelectCount.SetValue(self.CachedCards.Count.ToString());
            
            // 获得抽卡机会直接打开选择面板
            if (self.CachedCards.Count > 0)
            {
                var panelComponent = YIUIMgrComponent.Inst.GetPanel<LSCardSelectPanelComponent>();
                if (panelComponent == null || !panelComponent.UIBase.ActiveSelf) {
                    YIUIRootComponent yiuiRootComponent = self.Room().GetComponent<YIUIRootComponent>();
                    yiuiRootComponent.OpenPanelAsync<LSCardSelectPanelComponent>().Coroutine();
                }
            }
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
        
        private static void OnEventSelectCardAction(this PlayViewComponent self)
        {
            if (self.CachedCards.Count > 0)
            {
                var panelComponent = YIUIMgrComponent.Inst.GetPanel<LSCardSelectPanelComponent>();
                if (panelComponent == null || !panelComponent.UIBase.ActiveSelf) {
                    YIUIRootComponent yiuiRootComponent = self.Room().GetComponent<YIUIRootComponent>();
                    yiuiRootComponent.OpenPanelAsync<LSCardSelectPanelComponent>().Coroutine();
                }
            }
        }
        
        private static void OnEventSetScale2Action(this PlayViewComponent self)
        {
            var command = LSCommand.GenCommandLong(0, OperateCommandType.TimeScale, FP.Two.V);
            self.Room().SendCommandMeesage(command);
        }
        
        private static void OnEventSetScale1Action(this PlayViewComponent self)
        {
            var command = LSCommand.GenCommandLong(0, OperateCommandType.TimeScale, FP.One.V);
            self.Room().SendCommandMeesage(command);
        }
        
        private static void OnEventSetPauseAction(this PlayViewComponent self)
        {
            var command = LSCommand.GenCommandLong(0, OperateCommandType.TimeScale, 0);
            self.Room().SendCommandMeesage(command);
        }
        #endregion YIUIEvent结束
    }
}