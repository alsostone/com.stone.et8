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
    [EntitySystemOf(typeof(PlayCardItemComponent))]
    public static partial class PlayCardItemComponentSystem
    {
        [EntitySystem]
        private static void Awake(this PlayCardItemComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this PlayCardItemComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this PlayCardItemComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIComponent>();


        }
    }
}