﻿using System;
using System.Collections.Generic;
using ET;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace YIUIFramework
{
    /// <summary>
    /// 点击事件绑定
    /// 与按钮无关
    /// 只要是任何可以被射线检测的物体都可以响应点击事件
    /// 额外多一个String 参数自定义
    /// </summary>
    [InfoBox("提示: 可用事件参数 1个")]
    [LabelText("点击<String>")]
    [AddComponentMenu("YIUIBind/TaskEvent/点击 【Click String】 UITaskEventBindClickString")]
    public class UITaskEventBindClickString: UITaskEventBindClick
    {
        [SerializeField]
        [LabelText("额外string参数值")]
        private string m_ExtraParam;

        [NonSerialized]
        private List<EUIEventParamType> m_FilterParamType = new List<EUIEventParamType> { EUIEventParamType.String };

        protected override List<EUIEventParamType> GetFilterParamType => m_FilterParamType;

        protected override async ETTask OnUIEvent(PointerEventData eventData)
        {
            await m_UIEvent?.InvokeAsync(m_ExtraParam);
        }
    }
}