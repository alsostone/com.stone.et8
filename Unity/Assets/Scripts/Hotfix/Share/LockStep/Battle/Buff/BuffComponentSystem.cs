using System.Collections.Generic;

namespace ET
{
    [LSEntitySystemOf(typeof(BuffComponent))]
    [EntitySystemOf(typeof(BuffComponent))]
    [FriendOf(typeof(BuffComponent))]
    [FriendOf(typeof(Buff))]
    public static partial class BuffComponentSystem
    {
        [EntitySystem]
        private static void Awake(this BuffComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(152, self.LSParent().Id);
            self.IdBuffMap = new SortedDictionary<long, long>();
        }
        
        [EntitySystem]
        private static void Destroy(this BuffComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(37, self.LSParent().Id);
            foreach (var valuePair in self.IdBuffMap)
            {
                Buff buff = self.GetChild<Buff>(valuePair.Value);
                buff?.Dispose();
            }
            self.IdBuffMap.Clear();
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this BuffComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(36, self.LSParent().Id);
            int frame = self.LSWorld().Frame;
            
            List<long> buffs = ObjectPool.Instance.Fetch<List<long>>();
            buffs.AddRange(self.IdBuffMap.Values);
            foreach (long value in buffs)
            {
                Buff buff = self.GetChild<Buff>(value);
                if (buff == null) {
                    continue;
                }
                
                if (frame > buff.EndFrame)
                {
                    self.IdBuffMap.Remove(buff.BuffId);
                    buff.Dispose();
                }
                else if(self.LSOwner().EnabledRef >= 0)
                {
                    buff.TryExecuteInterval();
                }
            }
            buffs.Clear();
            ObjectPool.Instance.Recycle(buffs);
        }

        public static void AddBuffs(this BuffComponent self, int[] buffIds, LSUnit owner)
        {self.LSRoom()?.ProcessLog.LogFunction(35, self.LSParent().Id, owner.Id);
            foreach (int buffId in buffIds)
            {
                self.AddBuff(buffId, owner);
            }
        }
        
        public static void AddBuff(this BuffComponent self, int buffId, LSUnit owner)
        {self.LSRoom()?.ProcessLog.LogFunction(34, self.LSParent().Id, buffId, owner.Id);
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
        {self.LSRoom()?.ProcessLog.LogFunction(33, self.LSParent().Id, buffId, removeLayer ? 1 : 0);
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