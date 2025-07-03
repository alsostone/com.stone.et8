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
    [FriendOf(typeof(ReplayViewComponent))]
    public static partial class ReplayViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this ReplayViewComponent self)
        {
        }
        
        [EntitySystem]
        private static void Destroy(this ReplayViewComponent self)
        {
        }
        
        [EntitySystem]
        private static void Update(this ReplayViewComponent self)
        {
            Room room = self.Room();
            if (self.frame != room.AuthorityFrame)
            {
                self.frame = room.AuthorityFrame;
                self.u_DataProgress.SetValue(room.AuthorityFrame.ToString());
            }
        }
        
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this ReplayViewComponent self)
        {
            self.u_DataTotalFrame.SetValue(self.Room().Replay.FrameMessages.Count.ToString());
            
            await ETTask.CompletedTask;
            return true;
        }
        
        #region YIUIEvent开始
        
        private static void OnEventReplaySpeedAction(this ReplayViewComponent self)
        {
            LSReplayUpdater lsReplayUpdater = self.Room().GetComponent<LSReplayUpdater>();
            lsReplayUpdater.ChangeReplaySpeed();
            self.u_DataReplaySpeed.SetValue($"X{lsReplayUpdater.ReplaySpeed}");
        }
        
        private static void OnEventJumpAction(this ReplayViewComponent self)
        {
            LSClientHelper.JumpReplay(self.Room(), self.JumpToFrame);
        }
        
        private static void OnEventJumpToFrameAction(this ReplayViewComponent self, string p1)
        {
            self.JumpToFrame = int.Parse(p1);
        }
        #endregion YIUIEvent结束
    }
}