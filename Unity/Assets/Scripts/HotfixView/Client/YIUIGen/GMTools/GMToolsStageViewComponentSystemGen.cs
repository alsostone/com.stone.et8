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
            self.UIView.StackOption = EViewStackOption.Visible;

            self.u_ComFightMode = self.UIBase.ComponentTable.FindComponent<TMPro.TMP_Dropdown>("u_ComFightMode");
            self.u_ComStageLoop = self.UIBase.ComponentTable.FindComponent<UnityEngine.UI.LoopVerticalScrollRect>("u_ComStageLoop");
            self.u_ComStageLoopSel = self.UIBase.ComponentTable.FindComponent<UnityEngine.UI.LoopVerticalScrollRect>("u_ComStageLoopSel");
            self.u_ComHeroLoopSel = self.UIBase.ComponentTable.FindComponent<UnityEngine.UI.LoopVerticalScrollRect>("u_ComHeroLoopSel");
            self.u_ComHeroLoop = self.UIBase.ComponentTable.FindComponent<UnityEngine.UI.LoopVerticalScrollRect>("u_ComHeroLoop");
            self.u_DataTips = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueString>("u_DataTips");
            self.u_DataCanStart = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueBool>("u_DataCanStart");
            self.u_DataCanMatch = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueBool>("u_DataCanMatch");
            self.u_EventClose = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventClose");
            self.u_EventCloseHandle = self.u_EventClose.Add(self.OnEventCloseAction);
            self.u_EventPveStart = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventPveStart");
            self.u_EventPveStartHandle = self.u_EventPveStart.Add(self.OnEventPveStartAction);
            self.u_EventPvpStart = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventPvpStart");
            self.u_EventPvpStartHandle = self.u_EventPvpStart.Add(self.OnEventPvpStartAction);
            self.u_EventFightMode = self.UIBase.EventTable.FindEvent<UIEventP1<int>>("u_EventFightMode");
            self.u_EventFightModeHandle = self.u_EventFightMode.Add(self.OnEventFightModeAction);

        }
    }
}