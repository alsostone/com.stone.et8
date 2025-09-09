using MemoryPack;

namespace ET.Client
{

    public static partial class LSSceneChangeHelper
    {
        // 场景切换协程
        public static async ETTask SceneChangeTo(Scene root, LockStepMode lockStepMode, LockStepMatchInfo matchInfo, long ownerPlayerId, long lookPlayerId)
        {
            root.RemoveComponent<Room>();

            Room room = root.AddComponentWithId<Room>(matchInfo.ActorId.InstanceId);
            room.StageId = matchInfo.StageId;
            room.Replay.MatchInfo = matchInfo;
            
            LSWorld lsWorld = new LSWorld(SceneType.LockStepClient);
            room.InitNewWorld(lsWorld, matchInfo);
            room.InitLockStep(lockStepMode, ownerPlayerId, lookPlayerId);

            // 等待表现层订阅的事件完成
            TbStageRow row = TbStage.Instance.Get(matchInfo.StageId);
            await EventSystem.Instance.PublishAsync(root, new LSSceneChangeStart() {SceneName = row.SceneName, IsReplay = false});

            // 联网模式发送加载100%消息 且等待战斗开始消息
            long startTime = TimeInfo.Instance.ServerFrameTime();
            if (lockStepMode == LockStepMode.Server) {
                C2Room_LoadingProgress loadingProgress = C2Room_LoadingProgress.Create(true);
                loadingProgress.Progress = 100;
                root.GetComponent<ClientSenderComponent>().Send(loadingProgress);
                
                WaitType.Wait_Room2C_Start waitRoom2CStart = await root.GetComponent<ObjectWait>().Wait<WaitType.Wait_Room2C_Start>();
                startTime = waitRoom2CStart.Message.StartTime;
            }
            
            room.Start(startTime);
            
            // 这个事件中可以订阅取消loading
            EventSystem.Instance.Publish(root, new LSSceneInitFinish());
        }
        
        // 场景切换协程
        public static async ETTask SceneChangeToReplay(Scene root, Replay replay, long ownerPlayerId, long lookPlayerId)
        {
            root.RemoveComponent<Room>();

            Room room = root.AddComponent<Room>();
            room.StageId = replay.MatchInfo.StageId;
            room.Replay = replay;
            
            LSWorld lsWorld = new LSWorld(SceneType.LockStepClient);
            room.InitNewWorld(lsWorld, replay.MatchInfo);
            room.InitLockStep(LockStepMode.ObserverFile, ownerPlayerId, lookPlayerId);
            
            // 等待表现层订阅的事件完成
            TbStageRow row = TbStage.Instance.Get(replay.MatchInfo.StageId);
            await EventSystem.Instance.PublishAsync(root, new LSSceneChangeStart() {SceneName = row.SceneName, IsReplay = true});
            
            room.Start(TimeInfo.Instance.ServerFrameTime());
            
            // 这个事件中可以订阅取消loading
            EventSystem.Instance.Publish(root, new LSSceneInitFinish());
        }
        
        // 场景切换协程
        public static async ETTask SceneChangeToReconnect(Scene root, G2C_Reconnect message, long ownerPlayerId, long lookPlayerId)
        {
            root.RemoveComponent<Room>();
            
            Room room = root.AddComponentWithId<Room>(message.MatchInfo.ActorId.InstanceId);
            room.StageId = message.MatchInfo.StageId;
            
            room.Replay.MatchInfo = message.MatchInfo;
            room.Replay.LSWorldBytes = message.LSWorldBytes;
            
            LSWorld lsWorld = MemoryPackHelper.Deserialize(typeof(LSWorld), message.LSWorldBytes, 0, message.LSWorldBytes.Length) as LSWorld;
            room.InitExsitWorld(lsWorld, message.MatchInfo);
            room.InitLockStep(LockStepMode.Server, ownerPlayerId, lookPlayerId);

            LSClientHelper.RunLSRollbackSystem(room);
            
            // 等待表现层订阅的事件完成
            TbStageRow row = TbStage.Instance.Get(message.MatchInfo.StageId);
            await EventSystem.Instance.PublishAsync(root, new LSSceneChangeStart() {SceneName = row.SceneName, IsReplay = false});
            
            room.Start(message.StartTime);
            
            // 这个事件中可以订阅取消loading
            EventSystem.Instance.Publish(root, new LSSceneInitFinish());
        }
    }
}