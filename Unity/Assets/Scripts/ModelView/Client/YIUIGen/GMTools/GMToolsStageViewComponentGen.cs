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
    public partial class GMToolsStageViewComponent: Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize, IYIUIOpen
    {
        public const string PkgName = "GMTools";
        public const string ResName = "GMToolsStageView";

        public EntityRef<YIUIComponent> u_UIBase;
        public YIUIComponent UIBase => u_UIBase;
        public EntityRef<YIUIWindowComponent> u_UIWindow;
        public YIUIWindowComponent UIWindow => u_UIWindow;
        public EntityRef<YIUIViewComponent> u_UIView;
        public YIUIViewComponent UIView => u_UIView;
        public TMPro.TMP_Dropdown u_ComFightMode;
        public UnityEngine.UI.LoopVerticalScrollRect u_ComStageLoop;
        public UnityEngine.UI.LoopVerticalScrollRect u_ComStageLoopSel;
        public UnityEngine.UI.LoopVerticalScrollRect u_ComHeroLoopSel;
        public UnityEngine.UI.LoopVerticalScrollRect u_ComHeroLoop;
        public YIUIFramework.UIDataValueString u_DataTips;
        public YIUIFramework.UIDataValueBool u_DataCanStart;
        public YIUIFramework.UIDataValueBool u_DataCanMatch;
        public UIEventP0 u_EventClose;
        public UIEventHandleP0 u_EventCloseHandle;
        public UIEventP0 u_EventPveStart;
        public UIEventHandleP0 u_EventPveStartHandle;
        public UIEventP0 u_EventPvpStart;
        public UIEventHandleP0 u_EventPvpStartHandle;
        public UIEventP1<int> u_EventFightMode;
        public UIEventHandleP1<int> u_EventFightModeHandle;

    }
}