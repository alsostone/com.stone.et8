using TrueSync;

namespace ET
{
    // 操作类型枚举
    public enum OperateCommandType : byte
    {
        Invalid,        // 无效指令
        Move,           // 移动
        PlacementDragStart,      // 拖拽开始
        PlacementDrag,           // 拖拽
        PlacementDragEnd,        // 拖拽结束
        PlacementStart,          // 放置新物体
        Button,         // 按钮指令
        Online,         // 上下线指令 服务器生成该指令
#if ENABLE_DEBUG
        Gm,             // GM指令 正式服中丢弃该指令
#endif
    }
    
    public enum CommandButtonType : byte
    {
        PlacementRotate,    // 放置旋转
        PlacementCancel,    // 取消放置
        CardSelect,         // 选牌
        Jump,       // 跳跃
        Attack,     // 攻击
        Skill1,     // 技能1
        Skill2,     // 技能2
        Skill3,     // 技能3
        Skill4,     // 技能4
    }
    
#if ENABLE_DEBUG
    public enum CommandGMType : byte
    {
        Victory = 1,  // 结算胜利
        Failure = 2,  // 结算失败
    }
#endif

    public static class LSCommand
    {
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
        
        /// 特别注意：对于一个long类型的参数，将其保留48位
        /// 意为着原long取值范围需在-281474976710656 ~ 281474976710655 之间
        public static ulong GenCommandLong48(byte seatIndex, OperateCommandType type, ulong param)
        {
            ulong command = 0;
            command |= (ulong)seatIndex << 56; // 前8位存储座位索引
            command |= (ulong)type << 48; // 接下来的8位存储操作类型
            command |= (ulong)(param & 0xFFFFFFFFFFFF); // 后48位存储参数
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
        
        /// 特别注意：对于一个long类型的参数，将其保留48位
        /// 意为着原long取值范围需在-281474976710656 ~ 281474976710655 之间
        public static ulong ParseCommandLong48(ulong command)
        {
            ulong param = (command & 0xFFFFFFFFFFFF);
            if ((param >> 47) == 1) param |= 0xFFFF000000000000;
            return param;
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
        public static (CommandButtonType, long) ParseCommandButton(ulong command)
        {
            CommandButtonType buttonType = (CommandButtonType)((command >> 40) & 0xFF);
            ulong param = (command & 0xFFFFFFFFFF);
            if ((param >> 39) == 1) param |= 0xFFFFFF000000000;
            return (buttonType, (long)param);
        }
        
        public static byte ParseCommandSeatIndex(ulong command)
        {
            return (byte)(command >> 56);
        }
        
        public static OperateCommandType ParseCommandType(ulong command)
        {
            return (OperateCommandType)((command >> 48) & 0xFF);
        }
        
#if ENABLE_DEBUG
        // 特别注意：对于GM类型的参数，将其保留40位参数位 外部拼接好
        // 意为着原long取值范围需在-1099511627776 ~ 1099511627775 之间
        public static ulong GenCommandGm(byte seatIndex, CommandGMType type, long param = 0)
        {
            ulong command = 0;
            command |= (ulong)seatIndex << 56; // 前8位存储座位索引
            command |= (ulong)OperateCommandType.Gm << 48; // 接下来的8位存储操作类型
            command |= (ulong)type << 40; // 接下来的8位存储按钮类型
            command |= (ulong)(param & 0xFFFFFFFFFF); // 后40位存储参数
            return command;
        }
        
        // 特别注意：对于GM类型的参数，将其保留40位参数位 外部拼接好
        // 意为着原long取值范围需在-1099511627776 ~ 1099511627775 之间
        public static (CommandGMType, long) ParseCommandGm(ulong command)
        {
            CommandGMType gmType = (CommandGMType)((command >> 40) & 0xFF);
            ulong param = (command & 0xFFFFFFFFFF);
            if ((param >> 39) == 1) param |= 0xFFFFFF000000000;
            return (gmType, (long)param);
        }
#endif
        
    }
}
