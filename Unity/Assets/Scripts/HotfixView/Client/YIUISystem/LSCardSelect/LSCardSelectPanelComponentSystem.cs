using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  alsostone
    /// Date    2025.9.20
    /// Desc
    /// </summary>
    [FriendOf(typeof(LSCardSelectPanelComponent))]
    public static partial class LSCardSelectPanelComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this LSCardSelectPanelComponent self)
        {
            self.CardsView = new YIUIListView<LSCardSelectItemComponent>(self, self.u_ComCardSelectRoot);
        }

        [EntitySystem]
        private static void Destroy(this LSCardSelectPanelComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this LSCardSelectPanelComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this LSCardSelectPanelComponent self, List<LSRandomDropItem> items, int count)
        {
            await ETTask.CompletedTask;
            self.SelectCount = count;
            for (int index = 0; index < items.Count; index++) {
                LSCardSelectItemComponent renderer = self.CardsView.CreateItemRenderer();
                renderer.SetData(items[index], index);
            }
            return true;
        }        
        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}