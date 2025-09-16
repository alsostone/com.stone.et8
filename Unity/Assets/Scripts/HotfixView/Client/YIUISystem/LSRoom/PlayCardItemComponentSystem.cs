using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;
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

        public static void ResetItem(this PlayCardItemComponent self, Tuple<EUnitType, int, int> data)
        {
            self.Data = data;
            switch (data.Item1)
            {
                case EUnitType.Block:
                    self.u_DataName.SetValue($"Block{data.Item2}");
                    break;
                case EUnitType.Building:
                    self.u_DataName.SetValue($"Building{data.Item2}");
                    break;
            }
        }
        
        #region YIUIEvent开始
        
        private static void OnEventDragEndAction(this PlayCardItemComponent self, object p1)
        {
            PointerEventData eventData = p1 as PointerEventData;
            
        }
        
        private static void OnEventDragAction(this PlayCardItemComponent self, object p1)
        {
            PointerEventData eventData = p1 as PointerEventData;
            
        }
        
        private static void OnEventDragBeginAction(this PlayCardItemComponent self, object p1)
        {
            PointerEventData eventData = p1 as PointerEventData;
            
        }
        
        private static void OnEventClickAction(this PlayCardItemComponent self)
        {
            Room room = self.Room();
            LSOperaDragComponent dragComponent = room.GetComponent<LSOperaDragComponent>();
            dragComponent.SetPlacementObject(self.Data.Item1, self.Data.Item2);
        }
        #endregion YIUIEvent结束
    }
}