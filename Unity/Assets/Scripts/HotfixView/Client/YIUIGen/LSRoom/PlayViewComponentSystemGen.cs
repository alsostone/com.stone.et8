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
    [EntitySystemOf(typeof(PlayViewComponent))]
    public static partial class PlayViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this PlayViewComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this PlayViewComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this PlayViewComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIComponent>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIView = self.UIBase.GetComponent<YIUIViewComponent>();
            self.UIWindow.WindowOption = EWindowOption.None;
            self.UIView.ViewWindowType = EViewWindowType.View;
            self.UIView.StackOption = EViewStackOption.VisibleTween;

            self.u_DataPredictFrame = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueString>("u_DataPredictFrame");
            self.u_EventSaveReplay = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventSaveReplay");
            self.u_EventSaveReplayHandle = self.u_EventSaveReplay.Add(self.OnEventSaveReplayAction);
            self.u_EventSaveName = self.UIBase.EventTable.FindEvent<UIEventP1<string>>("u_EventSaveName");
            self.u_EventSaveNameHandle = self.u_EventSaveName.Add(self.OnEventSaveNameAction);

        }
    }
}