using System;
using System.Collections.Generic;
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
        
        public static void Init(this Room self, List<LockStepUnitInfo> unitInfos, long startTime, int seed, int frame = -1)
        {
            self.StartTime = startTime;
            self.Seed = seed;
            self.AuthorityFrame = frame;
            self.PredictionFrame = frame;
            self.Replay.UnitInfos = unitInfos;
            self.Replay.Seed = seed;
            self.FrameBuffer = new FrameBuffer(frame);
            self.FixedTimeCounter = new FixedTimeCounter(startTime, 0, LSConstValue.UpdateInterval);
            self.ProcessLog = new ProcessLogMgr(frame);
            
            LSWorld lsWorld = self.LSWorld;
            lsWorld.Random = new TSRandom(seed);
            lsWorld.Frame = frame + 1;
            
            lsWorld.AddComponent<LSTargetsComponent>();
            lsWorld.AddComponent<AIWorldComponent>();
            lsWorld.AddComponent<LSUnitComponent>();
            for (int i = 0; i < unitInfos.Count; ++i)
            {
                LockStepUnitInfo unitInfo = unitInfos[i];
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
        }

        public static void Update(this Room self, OneFrameInputs oneFrameInputs)
        {
            LSWorld lsWorld = self.LSWorld;
            
            // 设置输入到每个LSUnit身上
            LSUnitComponent unitComponent = lsWorld.GetComponent<LSUnitComponent>();
            foreach (var kv in oneFrameInputs.Inputs)
            {
                LSUnit lsUnit = unitComponent.GetChild<LSUnit>(kv.Key);
                LSInputComponent lsInputComponent = lsUnit.GetComponent<LSInputComponent>();
                lsInputComponent.LSInput = kv.Value;
            }
            
            if (!self.IsReplay)
            {
                // 保存当前帧场景数据
                self.SaveLSWorld(lsWorld.Frame);
                self.Record(lsWorld.Frame);
            }

            self.ProcessLog.LogFrameBegin(lsWorld.Frame);
            lsWorld.Update();
            self.ProcessLog.LogFrameEnd();
        }
        
        public static LSWorld GetLSWorld(this Room self, SceneType sceneType, int frame)
        {
            MemoryBuffer memoryBuffer = self.FrameBuffer.Snapshot(frame);
            memoryBuffer.Seek(0, SeekOrigin.Begin);
            LSWorld lsWorld = MemoryPackHelper.Deserialize(typeof (LSWorld), memoryBuffer) as LSWorld;
            lsWorld.SceneType = sceneType;
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
            OneFrameInputs oneFrameInputs = self.FrameBuffer.FrameInputs(frame);
            OneFrameInputs saveInput = OneFrameInputs.Create();
            oneFrameInputs.CopyTo(saveInput);
            self.Replay.FrameInputs.Add(saveInput);
            if (frame % LSConstValue.SaveLSWorldFrameCount == 0)
            {
                MemoryBuffer memoryBuffer = self.FrameBuffer.Snapshot(frame);
                byte[] bytes = memoryBuffer.ToArray();
                self.Replay.Snapshots.Add(bytes);
            }
        }
    }
}