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
        
        /// 特别注意：对于2个float类型的参数，将其乘以1000并转换为整数再保留24位
        /// 意为着原float取值范围需在-8388.608f ~ 8388.607f 之间
        /// 遥杆值-1000~1000够用 如若传输特大世界坐标就不够用了，需再拓展
        public static ulong GenCommandFloat24x2(byte seatIndex, OperateCommandType type, float param1 = 0, float param2 = 0)
        {
            ulong command = 0;
            command |= (ulong)seatIndex << 56; // 前8位存储座位索引
            command |= (ulong)type << 48; // 接下来的8位存储操作类型
            command |= (ulong)((uint)(param1 * 1000) & 0xFFFFFF) << 24; // 接下来的24位存储param1
            command |= (ulong)((uint)(param2 * 1000) & 0xFFFFFF) << 0; // 接下来的24位存储param2
            return command;
        }

        /// 特别注意：对于2个int类型的参数，将其保留24位
        /// 意为着原int取值范围需在-8388608 ~ 8388607 之间
        public static ulong GenCommandInt24x2(byte seatIndex, OperateCommandType type, int param1 = 0, int param2 = 0)
        {
            ulong command = 0;
            command |= (ulong)seatIndex << 56; // 前8位存储座位索引
            command |= (ulong)type << 48; // 接下来的8位存储操作类型
            command |= (ulong)((uint)param1 & 0xFFFFFF) << 24; // 接下来的24位存储param1
            command |= (ulong)((uint)param2 & 0xFFFFFF) << 0; // 接下来的24位存储param2
            return command;
        }
        
        // 特别注意：对于2个float类型的参数，将其乘以1000并转换为整数再保留24位
        // 意为着原float取值范围需在-8388.608f ~ 8388.607f 之间
        public static TSVector2 ParseCommandFloat24x2(ulong command)
        {
            uint i1 = (uint)((command >> 24) & 0x00FFFFFF);
            uint i2 = (uint)(command & 0x00FFFFFF);
            i1 = (i1 >> 23) == 1 ? i1 | 0xFF000000 : i1;
            i2 = (i2 >> 23) == 1 ? i2 | 0xFF000000 : i2;
            return new TSVector2(i1, i2) / 1000;
        }
        
        // 特别注意：对于2个int类型的参数，将其保留24位
        // 意为着原int取值范围需在-8388608 ~ 8388607 之间
        public static (int, int) ParseCommandInt24x2(ulong command)
        {
            uint i1 = (uint)((command >> 24) & 0xFFFFFF);
            uint i2 = (uint)(command & 0xFFFFFF);
            i1 = (i1 >> 23) == 1 ? i1 | 0xFF000000 : i1;
            i2 = (i2 >> 23) == 1 ? i2 | 0xFF000000 : i2;
            return ((int)i1, (int)i2);
        }
        
        /// 特别注意：对于按钮类型的参数，将其保留40位参数位 外部拼接好
        /// 意为着原long取值范围需在-1099511627776 ~ 1099511627775 之间
        public static ulong GenCommandButton(byte seatIndex, CommandButtonType type, long param = 0)
        {
            ulong command = 0;
            command |= (ulong)seatIndex << 56; // 前8位存储座位索引
            command |= (ulong)OperateCommandType.Button << 48; // 接下来的8位存储操作类型
            command |= (ulong)type << 40; // 接下来的8位存储按钮类型
            command |= (ulong)(param & 0xFFFFFFFFFF); // 后40位存储参数
            return command;
        }

        // 特别注意：对于按钮类型的参数，将其保留40位参数位 外部拼接好
        // 意为着原long取值范围需在-1099511627776 ~ 1099511627775 之间
        public static (CommandButtonType, int) ParseCommandButton(ulong command)
        {
            CommandButtonType buttonType = (CommandButtonType)((command >> 40) & 0xFF);
            int param = (int)(command & 0xFFFFFFFFFF);
            return (buttonType, param);
        }
        
        public static byte ParseCommandSeatIndex(ulong command)
        {
            return (byte)(command >> 56);
        }
        
        public static OperateCommandType ParseCommandType(ulong command)
        {
            return (OperateCommandType)((command >> 48) & 0xFF);
        }
        
    }
}
