using System;
using System.Collections.Generic;
using TrueSync;

namespace ET
{
    [LSEntitySystemOf(typeof(LSStageComponent))]
    [EntitySystemOf(typeof(LSStageComponent))]
    [FriendOf(typeof(LSStageComponent))]
    public static partial class LSStageComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSStageComponent self, int stageId)
        {self.LSRoom()?.ProcessLog.LogFunction(139, self.LSParent().Id, stageId);
            self.TableId = stageId;
            self.CurrentWaveCount = 0;
            self.CurrentMonsterCount = 0;
            
            self.NextWaveTime = self.LSWorld().ElapsedTime + self.TbRow.Delay * FP.EN3;
            self.NextMonsterTime = FP.MaxValue;
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this LSStageComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(138, self.LSParent().Id);
            if (self.TbRow.Count == 0 || self.CurrentWaveCount > self.TbRow.Count)
                return;
            
            // 波次根据配置的间隔递增
            LSWorld lsWorld = self.LSWorld();
            if (lsWorld.ElapsedTime >= self.NextWaveTime)
            {
                self.CurrentWaveCount++;
                self.CurrentMonsterCount = 0;
                self.NextWaveTime += self.TbRow.WaveInterval * FP.EN3;
                self.NextMonsterTime = lsWorld.ElapsedTime;
            }
            if (self.CurrentWaveCount == 0) {
                return;
            }

            // 波次开始后根据配置的刷怪数量刷怪
            if (lsWorld.ElapsedTime >= self.NextMonsterTime)
            {
                int index = Math.Min(self.TbRow.RandomSets.Length, self.CurrentWaveCount);
                int randomSet = self.TbRow.RandomSets[index - 1];
                
                self.CurrentMonsterCount++;
                self.NextMonsterTime += self.TbRow.RandomInterval * FP.EN3;
                
                var results = ObjectPool.Instance.Fetch<List<LSRandomDropItem>>();
                RandomDropHelper.RandomSet(self.GetRandom(), randomSet, 1, results);
                foreach (var item in results)
                {
                    for (var i = 0; i < item.Count; i++)
                    {
                        TSVector2 v2 = self.GetRandomPointOnCircle(self.GetRandom(), 32);
                        TSVector position = new TSVector(v2.x, 0, v2.y);
                        TSQuaternion rotation = TSQuaternion.LookRotation(position, TSVector.up);
                        
                        FP angle = rotation.eulerAngles.y;
                        LSUnitFactory.SummonUnit(lsWorld, item.Type, item.TableId, position, angle.AsInt(), TeamType.TeamB);
                    }
                }
                results.Clear();
                ObjectPool.Instance.Recycle(results);
                
                // 达到当前波次的最大刷怪数量则不再刷怪
                index = Math.Min(self.TbRow.RandomCounts.Length, self.CurrentWaveCount);
                int limit = self.TbRow.RandomCounts[index - 1];
                if (self.CurrentMonsterCount >= limit) {
                    self.NextMonsterTime = FP.MaxValue;
                }
            }
        }
        
        private static TSVector2 GetRandomPointOnCircle(this LSStageComponent self, TSRandom random, FP radius)
        {
            FP randomAngle = random.Range(0f, TSMath.Pi * 2f);
            FP x = radius * TSMath.Cos(randomAngle);
            FP y = radius * TSMath.Sin(randomAngle);
            return new TSVector2(x, y);
        }

        public static bool CheckAllWaveDone(this LSStageComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(137, self.LSParent().Id);
            if (self.TbRow.Count >= self.CurrentWaveCount)
                return false;
            if (self.NextMonsterTime != FP.MaxValue)
                return false;
            return true;
        }
        
        public static bool CheckWaveDone(this LSStageComponent self, int wave)
        {self.LSRoom()?.ProcessLog.LogFunction(136, self.LSParent().Id, wave);
            if (self.CurrentWaveCount != wave)
                return false;
            if (self.NextMonsterTime != FP.MaxValue)
                return false;
            return true;
        }
    }
}