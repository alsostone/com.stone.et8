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
