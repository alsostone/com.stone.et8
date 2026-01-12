using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// 由YIUI工具自动创建 请勿修改
    /// </summary>
    [FriendOf(typeof(YIUIComponent))]
    [FriendOf(typeof(YIUIWindowComponent))]
    [FriendOf(typeof(YIUIPanelComponent))]
    [EntitySystemOf(typeof(GMToolsPanelComponent))]
    public static partial class GMToolsPanelComponentSystem
    {
        [EntitySystem]
        private static void Awake(this GMToolsPanelComponent self)
        {
        }

        [EntitySystem]
        private static void YIUIBind(this GMToolsPanelComponent self)
        {
            self.UIBind();
        }
        
        private static void UIBind(this GMToolsPanelComponent self)
        {
            self.u_UIBase = self.GetParent<YIUIComponent>();
            self.u_UIWindow = self.UIBase.GetComponent<YIUIWindowComponent>();
            self.u_UIPanel = self.UIBase.GetComponent<YIUIPanelComponent>();
            self.UIWindow.WindowOption = EWindowOption.BanTween;
            self.UIPanel.Layer = EPanelLayer.Tips;
            self.UIPanel.PanelOption = EPanelOption.ForeverCache;
            self.UIPanel.StackOption = EPanelStackOption.Omit;
            self.UIPanel.Priority = 0;

            self.u_DataIsBattle = self.UIBase.DataTable.FindDataValue<YIUIFramework.UIDataValueBool>("u_DataIsBattle");
            self.u_EventOpenStage = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventOpenStage");
            self.u_EventOpenStageHandle = self.u_EventOpenStage.Add(self.OnEventOpenStageAction);
            self.u_EventOpenBatch = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventOpenBatch");
            self.u_EventOpenBatchHandle = self.u_EventOpenBatch.Add(self.OnEventOpenBatchAction);
            self.u_EventOpenGmTools = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventOpenGmTools");
            self.u_EventOpenGmToolsHandle = self.u_EventOpenGmTools.Add(self.OnEventOpenGmToolsAction);
            self.u_EventSetWin = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventSetWin");
            self.u_EventSetWinHandle = self.u_EventSetWin.Add(self.OnEventSetWinAction);
            self.u_EventSetLose = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventSetLose");
            self.u_EventSetLoseHandle = self.u_EventSetLose.Add(self.OnEventSetLoseAction);
            self.u_EventCardSelect = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventCardSelect");
            self.u_EventCardSelectHandle = self.u_EventCardSelect.Add(self.OnEventCardSelectAction);
            self.u_EventSaveReplay = self.UIBase.EventTable.FindEvent<UIEventP0>("u_EventSaveReplay");
            self.u_EventSaveReplayHandle = self.u_EventSaveReplay.Add(self.OnEventSaveReplayAction);

        }
    }
}