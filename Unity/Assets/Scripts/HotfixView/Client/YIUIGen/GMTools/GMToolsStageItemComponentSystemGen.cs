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
    [EntitySystemOf(typeof(GMToolsStageItemComponent))]
    public static partial class GMToolsStageItemComponentSystem
    {
        [EntitySystem]
        private static void Awake(this GMToolsStageItemComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this GMToolsStageItemComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this GMToolsStageItemComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIComponent>();

            self.u_DataText = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueString>("u_DataText");
            self.u_DataSelected = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueBool>("u_DataSelected");
            self.u_EventClick = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventClick");
            self.u_EventClickHandle = self.u_EventClick.Add(self.OnEventClickAction);

        }
    }
}