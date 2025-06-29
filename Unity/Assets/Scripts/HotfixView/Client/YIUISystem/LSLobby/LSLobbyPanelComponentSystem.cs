using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;
using System.IO;

namespace ET.Client
{
    /// <summary>
    /// Author  YIUI
    /// Date    2025.3.6
    /// Desc
    /// </summary>
    [FriendOf(typeof(LSLobbyPanelComponent))]
    public static partial class LSLobbyPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this LSLobbyPanelComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this LSLobbyPanelComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this LSLobbyPanelComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }
        
        #region YIUIEvent开始
        
        private static void OnEventEnterMapAction(this LSLobbyPanelComponent self)
        {
            EnterMapHelper.Match(self.Fiber()).Coroutine();
        }
        
        private static  void OnEventReplayAction(this LSLobbyPanelComponent self)
        {
            byte[] bytes = File.ReadAllBytes(self.ReplayPath);
            
            Replay replay = MemoryPackHelper.Deserialize(typeof (Replay), bytes, 0, bytes.Length) as Replay;
            Log.Debug($"start replay: {replay.Snapshots.Count} {replay.FrameInputs.Count} {replay.MatchInfo.UnitInfos.Count}");
            LSSceneChangeHelper.SceneChangeToReplay(self.Root(), replay).Coroutine();
        }
        
        private static void OnEventReplayPathAction(this LSLobbyPanelComponent self, string p1)
        {
            self.ReplayPath = p1;
        }
        #endregion YIUIEvent结束
    }
}