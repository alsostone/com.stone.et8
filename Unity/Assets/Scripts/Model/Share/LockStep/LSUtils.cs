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
        
        public static LSUnit LSUnit(this LSEntity entity, long id)
        {
            LSUnitComponent unitComponent = entity.LSWorld().GetComponent<LSUnitComponent>();
            return unitComponent.GetChild<LSUnit>(id);
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
            // TODO:随机数 应该在初始化LSWorld时初始化
            var random = entity.LSWorld().Random;
            if (random == null)
            {
                random = new TSRandom(entity.LSWorld().Frame);
                entity.LSWorld().Random = random;
            }
            return random;
        }
        
        public static int Convert2Frame(this int milliseconds)
        {
            return (milliseconds / LSConstValue.UpdateInterval);
        }

        public static FP GetAttackSqrRange(this LSUnit unit, FP range)
        {
            return range * range;
        }
    }
}
