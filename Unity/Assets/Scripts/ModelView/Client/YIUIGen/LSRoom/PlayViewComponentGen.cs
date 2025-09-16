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
    public partial class PlayViewComponent: Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize, IYIUIOpen
    {
        public const string PkgName = "LSRoom";
        public const string ResName = "PlayView";

        public EntityRef<YIUIComponent> u_UIBase;
        public YIUIComponent UIBase => u_UIBase;
        public EntityRef<YIUIWindowComponent> u_UIWindow;
        public YIUIWindowComponent UIWindow => u_UIWindow;
        public EntityRef<YIUIViewComponent> u_UIView;
        public YIUIViewComponent UIView => u_UIView;
        public UnityEngine.UI.LoopHorizontalScrollRect u_ComCardsLoop;
        public YIUIFramework.UIDataValueString u_DataPredictFrame;
        public YIUIFramework.UIDataValueString u_DataSelectCount;
        public UIEventP0 u_EventSaveReplay;
        public UIEventHandleP0 u_EventSaveReplayHandle;
        public UIEventP1<string> u_EventSaveName;
        public UIEventHandleP1<string> u_EventSaveNameHandle;
        public UIEventP0 u_EventTestMove;
        public UIEventHandleP0 u_EventTestMoveHandle;
        public UIEventP0 u_EventSelectCard;
        public UIEventHandleP0 u_EventSelectCardHandle;

    }
}