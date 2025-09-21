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
    public partial class LSCardSelectItemComponent: Entity, IDestroy, IAwake, IYIUIBind, IYIUIInitialize
    {
        public const string PkgName = "LSCardSelect";
        public const string ResName = "LSCardSelectItem";

        public EntityRef<YIUIComponent> u_UIBase;
        public YIUIComponent UIBase => u_UIBase;
        public YIUIFramework.UIDataValueString u_DataName;
        public UIEventP0 u_EventClick;
        public UIEventHandleP0 u_EventClickHandle;

    }
}