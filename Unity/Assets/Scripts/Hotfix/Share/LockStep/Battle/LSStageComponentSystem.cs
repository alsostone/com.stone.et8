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
        {
            self.TableId = stageId;
            self.CurrentWaveCount = 0;
            self.CurrentMonsterCount = 0;
            
            self.NextWaveFrame = self.LSWorld().Frame + self.TbRow.Delay.Convert2Frame();
            self.NextMonsterFrame = int.MaxValue;
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this LSStageComponent self)
        {
            if (self.CurrentWaveCount > self.TbRow.Count)
                return;
            
            // 波次根据配置的间隔递增
            LSWorld lsWorld = self.LSWorld();
            if (lsWorld.Frame >= self.NextWaveFrame)
            {
                self.CurrentWaveCount++;
                self.CurrentMonsterCount = 0;
                self.NextWaveFrame = lsWorld.Frame + self.TbRow.WaveInterval.Convert2Frame();
                self.NextMonsterFrame = lsWorld.Frame;
            }

            // 波次开始后根据配置的刷怪数量刷怪
            if (lsWorld.Frame >= self.NextMonsterFrame)
            {
                int index = Math.Min(self.TbRow.RandomCounts.Length, self.CurrentWaveCount);
                int limit = self.TbRow.RandomCounts[index - 1];
                if (self.CurrentMonsterCount > limit)
                    return;

                index = Math.Min(self.TbRow.RandomSets.Length, self.CurrentWaveCount);
                int randomSet = self.TbRow.RandomSets[index - 1];
                
                self.CurrentMonsterCount++;
                self.NextMonsterFrame = lsWorld.Frame + self.TbRow.RandomInterval.Convert2Frame();
                
                var results = ObjectPool.Instance.Fetch<List<Tuple<EUnitType, int, int>>>();
                RandomDropHelper.RandomSet(self.GetRandom(), randomSet, 1, results);
                foreach (var tuple in results)
                {
                    for (var i = 0; i <= tuple.Item3; i++)
                    {
                        TSVector2 v2 = self.GetRandom().GetRandomPointOnCircle(32);
                        TSVector position = new TSVector(v2.x, 0, v2.y);
                        TSQuaternion rotation = TSQuaternion.LookRotation(position, TSVector.up);
                        LSUnitFactory.SummonUnit(lsWorld, tuple.Item1, tuple.Item2, position, rotation, TeamType.TeamB);
                    }
                }
                results.Clear();
                ObjectPool.Instance.Recycle(results);
            }
        }
        
        private static TSVector2 GetRandomPointOnCircle(this TSRandom self, FP radius)
        {
            FP randomAngle = self.Range(0f, TSMath.Pi * 2f);
            FP x = radius * TSMath.Cos(randomAngle);
            FP y = radius * TSMath.Sin(randomAngle);
            return new TSVector2(x, y);
        }
    }
}