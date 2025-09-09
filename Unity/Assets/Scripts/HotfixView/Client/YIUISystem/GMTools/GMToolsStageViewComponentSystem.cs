using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;
using TrueSync;

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
            Fiber fiber = self.Fiber();
            LockStepMatchInfo matchInfo = LockStepMatchInfo.Create();
            matchInfo.StageId = 1001;
            matchInfo.ActorId = new ActorId(fiber.Process, fiber.Id, self.InstanceId);
            matchInfo.MatchTime = TimeInfo.Instance.ServerFrameTime();
            matchInfo.Seed = (int)TimeInfo.Instance.ServerFrameTime();

            long playerId = LSConstValue.PlayerIdOffset;
            LockStepUnitInfo lockStepUnitInfo = LockStepUnitInfo.Create();
            lockStepUnitInfo.PlayerId = playerId++;
            lockStepUnitInfo.Position = new TSVector(20, 0, -10);
            lockStepUnitInfo.Rotation = TSQuaternion.identity;
            matchInfo.UnitInfos.Add(lockStepUnitInfo);
            
            long ownerPlayerId = lockStepUnitInfo.PlayerId;
            LSSceneChangeHelper.SceneChangeTo(self.Root(), LockStepMode.Local, matchInfo, ownerPlayerId, ownerPlayerId).Coroutine();
            self.UIView.Close();
        }
        
        private static void OnEventCloseAction(this GMToolsStageViewComponent self)
        {
            self.UIView.Close();
        }
        #endregion YIUIEvent结束
    }
}