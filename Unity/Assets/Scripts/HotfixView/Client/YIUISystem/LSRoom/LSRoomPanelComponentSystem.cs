using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  YIUI
    /// Date    2025.3.6
    /// Desc
    /// </summary>
    [FriendOf(typeof(LSRoomPanelComponent))]
    public static partial class LSRoomPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this LSRoomPanelComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this LSRoomPanelComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this LSRoomPanelComponent self, ELSRoomPanelViewEnum index)
        {
            await self.UIPanel.OpenViewAsync(index.ToString());
            return true;
        }
        
        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}