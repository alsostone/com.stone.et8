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
    public partial class PlayViewComponent: Entity, IUpdate
    {
        public string SaveName;
        public int PredictFrame = 0;

        public int SelectCardCount;
        public YIUILoopScroll<Tuple<EUnitType, int, int>, PlayCardItemComponent> CardLoop;
    }
}