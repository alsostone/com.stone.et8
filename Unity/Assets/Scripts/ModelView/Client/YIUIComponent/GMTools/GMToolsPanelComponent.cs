using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    public partial class GMToolsPanelComponent: Entity
    {
#if ENABLE_DEBUG
        public ST.GmTools.GmToolsQuickBehaviour GmToolsQuickBehaviour;
        public ST.GmTools.GmToolsBehaviour GmToolsBehaviour;
#endif

    }
}