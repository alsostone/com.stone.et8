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
    [EntitySystemOf(typeof(LSCardSelectPanelComponent))]
    public static partial class LSCardSelectPanelComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSCardSelectPanelComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this LSCardSelectPanelComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this LSCardSelectPanelComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIComponent>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIPanel = self.UIBase.GetComponent<YIUIPanelComponent>();
            self.UIWindow.WindowOption = EWindowOption.None;
            self.UIPanel.Layer = EPanelLayer.Popup;
            self.UIPanel.PanelOption = EPanelOption.TimeCache;
            self.UIPanel.StackOption = EPanelStackOption.VisibleTween;
            self.UIPanel.Priority = 0;
            self.UIPanel.CachePanelTime = 10;

            self.u_ComCardSelectRoot = self.UIBase.ComponentTable.FindComponent<UnityEngine.RectTransform>("u_ComCardSelectRoot");

        }
    }
}