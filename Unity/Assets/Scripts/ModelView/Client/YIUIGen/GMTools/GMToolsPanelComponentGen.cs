using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// 当前Panel所有可用view枚举
    /// </summary>
    public enum EGMToolsPanelViewEnum
    {
        GMToolsStageView = 1,
        GMToolsBatchView = 2,
    }
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.Panel, EPanelLayer.Tips)]
    public partial class GMToolsPanelComponent: Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize, IYIUIOpen
    {
        public const string PkgName = "GMTools";
        public const string ResName = "GMToolsPanel";

        public EntityRef<YIUIComponent> u_UIBase;
        public YIUIComponent UIBase => u_UIBase;
        public EntityRef<YIUIWindowComponent> u_UIWindow;
        public YIUIWindowComponent UIWindow => u_UIWindow;
        public EntityRef<YIUIPanelComponent> u_UIPanel;
        public YIUIPanelComponent UIPanel => u_UIPanel;
        public YIUIFramework.UIDataValueBool u_DataIsBattle;
        public UIEventP0 u_EventOpenStage;
        public UIEventHandleP0 u_EventOpenStageHandle;
        public UIEventP0 u_EventOpenBatch;
        public UIEventHandleP0 u_EventOpenBatchHandle;
        public UIEventP0 u_EventOpenGmTools;
        public UIEventHandleP0 u_EventOpenGmToolsHandle;
        public UIEventP0 u_EventSetWin;
        public UIEventHandleP0 u_EventSetWinHandle;
        public UIEventP0 u_EventSetLose;
        public UIEventHandleP0 u_EventSetLoseHandle;
        public UIEventP0 u_EventCardSelect;
        public UIEventHandleP0 u_EventCardSelectHandle;

    }
}