using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  YIUI
    /// Date    2025.9.8
    /// Desc
    /// </summary>
    [FriendOf(typeof(GMToolsBatchViewComponent))]
    public static partial class GMToolsBatchViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this GMToolsBatchViewComponent self)
        {
        }
        
        [EntitySystem]
        private static void Destroy(this GMToolsBatchViewComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this GMToolsBatchViewComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }
        
        #region YIUIEvent开始
        
        private static void OnEventCloseAction(this GMToolsBatchViewComponent self)
        {
            self.UIView.Close();
        }
        #endregion YIUIEvent结束
    }
}