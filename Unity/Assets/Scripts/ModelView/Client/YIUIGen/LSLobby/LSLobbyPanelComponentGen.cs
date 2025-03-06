using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{

    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.Panel, EPanelLayer.Panel)]
    public partial class LSLobbyPanelComponent: Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize, IYIUIOpen
    {
        public const string PkgName = "LSLobby";
        public const string ResName = "LSLobbyPanel";

        public EntityRef<YIUIComponent> u_UIBase;
        public YIUIComponent UIBase => u_UIBase;
        public EntityRef<YIUIWindowComponent> u_UIWindow;
        public YIUIWindowComponent UIWindow => u_UIWindow;
        public EntityRef<YIUIPanelComponent> u_UIPanel;
        public YIUIPanelComponent UIPanel => u_UIPanel;
        public UIEventP0 u_EventReplay;
        public UIEventHandleP0 u_EventReplayHandle;
        public UIEventP0 u_EventEnterMap;
        public UIEventHandleP0 u_EventEnterMapHandle;
        public UIEventP1<string> u_EventReplayPath;
        public UIEventHandleP1<string> u_EventReplayPathHandle;

    }
}