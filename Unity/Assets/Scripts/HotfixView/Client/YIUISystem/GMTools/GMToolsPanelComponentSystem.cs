using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(GMToolsPanelComponent))]
    public static partial class GMToolsPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this GMToolsPanelComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this GMToolsPanelComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this GMToolsPanelComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }
        
        #region YIUIEvent开始
        
        private static void OnEventSetLoseAction(this GMToolsPanelComponent self)
        {
            
        }
        
        private static void OnEventSetWinAction(this GMToolsPanelComponent self)
        {
            
        }
        
        private static void OnEventOpenGmToolsAction(this GMToolsPanelComponent self)
        {
            
        }
        
        private static void OnEventOpenBatchAction(this GMToolsPanelComponent self)
        {
            
        }
        
        private static void OnEventOpenStageAction(this GMToolsPanelComponent self)
        {
            
        }
        #endregion YIUIEvent结束
    }
}