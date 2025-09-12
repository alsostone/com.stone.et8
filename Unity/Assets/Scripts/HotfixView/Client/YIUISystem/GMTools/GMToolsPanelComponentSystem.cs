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
        
        [EntitySystem]
        private static void Update(this GMToolsPanelComponent self)
        {
            var room = self.Root().GetComponent<Room>();
            self.u_DataIsBattle.SetValue(room != null);
        }
        
        #region YIUIEvent开始
        
        private static void OnEventSetLoseAction(this GMToolsPanelComponent self)
        {
#if ENABLE_DEBUG
            var room = self.Root().GetComponent<Room>();
            if (room != null)
            {
                ulong cmd = LSCommand.GenCommandGm(0, CommandGMType.Failure);
                room.SendCommandMeesage(cmd);
            }
#endif
        }
        
        private static void OnEventSetWinAction(this GMToolsPanelComponent self)
        {
#if ENABLE_DEBUG
            var room = self.Root().GetComponent<Room>();
            if (room != null)
            {
                ulong cmd = LSCommand.GenCommandGm(0, CommandGMType.Victory);
                room.SendCommandMeesage(cmd);
            }
#endif
        }
        
        private static void OnEventOpenGmToolsAction(this GMToolsPanelComponent self)
        {
            Log.Debug("还未支持，敬请期待");
        }
        
        private static void OnEventOpenBatchAction(this GMToolsPanelComponent self)
        {
            self.UIPanel.OpenViewAsync<GMToolsBatchViewComponent>().Coroutine();
        }
        
        private static void OnEventOpenStageAction(this GMToolsPanelComponent self)
        {
            self.UIPanel.OpenViewAsync<GMToolsStageViewComponent>().Coroutine();
        }
        #endregion YIUIEvent结束
    }
}