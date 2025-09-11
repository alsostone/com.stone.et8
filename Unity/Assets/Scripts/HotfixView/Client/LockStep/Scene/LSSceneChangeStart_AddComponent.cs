using UnityEngine.SceneManagement;

namespace ET.Client
{
    [Event(SceneType.LockStep)]
    public class LSSceneChangeStart_AddComponent: AEvent<Scene, LSSceneChangeStart>
    {
        protected override async ETTask Run(Scene clientScene, LSSceneChangeStart args)
        {
            Room room = clientScene.GetComponent<Room>();
            ResourcesLoaderComponent resourcesLoaderComponent = room.AddComponent<ResourcesLoaderComponent>();
            YIUIRootComponent yiuiRootComponent = room.AddComponent<YIUIRootComponent>();
            room.AddComponent<LSCacheSceneNameComponent, string>(SceneManager.GetActiveScene().name);

            // 创建loading界面
            
            
            // 创建房间UI
            var viewIndex = room.LockStepMode < LockStepMode.Local ? ELSRoomPanelViewEnum.ReplayView : ELSRoomPanelViewEnum.PlayView;
            await yiuiRootComponent.OpenPanelAsync<LSRoomPanelComponent, ELSRoomPanelViewEnum>(viewIndex);
            
            // 加载场景资源
            await resourcesLoaderComponent.LoadSceneAsync($"Assets/Bundles/Scenes/{args.SceneName}.unity", LoadSceneMode.Single);
        }
    }
}