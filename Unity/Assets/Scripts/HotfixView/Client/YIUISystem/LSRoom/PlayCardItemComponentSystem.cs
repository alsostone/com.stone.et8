using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

namespace ET.Client
{
    /// <summary>
    /// Author  YIUI
    /// Date    2025.9.16
    /// Desc
    /// </summary>
    [FriendOf(typeof(PlayCardItemComponent))]
    public static partial class PlayCardItemComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this PlayCardItemComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this PlayCardItemComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask YIUIEvent(this PlayCardItemComponent self, OnCardItemHighlightEvent message)
        {
            await ETTask.CompletedTask;
            self.SetHighlight(message.ItemId == self.ItemData.Id);
        }
        
        public static void SetData(this PlayCardItemComponent self, CardBagItem itemData, Vector3 position)
        {
            self.ItemData = itemData;
            self.Position = position;
            
            switch (itemData.Type)
            {
                case EUnitType.Block:
                    self.u_DataName.SetValue($"Block{itemData.TableId}");
                    break;
                case EUnitType.Building:
                    self.u_DataName.SetValue($"Building{itemData.TableId}");
                    break;
            }
            self.UIBase.OwnerRectTransform.localPosition = position + new Vector3(0, 30);
            self.SetPosition(position);
        }
        
        public static void SetPosition(this PlayCardItemComponent self, Vector3 position)
        {
            self.Position = position;
            self.UIBase.OwnerRectTransform.DOLocalMove(position, 0.25f);
        }
        
        private static void SetHighlight(this PlayCardItemComponent self, bool highlight)
        {
            if (self.IsHighlight == highlight)
                return;
            
            self.IsHighlight = highlight;
            if (highlight) {
                self.u_ComCardRoot.DOLocalMoveY(30, 0.25f);
                // 在此处可以发送选中该卡牌的指令给服务器，用来增加氛围感
                // ...
            }
            else {
                self.u_ComCardRoot.DOLocalMoveY(0, 0.25f);
            }
        }
        
        #region YIUIEvent开始
        private static void OnEventDragBeginAction(this PlayCardItemComponent self, object p1)
        {
            PointerEventData eventData = p1 as PointerEventData;
            self.PointerId = eventData.pointerId;
            
            Room room = self.Room();
            LSOperaDragComponent dragComponent = room.GetComponent<LSOperaDragComponent>();
            dragComponent.SetPlacementObject(self.ItemData.Id, true);
            dragComponent.OnTouchMove(eventData.position);
        }

        private static void OnEventDragAction(this PlayCardItemComponent self, object p1)
        {
            PointerEventData eventData = p1 as PointerEventData;
            if (self.PointerId != eventData.pointerId)
                return;
            
            Room room = self.Room();
            LSOperaDragComponent dragComponent = room.GetComponent<LSOperaDragComponent>();
            dragComponent.OnTouchMove(eventData.position);
        }
        
        private static void OnEventDragEndAction(this PlayCardItemComponent self, object p1)
        {
            PointerEventData eventData = p1 as PointerEventData;
            if (self.PointerId != eventData.pointerId)
                return;
            
            Room room = self.Room();
            LSOperaDragComponent dragComponent = room.GetComponent<LSOperaDragComponent>();
            dragComponent.OnTouchEnd(eventData.position);
            self.PointerId = -1;
        }
        
        private static void OnEventClickEnterAction(this PlayCardItemComponent self)
        {
            self.IsCliickEnter = true;
            
            // 只要处于拖动中就不改变高亮状态（不论这个拖动是不是本Item发起的）
            LSOperaDragComponent dragComponent = self.Room().GetComponent<LSOperaDragComponent>();
            if (!dragComponent.isDraging) {
                self.Fiber().UIEvent(new OnCardItemHighlightEvent() { ItemId = self.ItemData.Id }).Coroutine();
            }
        }

        private static void OnEventClickDownAction(this PlayCardItemComponent self)
        {
            self.IsClickPress = false;
        }
        
        private static void OnEventClickUpAction(this PlayCardItemComponent self)
        {
            // 当鼠标还在该Item内时 不取消高亮
            if (!self.IsCliickEnter) {
                self.SetHighlight(false);
            }
        }
        
        private static void OnEventClickExitAction(this PlayCardItemComponent self)
        {
            self.IsCliickEnter = false;
            
            // 只要处于拖动中就不改变高亮状态（不论这个拖动是不是本Item发起的）
            LSOperaDragComponent dragComponent = self.Room().GetComponent<LSOperaDragComponent>();
            if (!dragComponent.isDraging) {
                self.SetHighlight(false);
            }
        }

        private static void OnEventPressAction(this PlayCardItemComponent self)
        {
            // 拖动开始后 不响应长按事件（面板未支持配置，这么判断即可）
            if (self.PointerId == -1) {
                // 在此处可以做长按展示卡牌详情的功能
                // TipsHelper.OpenSync<TextTipsViewComponent>("no code");
                
                // 若是长按生效后 则不响应点击事件
                // self.IsClickPress = true;
            }
        }

        private static void OnEventClickAction(this PlayCardItemComponent self)
        {
            // 长按生效后 不响应点击事件（拖动时是否响应由面板配置）
            if (!self.IsClickPress) {
                Room room = self.Room();
                LSOperaDragComponent dragComponent = room.GetComponent<LSOperaDragComponent>();
                dragComponent.SetPlacementObject(self.ItemData.Id, false);
                self.SetHighlight(true);
            }
        }
        
        #endregion YIUIEvent结束
    }
}