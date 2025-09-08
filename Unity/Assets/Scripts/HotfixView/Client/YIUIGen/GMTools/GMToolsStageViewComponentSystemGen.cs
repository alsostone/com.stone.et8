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
    [EntitySystemOf(typeof(GMToolsStageViewComponent))]
    public static partial class GMToolsStageViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this GMToolsStageViewComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this GMToolsStageViewComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this GMToolsStageViewComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIComponent>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIView = self.UIBase.GetComponent<YIUIViewComponent>();
            self.UIWindow.WindowOption = EWindowOption.BanTween|EWindowOption.BanAwaitOpenTween|EWindowOption.BanAwaitCloseTween|EWindowOption.SkipOtherOpenTween|EWindowOption.SkipOtherCloseTween;
            self.UIView.ViewWindowType = EViewWindowType.View;
            self.UIView.StackOption = EViewStackOption.None;

            self.u_EventClose = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventClose");
            self.u_EventCloseHandle = self.u_EventClose.Add(self.OnEventCloseAction);
            self.u_EventPveStart = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventPveStart");
            self.u_EventPveStartHandle = self.u_EventPveStart.Add(self.OnEventPveStartAction);
            self.u_EventPvpStart = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventPvpStart");
            self.u_EventPvpStartHandle = self.u_EventPvpStart.Add(self.OnEventPvpStartAction);

        }
    }
}