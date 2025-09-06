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
#if ENABLE_DEBUG
            var quickBehaviour = self.ViewGO.GetComponent<ST.GmTools.GmToolsBehaviour>();
            if (quickBehaviour == null)
                quickBehaviour = self.ViewGO.AddComponent<ST.GmTools.GmToolsBehaviour>();
            quickBehaviour.enabled = true;
#endif
        }
        
        private static void OnEventOpenBatchAction(this GMToolsPanelComponent self)
        {
            
        }
        
        private static void OnEventOpenStageAction(this GMToolsPanelComponent self)
        {
#if ENABLE_DEBUG
            var quickBehaviour = self.ViewGO.GetComponent<ST.GmTools.GmToolsQuickBehaviour>();
            if (quickBehaviour == null)
            {
                quickBehaviour = self.ViewGO.AddComponent<ST.GmTools.GmToolsQuickBehaviour>();
                quickBehaviour.SetQuickView(new ST.GmTools.DrawViewStage());
            }
            quickBehaviour.enabled = true;
#endif
        }
        #endregion YIUIEvent结束
    }
}