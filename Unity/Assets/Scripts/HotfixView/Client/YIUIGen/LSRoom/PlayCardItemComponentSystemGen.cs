using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [FriendOf(typeof(YIUIComponent))]
    [EntitySystemOf(typeof(PlayCardItemComponent))]
    public static partial class PlayCardItemComponentSystem
    {
        [EntitySystem]
        private static void Awake(this PlayCardItemComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this PlayCardItemComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this PlayCardItemComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIComponent>();

            self.u_DataName = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueString>("u_DataName");
            self.u_EventClick = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventClick");
            self.u_EventClickHandle = self.u_EventClick.Add(self.OnEventClickAction);
            self.u_EventDragBegin = self.UIBase.EventTable.FindEvent<UIEventP1<object>>("u_EventDragBegin");
            self.u_EventDragBeginHandle = self.u_EventDragBegin.Add(self.OnEventDragBeginAction);
            self.u_EventDrag = self.UIBase.EventTable.FindEvent<UIEventP1<object>>("u_EventDrag");
            self.u_EventDragHandle = self.u_EventDrag.Add(self.OnEventDragAction);
            self.u_EventDragEnd = self.UIBase.EventTable.FindEvent<UIEventP1<object>>("u_EventDragEnd");
            self.u_EventDragEndHandle = self.u_EventDragEnd.Add(self.OnEventDragEndAction);

        }
    }
}