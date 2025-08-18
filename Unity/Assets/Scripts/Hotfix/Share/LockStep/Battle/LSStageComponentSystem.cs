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
            self.NextWaveFrame = self.LSWorld().Frame + self.TbRow.Delay.Convert2Frame();
            self.CurrentMonsterCount = int.MaxValue;
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this LSStageComponent self)
        {
            if (self.CurrentWaveCount > self.TbRow.Count)
                return;
            
            if (self.LSWorld().Frame >= self.NextWaveFrame)
            {
                self.CurrentWaveCount++;
                self.CurrentMonsterCount = 0;
                self.NextWaveFrame = self.LSWorld().Frame + self.TbRow.WaveInterval.Convert2Frame();
            }
            if (self.CurrentWaveCount == 0)
                return;
            
            int index = Math.Min(self.TbRow.RandomCounts.Length, self.CurrentWaveCount);
            if (self.CurrentMonsterCount >= self.TbRow.RandomCounts[index - 1])
                return;
            
            
            
        }
    }
}