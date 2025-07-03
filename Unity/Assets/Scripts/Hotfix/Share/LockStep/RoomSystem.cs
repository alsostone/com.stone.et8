using System.IO;
using ST.GridBuilder;
using TrueSync;
using MemoryPack;

namespace ET
{
    [FriendOf(typeof(Room))]
    public static partial class RoomSystem
    {
        public static Room Room(this Entity entity)
        {
            return entity.IScene as Room;
        }
        
        public static async ETTask InitNewWorld(this Room self, LSWorld lsWorld, LockStepMatchInfo matchInfo)
        {
            int frame = -1;
            self.AuthorityFrame = frame;
            self.PredictionFrame = frame;
            self.FrameBuffer = new FrameBuffer(frame);
            self.ProcessLog = new ProcessLogMgr(frame);
            self.LSWorld = lsWorld;
            
            lsWorld.Random = new TSRandom(matchInfo.Seed);
            lsWorld.Frame = frame;
            
            byte[] gridBytes = await FileComponent.Instance.Get($"Map/{matchInfo.SceneName}.bytes");
            GridData gridData = MemoryPackSerializer.Deserialize<GridData>(gridBytes);
            lsWorld.AddComponent<LSGridMapComponent, GridData>(gridData);
            lsWorld.AddComponent<LSTargetsComponent>();
            lsWorld.AddComponent<AIWorldComponent>();
            lsWorld.AddComponent<LSUnitComponent>();
            
            for (int i = 0; i < matchInfo.UnitInfos.Count; ++i)
            {
                LockStepUnitInfo unitInfo = matchInfo.UnitInfos[i];
                LSUnitFactory.CreateHero(lsWorld, 1001, unitInfo.Position, unitInfo.Rotation, unitInfo.PlayerId);
                self.PlayerIds.Add(unitInfo.PlayerId);
            }
            
            for (int i = 0; i < 10; ++i)
            {
                LSUnitFactory.CreateSoldier(lsWorld, 4001, new TSVector(1, 0, 1 * i + 20), TSQuaternion.identity, TeamType.TeamA);
                LSUnitFactory.CreateSoldier(lsWorld, 4002, new TSVector(3, 0, 1 * i + 20), TSQuaternion.identity, TeamType.TeamB);
            }
            for (int i = 0; i < 10; ++i)
            {
                LSUnitFactory.CreateBuilding(lsWorld, i == 5 ? 3003 : 3001, new TSVector(6, 0, 1 * i + 20), TSQuaternion.identity, TeamType.TeamA);
                LSUnitFactory.CreateBuilding(lsWorld, 3002, new TSVector(9, 0, 1 * i + 20), TSQuaternion.identity, TeamType.TeamB);
            }
            
            LSUnitFactory.CreateBuilding(lsWorld, 3004, new TSVector(14, 0, 1 * 5 + 20), TSQuaternion.identity, TeamType.TeamA);
            self.ProcessLog.LogFrameEnd();
        }

        public static async ETTask InitExsitWorld(this Room self, LSWorld lsWorld, LockStepMatchInfo matchInfo)
        {
            int frame = lsWorld.Frame;
            self.AuthorityFrame = frame;
            self.PredictionFrame = frame;
            self.FrameBuffer = new FrameBuffer(frame);
            self.ProcessLog = new ProcessLogMgr(frame);
            self.LSWorld = lsWorld;
            
            for (int i = 0; i < matchInfo.UnitInfos.Count; ++i)
            {
                LockStepUnitInfo unitInfo = matchInfo.UnitInfos[i];
                self.PlayerIds.Add(unitInfo.PlayerId);
            }
            self.ProcessLog.LogFrameEnd();
            await ETTask.CompletedTask;
        }

        public static void Start(this Room self, long startTime)
        {
            self.StartTime = startTime;
            self.FixedTimeCounter = new FixedTimeCounter(startTime, 0, LSConstValue.UpdateInterval);
        }

        public static int GetSeatIndex(this Room self, long playerId)
        {
            return self.PlayerIds.IndexOf(playerId);
        }

        public static void Update(this Room self, Room2C_FrameMessage frameMessage)
        {
            LSWorld lsWorld = self.LSWorld;
            
            // 设置输入到每个LSUnit身上
            LSUnitComponent unitComponent = lsWorld.GetComponent<LSUnitComponent>();
            foreach (var kv in frameMessage.Inputs)
            {
                LSUnit lsUnit = unitComponent.GetChild<LSUnit>(kv.Key);
                LSInputComponent lsInputComponent = lsUnit.GetComponent<LSInputComponent>();
                lsInputComponent.LSInput = kv.Value;
            }
            
            ++lsWorld.Frame;
            self.ProcessLog.LogFrameBegin(lsWorld.Frame);
            lsWorld.Update();
            self.ProcessLog.LogFrameEnd();
            
            if (!self.IsReplay)
            {
                // 保存当前帧场景数据
                self.SaveLSWorld(lsWorld.Frame);
                self.Record(lsWorld.Frame);
            }
        }
        
        public static LSWorld GetLSWorld(this Room self, int frame)
        {
            MemoryBuffer memoryBuffer = self.FrameBuffer.Snapshot(frame);
            memoryBuffer.Seek(0, SeekOrigin.Begin);
            LSWorld lsWorld = MemoryPackHelper.Deserialize(typeof(LSWorld), memoryBuffer) as LSWorld;
            memoryBuffer.Seek(0, SeekOrigin.Begin);
            return lsWorld;
        }

        private static void SaveLSWorld(this Room self, int frame)
        {
            MemoryBuffer memoryBuffer = self.FrameBuffer.Snapshot(frame);
            memoryBuffer.Seek(0, SeekOrigin.Begin);
            memoryBuffer.SetLength(0);
            
            MemoryPackHelper.Serialize(self.LSWorld, memoryBuffer);
            memoryBuffer.Seek(0, SeekOrigin.Begin);

            long hash = memoryBuffer.GetBuffer().Hash(0, (int) memoryBuffer.Length);
            
            self.FrameBuffer.SetHash(frame, hash);
        }

        // 记录需要存档的数据
        public static void Record(this Room self, int frame)
        {
            if (frame > self.AuthorityFrame)
            {
                return;
            }
            Room2C_FrameMessage frameMessage = self.FrameBuffer.GetFrameMessage(frame);
            Room2C_FrameMessage saveFrameMessage = Room2C_FrameMessage.Create();
            frameMessage.CopyTo(saveFrameMessage);
            self.Replay.FrameMessages.Add(saveFrameMessage);
            if (frame % LSConstValue.SaveLSWorldFrameCount == 0)
            {
                MemoryBuffer memoryBuffer = self.FrameBuffer.Snapshot(frame);
                byte[] bytes = memoryBuffer.ToArray();
                self.Replay.Snapshots.Add(bytes);
            }
        }
    }
}