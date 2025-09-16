using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  YIUI
    /// Date    2025.9.16
    /// Desc
    /// </summary>
    [FriendOf(typeof(PlayCardItemComponent))]
    public static partial class PlayCardItemComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this PlayCardItemComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this PlayCardItemComponent self)
        {
        }

        public static void ResetItem(this PlayCardItemComponent self, Tuple<EUnitType, int, int> data)
        {
            
        }
        
        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}