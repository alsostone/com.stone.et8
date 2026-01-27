using System;
using System.Collections.Generic;
using System.Linq;
using TrueSync;
using YIUIFramework;

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
            self.u_ComFightMode.options.Clear();
            self.u_ComFightMode.AddOptions(Enum.GetNames(typeof(FightMode)).ToList());
            self.StageLoop = new YIUILoopScroll<GMToolsStageInfo, GMToolsStageItemComponent>(self, self.u_ComStageLoop, self.StageLoopping);
            self.StageLoopSel = new YIUILoopScroll<GMToolsStageInfo, GMToolsStageItemComponent>(self, self.u_ComStageLoopSel, self.StageSelLoopping);
        }
        
        [EntitySystem]
        private static void Destroy(this GMToolsStageViewComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this GMToolsStageViewComponent self)
        {
            self.u_DataCanMatch.SetValue(false);
            self.u_ComFightMode.value = self._FightMode.Value;
            self.RefreshStageLoopSel();
            
            await ETTask.CompletedTask;
            return true;
        }
        
        private static void RefreshStageLoop(this GMToolsStageViewComponent self)
        {
            HashSet<int> hashSet = self._StageSelected.Get().ToHashSet<int>();
            
            foreach (GMToolsStageInfo info in self.StageInfos)
            {
                if (info.Selected && !hashSet.Contains(info.Id))
                {
                    info.Selected = false;
                }
                else if (!info.Selected && hashSet.Contains(info.Id))
                {
                    info.Selected = true;
                }
            }
            self.StageLoop.RefreshCells();
        }
        
        private static void RefreshTips(this GMToolsStageViewComponent self)
        {
            self.u_DataCanStart.SetValue(self.TipsMask == 0);
            if ((self.TipsMask & 1) == 1)
            {
                self.u_DataTips.SetValue($"You need to click to select a stage");
            }
            else
            {
                self.u_DataTips.SetValue(string.Empty);
            }
        }
        
        private static void RefreshStageLoopSel(this GMToolsStageViewComponent self)
        {
            int[] array = self._StageSelected.Get();
            
            self.StageSelInfos.Clear();
            for (int index = 0; index < array.Length; index++)
            {
                GMToolsStageInfo info = new GMToolsStageInfo();
                info.Id = array[index];
                info.Selected = true;
                self.StageSelInfos.Add(info);
            }
            self.StageLoopSel.SetDataRefresh(self.StageSelInfos);
            
            if (array.Length == 0) self.TipsMask |= 1; // 没有选择关卡
            else self.TipsMask &= ~1;
            self.RefreshTips();
        }
        
        private static void OnStageValueChanged(this GMToolsStageViewComponent self)
        {
            List<int> array = self._StageSelected.Get().ToList();
            foreach (GMToolsStageInfo info in self.StageInfos)
            {
                if (info.Selected && !array.Contains(info.Id))
                    array.Add(info.Id);
                else if (!info.Selected && array.Contains(info.Id))
                    array.Remove(info.Id);
            }
            self._StageSelected.Set(array.ToArray());
            self.RefreshStageLoopSel();
        }
        
        private static void OnStageSelValueChanged(this GMToolsStageViewComponent self)
        {
            List<int> array = self._StageSelected.Get().ToList();
            foreach (GMToolsStageInfo info in self.StageSelInfos)
            {
                if (!info.Selected && array.Contains(info.Id))
                    array.Remove(info.Id);
            }
            self._StageSelected.Set(array.ToArray());
            self.RefreshStageLoop();
            self.RefreshStageLoopSel();
        }
        
        private static void StageLoopping(this GMToolsStageViewComponent self, int index, GMToolsStageInfo data, GMToolsStageItemComponent item, bool select)
        {
            item.ResetItem(new GMToolsItemExtraData() { ChangeCallBack = self.OnStageValueChanged }, data);
        }
        
        private static void StageSelLoopping(this GMToolsStageViewComponent self, int index, GMToolsStageInfo data, GMToolsStageItemComponent item, bool select)
        {
            item.ResetItem(new GMToolsItemExtraData() { ChangeCallBack = self.OnStageSelValueChanged }, data);
        }

        #region YIUIEvent开始
        
        private static void OnEventPvpStartAction(this GMToolsStageViewComponent self)
        {
            
        }
        
        private static void OnEventPveStartAction(this GMToolsStageViewComponent self)
        {
            int[] array = self._StageSelected.Get();
            
            Fiber fiber = self.Fiber();
            LockStepMatchInfo matchInfo = LockStepMatchInfo.Create();
            matchInfo.StageId = array[^1];
            matchInfo.ActorId = new ActorId(fiber.Process, fiber.Id, self.InstanceId);
            matchInfo.MatchTime = TimeInfo.Instance.ServerFrameTime();
            matchInfo.Seed = (int)TimeInfo.Instance.ServerFrameTime();

            long playerId = LSConstValue.PlayerIdOffset;
            LockStepUnitInfo lockStepUnitInfo = LockStepUnitInfo.Create();
            lockStepUnitInfo.PlayerId = playerId++;
            lockStepUnitInfo.CampId = 30001; // 操作的营地ID 如果为0则是没有控制营地单位
            lockStepUnitInfo.HeroSkinId = 100201; // 操作的英雄ID 如果为0则是没有控制英雄单位
            lockStepUnitInfo.Position = TSVector.zero;
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
        
        private static void OnEventFightModeAction(this GMToolsStageViewComponent self, int p1)
        {
            self._FightMode.Value = p1;
            HashSet<int> hashSet = self._StageSelected.Get().ToHashSet<int>();
            
            self.StageInfos.Clear();
            foreach (TbStageRow tbStageRow in TbStage.Instance.DataList)
            {
                if (tbStageRow.FightMode == (FightMode)self._FightMode.Value)
                {
                    GMToolsStageInfo info = new GMToolsStageInfo();
                    info.Id = tbStageRow.Id;
                    info.Desc = tbStageRow.Name;
                    info.Selected = hashSet.Contains(tbStageRow.Id);
                    self.StageInfos.Add(info);
                }
            }
            self.StageLoop.SetDataRefresh(self.StageInfos);
        }
        #endregion YIUIEvent结束
    }
}