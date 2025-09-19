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

        public static void SetData(this PlayCardItemComponent self, CardBagItem itemData, Vector3 position)
        {
            self.ItemData = itemData;
            switch (itemData.Type)
            {
                case EUnitType.Block:
                    self.u_DataName.SetValue($"Block{itemData.TableId}");
                    break;
                case EUnitType.Building:
                    self.u_DataName.SetValue($"Building{itemData.TableId}");
                    break;
            }
            self.SetPosition(position);
        }
        
        public static void SetPosition(this PlayCardItemComponent self, Vector3 position)
        {
            self.UIBase.OwnerRectTransform.DOLocalMove(position, 0.2f);
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
        }
        
        private static void OnEventClickAction(this PlayCardItemComponent self)
        {
            Room room = self.Room();
            LSOperaDragComponent dragComponent = room.GetComponent<LSOperaDragComponent>();
            dragComponent.SetPlacementObject(self.ItemData.Id, false);
        }
        
        #endregion YIUIEvent结束
    }
}