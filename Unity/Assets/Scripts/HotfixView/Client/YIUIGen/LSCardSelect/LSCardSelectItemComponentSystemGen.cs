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
    [EntitySystemOf(typeof(LSCardSelectItemComponent))]
    public static partial class LSCardSelectItemComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSCardSelectItemComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this LSCardSelectItemComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this LSCardSelectItemComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIComponent>();

            self.u_DataName = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueString>("u_DataName");
            self.u_EventClick = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventClick");
            self.u_EventClickHandle = self.u_EventClick.Add(self.OnEventClickAction);

        }
    }
}