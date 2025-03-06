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
    [FriendOf(typeof(YIUIPanelComponent))]
    [EntitySystemOf(typeof(LSLobbyPanelComponent))]
    public static partial class LSLobbyPanelComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSLobbyPanelComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this LSLobbyPanelComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this LSLobbyPanelComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIComponent>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIPanel = self.UIBase.GetComponent<YIUIPanelComponent>();
            self.UIWindow.WindowOption = EWindowOption.None;
            self.UIPanel.Layer = EPanelLayer.Panel;
            self.UIPanel.PanelOption = EPanelOption.TimeCache;
            self.UIPanel.StackOption = EPanelStackOption.VisibleTween;
            self.UIPanel.Priority = 0;
            self.UIPanel.CachePanelTime = 10;

            self.u_EventReplay = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventReplay");
            self.u_EventReplayHandle = self.u_EventReplay.Add(self.OnEventReplayAction);
            self.u_EventEnterMap = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventEnterMap");
            self.u_EventEnterMapHandle = self.u_EventEnterMap.Add(self.OnEventEnterMapAction);
            self.u_EventReplayPath = self.UIBase.EventTable.FindEvent<UIEventP1<string>>("u_EventReplayPath");
            self.u_EventReplayPathHandle = self.u_EventReplayPath.Add(self.OnEventReplayPathAction);

        }
    }
}