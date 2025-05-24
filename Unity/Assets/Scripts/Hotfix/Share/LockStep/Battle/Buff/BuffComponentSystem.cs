﻿namespace ET
{
    [EntitySystemOf(typeof(BuffComponent))]
    [FriendOf(typeof(BuffComponent))]
    [FriendOf(typeof(Buff))]
    public static partial class BuffComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BuffComponent self)
        {
            
        }
        
        [EntitySystem]
        private static void Destroy(this BuffComponent self)
        {
            foreach (var valuePair in self.IdBuffMap)
            {
                Buff buff = self.GetChild<Buff>(valuePair.Value);
                buff?.Dispose();
            }
            self.IdBuffMap.Clear();
        }
        
        public static void AddBuffs(this BuffComponent self, int[] buffIds, LSUnit owner)
        {
            foreach (int buffId in buffIds)
            {
                self.AddBuff(buffId, owner);
            }
        }
        
        public static void AddBuff(this BuffComponent self, int buffId, LSUnit owner)
        {
            if (self.IdBuffMap.TryGetValue(buffId, out long eid))
            {
                // 若buffId已存在，则增加层数，且重新计时
                var buff = self.GetChild<Buff>(eid);
                if (buff.TbBuffRow.MaxLayer > 0 && buff.TbBuffRow.MaxLayer > buff.LayerCount)
                    buff.LayerCount++;
                buff.ResetEndFrame();
            }
            else
            {
                // 若buffId不存在，则添加新的buff
                var buff = self.AddChild<Buff, int, LSUnit>(buffId, owner);
                buff.LayerCount = 1;
                buff.ResetEndFrame();
                self.IdBuffMap.Add(buffId, buff.Id);
            }
        }
        
        public static void RemoveBuff(this BuffComponent self, int buffId, bool removeLayer = false)
        {
            if (self.IdBuffMap.TryGetValue(buffId, out long eid))
            {
                var buff = self.GetChild<Buff>(eid);
                if (removeLayer)
                {
                    // 若为移除层数，则减少层数，且重新计时
                    buff.LayerCount--;
                    if (buff.LayerCount > 0)
                    {
                        buff.ResetEndFrame();
                        return;
                    }
                }
                buff.Dispose();
                self.IdBuffMap.Remove(buffId);
            }
        }
        
    }
}