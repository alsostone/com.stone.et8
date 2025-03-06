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
    [EntitySystemOf(typeof(ReplayViewComponent))]
    public static partial class ReplayViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ReplayViewComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this ReplayViewComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this ReplayViewComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIComponent>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIView = self.UIBase.GetComponent<YIUIViewComponent>();
            self.UIWindow.WindowOption = EWindowOption.None;
            self.UIView.ViewWindowType = EViewWindowType.View;
            self.UIView.StackOption = EViewStackOption.VisibleTween;

            self.u_DataTotalFrame = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueString>("u_DataTotalFrame");
            self.u_DataReplaySpeed = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueString>("u_DataReplaySpeed");
            self.u_DataProgress = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueString>("u_DataProgress");
            self.u_EventJumpToFrame = self.UIBase.EventTable.FindEvent<UIEventP1<string>>("u_EventJumpToFrame");
            self.u_EventJumpToFrameHandle = self.u_EventJumpToFrame.Add(self.OnEventJumpToFrameAction);
            self.u_EventJump = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventJump");
            self.u_EventJumpHandle = self.u_EventJump.Add(self.OnEventJumpAction);
            self.u_EventReplaySpeed = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventReplaySpeed");
            self.u_EventReplaySpeedHandle = self.u_EventReplaySpeed.Add(self.OnEventReplaySpeedAction);

        }
    }
}