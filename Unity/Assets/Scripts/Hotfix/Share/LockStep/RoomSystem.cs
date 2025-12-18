using System;
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
            lsWorld.AddComponentWithId<LSRVO2Component>(LSConstValue.RVO2ComponentId);
            lsWorld.AddComponent<LSGridMapComponent, string>($"Map/{lsStageComponent.TbRow.SceneName}.bytes");
            lsWorld.AddComponent<LSTargetsComponent>();
            lsWorld.AddComponent<AIWorldComponent>();
            lsWorld.AddComponent<LSUnitComponent>();

            LSUnitFactory.CreateGlobal(lsWorld);
            LSUnitFactory.CreateTeamPlayer(lsWorld, TeamType.TeamA);
            LSUnitFactory.CreateTeamMonster(lsWorld, TeamType.TeamB);
            LSUnitFactory.CreateTeamMonster(lsWorld, TeamType.TeamNeutral);
            
            for (int i = 0; i < matchInfo.UnitInfos.Count; ++i) {
                LockStepUnitInfo unitInfo = matchInfo.UnitInfos[i];
                TeamType teamType = (TeamType)(1 << i);
                
                LSUnit lsHero = null;
                if (unitInfo.HeroSkinId > 0) {
                    lsHero = LSUnitFactory.CreateHero(lsWorld, unitInfo.HeroSkinId, unitInfo.Position, unitInfo.Rotation, teamType);
                }
                LSUnitFactory.CreatePlayer(lsWorld, unitInfo.PlayerId, teamType, lsHero?.Id ?? 0);
                self.PlayerIds.Add(unitInfo.PlayerId);
            }
            
            // 创建基地 (测试用)
            if (lsStageComponent.TbRow.BaseCampTower > 0)
                LSUnitFactory.CreateBuilding(lsWorld, lsStageComponent.TbRow.BaseCampTower, TSVector.zero, 0, TeamType.TeamA);
            if (lsStageComponent.TbRow.BaseCampSoldier > 0 && matchInfo.UnitInfos.Count > 1)
                LSUnitFactory.CreateBuilding(lsWorld, lsStageComponent.TbRow.BaseCampSoldier, new TSVector(32, 0, 32), 0, TeamType.TeamB);
            
            // 创建初始单位 后期可改为读取配置文件
            if (!string.IsNullOrEmpty(lsStageComponent.TbRow.InitData))
            {
                string[] array = lsStageComponent.TbRow.InitData.Split(";");
                int rowCount = Int32.Parse(array[0]);
                int colCount = Int32.Parse(array[1]);
                FP spacing = (FP)Int32.Parse(array[2]) / LSConstValue.PropValueScale;
                int idMonster = Int32.Parse(array[3]);
                TeamType team = (TeamType)Int32.Parse(array[4]);
                
                FP width = (colCount - 1) * spacing;
                FP depth = (rowCount - 1) * spacing;
                TSVector start = TSVector.left * (width * FP.Half) + TSVector.forward * (depth * FP.Half);
                for (int row = 0; row < rowCount; ++row)
                for (int col = 0; col < colCount; ++col) {
                    TSVector position = start + TSVector.right * (col * spacing) - TSVector.forward * (row * spacing);
                    LSUnitFactory.CreateSoldier(lsWorld, idMonster, position, 0, team);
                }
            }
   
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

        public static void Start(this Room self, long startTime)
        {
            self.StartTime = startTime;
            self.FixedTimeCounter = new FixedTimeCounter(startTime, 0, LSConstValue.UpdateInterval);
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
            foreach (var command in frameMessage.Commands)
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
#if !DISABLE_FRAME_SNAPSHOT
                self.SaveLSWorld();
#endif
                self.Record();
            }
        }
        
#if !DISABLE_FRAME_SNAPSHOT
        public static LSWorld GetLSWorld(this Room self, int frame)
        {
            MemoryBuffer memoryBuffer = self.FrameBuffer.Snapshot(frame);
            memoryBuffer.Seek(0, SeekOrigin.Begin);
            LSWorld lsWorld = MemoryPackHelper.Deserialize(typeof(LSWorld), memoryBuffer) as LSWorld;
            memoryBuffer.Seek(0, SeekOrigin.Begin);
            return lsWorld;
        }
#endif
        public static byte[] GetLSWorldBytes(this Room self)
        {
#if !DISABLE_FRAME_SNAPSHOT
            MemoryBuffer buffer = self.FrameBuffer.Snapshot(self.LSWorld.Frame);
            buffer.Seek(0, SeekOrigin.Begin);
            return buffer.ToArray();
#else
            MemoryBuffer buffer = ObjectPool.Instance.Fetch<MemoryBuffer>();
            buffer.Seek(0, SeekOrigin.Begin);
            MemoryPackHelper.Serialize(self.LSWorld, buffer);
            buffer.Seek(0, SeekOrigin.Begin);
            byte[] bytes = buffer.ToArray();
            ObjectPool.Instance.Recycle(buffer);
            return bytes;
#endif
        }
        
#if !DISABLE_FRAME_SNAPSHOT
        private static void SaveLSWorld(this Room self)
        {
            MemoryBuffer memoryBuffer = self.FrameBuffer.Snapshot(self.LSWorld.Frame);
            memoryBuffer.Seek(0, SeekOrigin.Begin);
            memoryBuffer.SetLength(0);
            
            MemoryPackHelper.Serialize(self.LSWorld, memoryBuffer);
            memoryBuffer.Seek(0, SeekOrigin.Begin);
        }
#endif
        // 记录需要存档的数据
        public static void Record(this Room self)
        {
            if (self.LSWorld.Frame > self.AuthorityFrame) {
                return;
            }
            Room2C_FrameMessage frameMessage = self.FrameBuffer.GetFrameMessage(self.LSWorld.Frame);
            Room2C_FrameMessage saveFrameMessage = Room2C_FrameMessage.Create();
            frameMessage.CopyTo(saveFrameMessage);
            self.Replay.FrameMessages.Add(saveFrameMessage);
            
#if !DISABLE_FRAME_SNAPSHOT
            if (self.LSWorld.Frame % LSConstValue.SaveLSWorldFrameCount == 0) {
                self.Replay.Snapshots.Add(self.GetLSWorldBytes());
            }
#endif
        }
    }
}