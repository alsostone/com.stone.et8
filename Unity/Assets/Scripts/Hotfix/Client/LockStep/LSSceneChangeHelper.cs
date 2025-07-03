using MemoryPack;

namespace ET.Client
{

    public static partial class LSSceneChangeHelper
    {
        // 场景切换协程
        public static async ETTask SceneChangeTo(Scene root, LockStepMatchInfo matchInfo)
        {
            root.RemoveComponent<Room>();

            Room room = root.AddComponentWithId<Room>(matchInfo.ActorId.InstanceId);
            room.Name = matchInfo.SceneName;
            room.Replay.MatchInfo = matchInfo;
            
            LSWorld lsWorld = new LSWorld(SceneType.LockStepClient);
            await room.InitNewWorld(lsWorld, matchInfo);

            // 等待表现层订阅的事件完成
            await EventSystem.Instance.PublishAsync(root, new LSSceneChangeStart() {SceneName = matchInfo.SceneName, IsReplay = false});

            C2Room_LoadingProgress loadingProgress = C2Room_LoadingProgress.Create(true);
            loadingProgress.Progress = 100;
            root.GetComponent<ClientSenderComponent>().Send(loadingProgress);
            
            // 等待Room2C_EnterMap消息
            WaitType.Wait_Room2C_Start waitRoom2CStart = await root.GetComponent<ObjectWait>().Wait<WaitType.Wait_Room2C_Start>();

            room.Start(waitRoom2CStart.Message.StartTime);
            
            // 这个事件中可以订阅取消loading
            EventSystem.Instance.Publish(root, new LSSceneInitFinish());
        }
        
        // 场景切换协程
        public static async ETTask SceneChangeToReplay(Scene root, Replay replay)
        {
            root.RemoveComponent<Room>();

            Room room = root.AddComponent<Room>();
            room.Name = replay.MatchInfo.SceneName;
            room.IsReplay = true;
            room.Replay = replay;
            
            LSWorld lsWorld = new LSWorld(SceneType.LockStepClient);
            await room.InitNewWorld(lsWorld, replay.MatchInfo);
            
            // 等待表现层订阅的事件完成
            await EventSystem.Instance.PublishAsync(root, new LSSceneChangeStart() {SceneName = replay.MatchInfo.SceneName, IsReplay = true});
            
            room.Start(TimeInfo.Instance.ServerFrameTime());
            
            // 这个事件中可以订阅取消loading
            EventSystem.Instance.Publish(root, new LSSceneInitFinish());
        }
        
        // 场景切换协程
        public static async ETTask SceneChangeToReconnect(Scene root, G2C_Reconnect message)
        {
            root.RemoveComponent<Room>();
            
            Room room = root.AddComponentWithId<Room>(message.MatchInfo.ActorId.InstanceId);
            room.Name = message.MatchInfo.SceneName;
            
            room.Replay.MatchInfo = message.MatchInfo;
            room.Replay.LSWorldBytes = message.LSWorldBytes;
            
            LSWorld lsWorld = MemoryPackHelper.Deserialize(typeof(LSWorld), message.LSWorldBytes, 0, message.LSWorldBytes.Length) as LSWorld;
            await room.InitExsitWorld(lsWorld, message.MatchInfo);
            LSClientHelper.RunLSRollbackSystem(room);
            
            // 等待表现层订阅的事件完成
            await EventSystem.Instance.PublishAsync(root, new LSSceneChangeStart() {SceneName = message.MatchInfo.SceneName, IsReplay = false});
            
            room.Start(message.StartTime);
            
            // 这个事件中可以订阅取消loading
            EventSystem.Instance.Publish(root, new LSSceneInitFinish());
        }
    }
}