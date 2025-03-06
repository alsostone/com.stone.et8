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
    public partial class ReplayViewComponent: Entity, IUpdate
    {
        public int JumpToFrame;
        public int frame;
    }
}