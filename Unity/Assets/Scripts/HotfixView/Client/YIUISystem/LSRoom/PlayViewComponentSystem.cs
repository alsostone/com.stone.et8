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
    [FriendOf(typeof(PlayViewComponent))]
    public static partial class PlayViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this PlayViewComponent self)
        {
        }
        
        [EntitySystem]
        private static void Destroy(this PlayViewComponent self)
        {
        }
        
        [EntitySystem]
        private static void Update(this PlayViewComponent self)
        {
            Room room = self.Room();
            if (self.PredictFrame != room.PredictionFrame)
            {
                self.PredictFrame = room.PredictionFrame;
                self.u_DataPredictFrame.SetValue(room.PredictionFrame.ToString());
            }
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this PlayViewComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }
        
        #region YIUIEvent开始
        
        private static void OnEventSaveNameAction(this PlayViewComponent self, string p1)
        {
            self.SaveName = p1;
        }
        
        private static void OnEventSaveReplayAction(this PlayViewComponent self)
        {
            LSClientHelper.SaveReplay(self.Room(), self.SaveName);
        }
        #endregion YIUIEvent结束
    }
}