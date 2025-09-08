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
    [FriendOf(typeof(GMToolsStageViewComponent))]
    public static partial class GMToolsStageViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this GMToolsStageViewComponent self)
        {
        }
        
        [EntitySystem]
        private static void Destroy(this GMToolsStageViewComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this GMToolsStageViewComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }
        
        #region YIUIEvent开始
        
        private static void OnEventPvpStartAction(this GMToolsStageViewComponent self)
        {
            
        }
        
        private static void OnEventPveStartAction(this GMToolsStageViewComponent self)
        {
            
        }
        
        private static void OnEventCloseAction(this GMToolsStageViewComponent self)
        {
            self.UIView.Close();
        }
        #endregion YIUIEvent结束
    }
}