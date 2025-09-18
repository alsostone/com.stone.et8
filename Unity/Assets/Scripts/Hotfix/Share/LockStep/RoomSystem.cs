using System.IO;
using TrueSync;

namespace ET
{
    [FriendOf(typeof(Room))]
    public static partial class RoomSystem
    {
        public static Room Room(this Entity entity)
        {
            return entity.IScene as Room;
        }
        
        public static void InitNewWorld(this Room self, LSWorld lsWorld, LockStepMatchInfo matchInfo)
        {
            int frame = -1;
            self.AuthorityFrame = frame;
            self.PredictionFrame = frame;
            self.FrameBuffer = new FrameBuffer(frame);
            self.ProcessLog = new ProcessLogMgr(frame);
            self.LSWorld = lsWorld;
            
            lsWorld.Random = new TSRandom(matchInfo.Seed);
            lsWorld.Frame = frame;
            
            LSStageComponent lsStageComponent = lsWorld.AddComponent<LSStageComponent, int>(matchInfo.StageId);
            lsWorld.AddComponent<LSGridMapComponent, string>($"Map/{lsStageComponent.TbRow.SceneName}.bytes");
            lsWorld.AddComponent<LSTargetsComponent>();
            lsWorld.AddComponent<AIWorldComponent>();
            lsWorld.AddComponent<LSUnitComponent>();

            LSUnitFactory.CreateGlobal(lsWorld);
            for (TeamType teamType = TeamType.None; teamType < TeamType.Max; ++teamType) {
                LSUnitFactory.CreateTeam(lsWorld, teamType);
            }
            
            for (int i = 0; i < matchInfo.UnitInfos.Count; ++i) {
                LockStepUnitInfo unitInfo = matchInfo.UnitInfos[i];
                TeamType teamType = (TeamType)(i + 1);
                
                LSUnit lsHero = null;
                if (unitInfo.HeroSkinId > 0) {
                    lsHero = LSUnitFactory.CreateHero(lsWorld, unitInfo.HeroSkinId, unitInfo.Position, unitInfo.Rotation, teamType);
                }
                LSUnitFactory.CreatePlayer(lsWorld, unitInfo.PlayerId, teamType, lsHero?.Id ?? 0);
                self.PlayerIds.Add(unitInfo.PlayerId);
            }
            
            // 创建基地 (测试用)
            LSUnitFactory.CreateBuilding(lsWorld, lsStageComponent.TbRow.BaseCampTower, TSVector.zero, 0, TeamType.TeamA);
            if (matchInfo.UnitInfos.Count > 1)
                LSUnitFactory.CreateBuilding(lsWorld, lsStageComponent.TbRow.BaseCampSoldier, new TSVector(32, 0, 32), 0, TeamType.TeamB);

            
            // for (int i = 0; i < 10; ++i)
            // {
            //     LSUnitFactory.CreateBuilding(lsWorld, i == 5 ? 30031 : 30011, new TSVector(11, 0, 1.5f * i + 5), TSQuaternion.Euler(0, 90, 0), TeamType.TeamA);
            //     LSUnitFactory.CreateBuilding(lsWorld, 30021, new TSVector(14, 0, 1.5f * i + 5), TSQuaternion.identity, TeamType.TeamB);
            // }
            
            // LSUnitFactory.CreateBuilding(lsWorld, 30041, new TSVector(19, 0, 1.5f * 5 + 5), TSQuaternion.identity, TeamType.TeamA);
            
            lsWorld.AddComponent<LSGameOverComponent, TbStageRow>(lsStageComponent.TbRow);
            self.ProcessLog.LogFrameEnd();
        }

        public static void InitExsitWorld(this Room self, LSWorld lsWorld, LockStepMatchInfo matchInfo)
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
        }

        public static void InitLockStep(this Room self, LockStepMode lockStepMode, long ownerPlayerId, long lookPlayerId)
        {
            self.LockStepMode = lockStepMode;
            self.OwnerPlayerId = ownerPlayerId;
            self.LookPlayerId = lookPlayerId == 0 ? self.PlayerIds[0] : lookPlayerId;
        }

        public static void Start(this Room self, long startTime)
        {
            self.StartTime = startTime;
            self.FixedTimeCounter = new FixedTimeCounter(startTime, 0, LSConstValue.UpdateInterval);
        }

        public static int GetLookSeatIndex(this Room self)
        {
            return self.PlayerIds.IndexOf(self.LookPlayerId);
        }

        public static int GetOwnerSeatIndex(this Room self)
        {
            return self.PlayerIds.IndexOf(self.OwnerPlayerId);
        }
        
        public static void Update(this Room self, Room2C_FrameMessage frameMessage)
        {
            LSWorld lsWorld = self.LSWorld;
            if (lsWorld.EndFrame > 0 && lsWorld.Frame >= lsWorld.EndFrame) {
                return;
            }
            
            // 设置输入到每个LSUnit身上
            LSUnitComponent unitComponent = lsWorld.GetComponent<LSUnitComponent>();
            
            LSCommandsRunComponent lsCommandsRunComponent = null;
            byte currentSeatIndex = 0;
            foreach (ulong command in frameMessage.Commands)
            {
                byte seatIndex = LSCommand.ParseCommandSeatIndex(command);
                if (lsCommandsRunComponent == null || currentSeatIndex != seatIndex)
                {
                    LSUnit lsUnit = unitComponent.GetChild<LSUnit>(self.PlayerIds[seatIndex]);
                    lsCommandsRunComponent = lsUnit.GetComponent<LSCommandsRunComponent>();
                    currentSeatIndex = seatIndex;
                }
                lsCommandsRunComponent.Commands.Add(command);
            }
            
            ++lsWorld.Frame;
            self.ProcessLog.LogFrameBegin(lsWorld.Frame);
            lsWorld.Update();
            self.ProcessLog.LogFrameEnd();
            
            if (self.LockStepMode >= LockStepMode.Local)
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