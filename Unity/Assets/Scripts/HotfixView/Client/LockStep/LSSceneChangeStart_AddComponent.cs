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
            room.AddComponent<YIUIRootComponent>();

            // 创建loading界面
            
            
            // 创建房间UI
            var viewIndex = args.IsReplay ? ELSRoomPanelViewEnum.ReplayView : ELSRoomPanelViewEnum.PlayView;
            await room.GetComponent<YIUIRootComponent>().OpenPanelAsync<LSRoomPanelComponent, ELSRoomPanelViewEnum>(viewIndex);
            
            // 加载场景资源
            await resourcesLoaderComponent.LoadSceneAsync($"Assets/Bundles/Scenes/{args.SceneName}.unity", LoadSceneMode.Single);
        }
    }
}