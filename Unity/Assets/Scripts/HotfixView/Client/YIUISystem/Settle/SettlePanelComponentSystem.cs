using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  YIUI
    /// Date    2025.9.11
    /// Desc
    /// </summary>
    [FriendOf(typeof(SettlePanelComponent))]
    public static partial class SettlePanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this SettlePanelComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this SettlePanelComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this SettlePanelComponent self)
        {
            self.UIPanel.OpenViewAsync<SettleStatViewComponent>().Coroutine();
            await ETTask.CompletedTask;
            return true;
        }
        
        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}