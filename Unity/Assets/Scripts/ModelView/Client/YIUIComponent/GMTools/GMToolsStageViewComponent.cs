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
    public partial class GMToolsStageViewComponent: Entity
    {
        public int TipsMask;        // 二进制每一位代表一类提示
        public IntPrefs _FightMode = new("GMToolsStageViewComponent_FightMode");
        public ArrPrefs<int> _StageSelected = new("GMToolsStageViewComponent_StageSelected");
        
        public List<GMToolsStageInfo> StageInfos = new List<GMToolsStageInfo>();
        public List<GMToolsStageInfo> StageSelInfos = new List<GMToolsStageInfo>();
        public YIUILoopScroll<GMToolsStageInfo, GMToolsStageItemComponent> StageLoop;
        public YIUILoopScroll<GMToolsStageInfo, GMToolsStageItemComponent> StageLoopSel;
    }
}