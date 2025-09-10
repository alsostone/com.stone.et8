using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  YIUI
    /// Date    2025.9.9
    /// Desc
    /// </summary>
    public partial class GMToolsStageItemComponent: Entity
    {
        public GMToolsItemExtraData ExtraData;
        public GMToolsStageInfo Info;
    }
    
    [EnableClass]
    public class GMToolsItemExtraData
    {
        public Action ChangeCallBack;
    }
    
    [EnableClass]
    public class GMToolsStageInfo
    {
        public int Id;
        public string Desc;
        public bool Selected;
    }
}