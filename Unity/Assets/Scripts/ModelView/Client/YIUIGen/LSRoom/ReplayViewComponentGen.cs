using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{

    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.View)]
    public partial class ReplayViewComponent: Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize, IYIUIOpen
    {
        public const string PkgName = "LSRoom";
        public const string ResName = "ReplayView";

        public EntityRef<YIUIComponent> u_UIBase;
        public YIUIComponent UIBase => u_UIBase;
        public EntityRef<YIUIWindowComponent> u_UIWindow;
        public YIUIWindowComponent UIWindow => u_UIWindow;
        public EntityRef<YIUIViewComponent> u_UIView;
        public YIUIViewComponent UIView => u_UIView;
        public YIUIFramework.UIDataValueString u_DataTotalFrame;
        public YIUIFramework.UIDataValueString u_DataReplaySpeed;
        public YIUIFramework.UIDataValueString u_DataProgress;
        public UIEventP1<string> u_EventJumpToFrame;
        public UIEventHandleP1<string> u_EventJumpToFrameHandle;
        public UIEventP0 u_EventJump;
        public UIEventHandleP0 u_EventJumpHandle;
        public UIEventP0 u_EventReplaySpeed;
        public UIEventHandleP0 u_EventReplaySpeedHandle;

    }
}