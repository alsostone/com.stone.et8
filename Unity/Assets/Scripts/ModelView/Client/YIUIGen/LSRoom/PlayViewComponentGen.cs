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
        public UnityEngine.RectTransform u_ComCardsRoot;
        public UnityEngine.CanvasGroup u_ComAlphaGroup;
        public UnityEngine.RectTransform u_ComArrowIndicator;
        public UnityEngine.RectTransform u_ComArrowBody;
        public UnityEngine.RectTransform u_ComArrowBodyView;
        public UnityEngine.RectTransform u_ComSelectIndicator;
        public YIUIFramework.UIDataValueString u_DataSelectCount;
        public YIUIFramework.UIDataValueBool u_DataLocalMode;
        public UIEventP0 u_EventSelectCard;
        public UIEventHandleP0 u_EventSelectCardHandle;
        public UIEventP0 u_EventSetPause;
        public UIEventHandleP0 u_EventSetPauseHandle;
        public UIEventP0 u_EventSetScale1;
        public UIEventHandleP0 u_EventSetScale1Handle;
        public UIEventP0 u_EventSetScale2;
        public UIEventHandleP0 u_EventSetScale2Handle;

    }
}