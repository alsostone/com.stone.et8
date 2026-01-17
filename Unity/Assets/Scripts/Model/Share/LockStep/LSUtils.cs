using System.Collections.Generic;
using TrueSync;

namespace ET
{
    public static class LSUtils
    {
        public static LSUnit LSOwner(this LSEntity entity)
        {
            if (entity.Parent is LSUnit lsUnit)
                return lsUnit;
            return entity.Parent.Parent as LSUnit;
        }
        
        public static Entity LSParent(this LSEntity entity)
        {
            // 全局组件的父级是LSWorld
            if (entity.Parent is LSWorld lsWorld)
                return lsWorld;
            
            // 普通组件的父级是战斗单位
            if (entity.Parent is LSUnit lsUnit)
                return lsUnit;
            
            // 一个技能的父级是技能组件 技能组件的父级是战斗单位
            if (entity.Parent?.Parent is LSUnit lsParent)
                return lsParent;
            
            return entity;
        }
        
        public static LSUnit LSUnit(this LSEntity entity, long id)
        {
            LSUnitComponent unitComponent = entity.LSWorld().GetComponent<LSUnitComponent>();
            return unitComponent.GetChild<LSUnit>(id);
        }
        
        public static LSUnit LSTeamUnit(this LSEntity entity, TeamType team)
        {
            LSUnitComponent unitComponent = entity.LSWorld().GetComponent<LSUnitComponent>();
            return unitComponent.GetChild<LSUnit>(LSConstValue.GlobalIdOffset - 1 - (int)team);
        }
        
        public static Room LSRoom(this LSEntity entity)
        {
            var world = entity.IScene as LSWorld;
            return world.Parent as Room;
        }
        
        public static LSWorld LSWorld(this LSEntity entity)
        {
            return entity.IScene as LSWorld;
        }

        public static long GetId(this LSEntity entity)
        {
            return entity.LSWorld().GetId();
        }
        
        public static TSRandom GetRandom(this LSEntity entity)
        {
            return entity.LSWorld().Random;
        }
        
        public static void Shuffle<T>(this IList<T> list, TSRandom random)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(0, n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
        
        public static TSVector IgnoreY(this TSVector vector)
        {
            return new TSVector(vector.x, FP.Zero, vector.z);
        }
        
        public static TSVector ToXZ(this TSVector2 vector)
        {
            return new TSVector(vector.x, FP.Zero, vector.y);
        }

    }
}
