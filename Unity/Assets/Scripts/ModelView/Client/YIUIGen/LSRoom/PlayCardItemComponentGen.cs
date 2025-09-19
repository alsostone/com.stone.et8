using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{

    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [YIUI(EUICodeType.Common)]
    public partial class PlayCardItemComponent: Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize
    {
        public const string PkgName = "LSRoom";
        public const string ResName = "PlayCardItem";

        public EntityRef<YIUIComponent> u_UIBase;
        public YIUIComponent UIBase => u_UIBase;
        public UnityEngine.RectTransform u_ComCardRoot;
        public YIUIFramework.UIDataValueString u_DataName;
        public UIEventP0 u_EventClick;
        public UIEventHandleP0 u_EventClickHandle;
        public UIEventP1<object> u_EventDragBegin;
        public UIEventHandleP1<object> u_EventDragBeginHandle;
        public UIEventP1<object> u_EventDrag;
        public UIEventHandleP1<object> u_EventDragHandle;
        public UIEventP1<object> u_EventDragEnd;
        public UIEventHandleP1<object> u_EventDragEndHandle;
        public UIEventP0 u_EventPress;
        public UIEventHandleP0 u_EventPressHandle;
        public UIEventP0 u_EventClickDown;
        public UIEventHandleP0 u_EventClickDownHandle;
        public UIEventP0 u_EventClickUp;
        public UIEventHandleP0 u_EventClickUpHandle;
        public UIEventP0 u_EventClickEnter;
        public UIEventHandleP0 u_EventClickEnterHandle;
        public UIEventP0 u_EventClickExit;
        public UIEventHandleP0 u_EventClickExitHandle;

    }
}