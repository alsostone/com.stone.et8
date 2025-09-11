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
    [FriendOf(typeof(YIUIWindowComponent))]
    [FriendOf(typeof(YIUIViewComponent))]
    [EntitySystemOf(typeof(SettleStatViewComponent))]
    public static partial class SettleStatViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SettleStatViewComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this SettleStatViewComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this SettleStatViewComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIComponent>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIView = self.UIBase.GetComponent<YIUIViewComponent>();
            self.UIWindow.WindowOption = EWindowOption.BanTween|EWindowOption.BanAwaitOpenTween|EWindowOption.BanAwaitCloseTween|EWindowOption.SkipOtherOpenTween|EWindowOption.SkipOtherCloseTween;
            self.UIView.ViewWindowType = EViewWindowType.View;
            self.UIView.StackOption = EViewStackOption.None;

            self.u_DataIsWin = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueBool>("u_DataIsWin");
            self.u_EventBackLast = self.UIBase.EventTable.FindEvent<UITaskEventP0>("u_EventBackLast");
            self.u_EventBackLastHandle = self.u_EventBackLast.Add(self.OnEventBackLastAction);

        }
    }
}