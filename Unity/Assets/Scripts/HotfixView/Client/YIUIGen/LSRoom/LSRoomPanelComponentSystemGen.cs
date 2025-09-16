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
    [EntitySystemOf(typeof(LSRoomPanelComponent))]
    public static partial class LSRoomPanelComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSRoomPanelComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this LSRoomPanelComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this LSRoomPanelComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIComponent>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIPanel = self.UIBase.GetComponent<YIUIPanelComponent>();
            self.UIWindow.WindowOption = EWindowOption.BanTween|EWindowOption.BanAwaitOpenTween|EWindowOption.BanAwaitCloseTween|EWindowOption.SkipOtherOpenTween|EWindowOption.SkipOtherCloseTween;
            self.UIPanel.Layer = EPanelLayer.Panel;
            self.UIPanel.PanelOption = EPanelOption.TimeCache;
            self.UIPanel.StackOption = EPanelStackOption.Visible;
            self.UIPanel.Priority = 0;
            self.UIPanel.CachePanelTime = 10;


        }
    }
}