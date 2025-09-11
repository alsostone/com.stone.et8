using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace ET.Client
{
    /// <summary>
    /// Author  YIUI
    /// Date    2025.9.11
    /// Desc
    /// </summary>
    [FriendOf(typeof(SettleStatViewComponent))]
    public static partial class SettleStatViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this SettleStatViewComponent self)
        {
        }
        
        [EntitySystem]
        private static void Destroy(this SettleStatViewComponent self)
        {
        }
        
        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this SettleStatViewComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }
        
        #region YIUIEvent开始
        private static async ETTask OnEventBackLastAction(this SettleStatViewComponent self)
        {
            Room room = self.Room();
            LSCacheSceneNameComponent sceneNameComponent = room.GetComponent<LSCacheSceneNameComponent>();
            if (sceneNameComponent.Name == "Init")
            {
                ResourcesLoaderComponent resourcesLoaderComponent = self.Root().GetComponent<ResourcesLoaderComponent>();
                await resourcesLoaderComponent.LoadSceneAsync($"Assets/Bundles/Scenes/Empty.unity", LoadSceneMode.Single);
                await YIUIMgrComponent.Inst.HomePanel<LoginPanelComponent>();
                room.Dispose();
            }
            
            await ETTask.CompletedTask;
        }
        #endregion YIUIEvent结束
    }
}