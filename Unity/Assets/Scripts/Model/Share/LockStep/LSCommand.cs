using System;
using TrueSync;

namespace ET
{
    // 操作类型枚举
    public enum OperateCommandType : byte
    {
        Invalid,        // 无效指令
        Move,           // 移动
        MoveTo,         // 移动到指定位置
        TouchDown,         // 触摸按下
        TouchDragStart,      // 拖拽开始
        TouchDrag,           // 拖拽
        TouchDragEnd,        // 拖拽结束
        PlacementDrag,          // 拖拽开始
        PlacementNew,          // 放置开始
        Button,         // 按钮指令
        Online,         // 上下线指令 服务器生成该指令
#if ENABLE_DEBUG
        Gm,             // GM指令 正式服中丢弃该指令
#endif
    }
    
    public enum CommandButtonType : byte
    {
        Escape,        // 取消
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
        CardSelect = 3, // 选牌
    }
#endif
    
    public struct LSCommandData
    {
        public int Header;
        public int Param1;
        public int Param2;

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Header, this.Param1, this.Param2);
        }

        public bool Equals(LSCommandData other)
        {
            return this.Header == other.Header && this.Param1 == other.Param1 && this.Param2 == other.Param2;
        }

        public override bool Equals(object obj)
        {
            return obj is LSCommandData data && Equals(data);
        }

        public static bool operator ==(LSCommandData left, LSCommandData right)
        {
            return left.Header == right.Header && left.Param1 == right.Param1 && left.Param2 == right.Param2;
        }
        public static bool operator !=(LSCommandData left, LSCommandData right)
        {
            return left.Header != right.Header || left.Param1 != right.Param1 || left.Param2 != right.Param2;
        }
    }

    public static class LSCommand
    {
        /// 特别注意：对于2个float类型的参数，将其乘以1000并转换为整数
        public static LSCommandData GenCommandFloat2(byte seatIndex, OperateCommandType type, float param1 = 0, float param2 = 0)
        {
            LSCommandData command = new LSCommandData();
            command.Header |= seatIndex << 24; // 前8位存储座位索引
            command.Header |= (int)type << 16; // 接下来的8位存储操作类型
            
            command.Param1 = (int)(param1 * 1000);
            command.Param2 = (int)(param2 * 1000);
            return command;
        }
        
        public static LSCommandData GenCommandInt2(byte seatIndex, OperateCommandType type, int param1 = 0, int param2 = 0)
        {
            LSCommandData command = new LSCommandData();
            command.Header |= seatIndex << 24; // 前8位存储座位索引
            command.Header |= (int)type << 16; // 接下来的8位存储操作类型
            
            command.Param1 = (int)param1;
            command.Param2 = (int)param2;
            return command;
        }
        
        public static LSCommandData GenCommandLong(byte seatIndex, OperateCommandType type, long param)
        {
            LSCommandData command = new LSCommandData();
            command.Header |= seatIndex << 24; // 前8位存储座位索引
            command.Header |= (int)type << 16; // 接下来的8位存储操作类型
            
            command.Param1 = (int)((param >> 32) & 0xFFFFFFFF); // 存储高位参数
            command.Param2 = (int)(param & 0xFFFFFFFF); // 存储低位参数
            return command;
        }
        
        /// 特别注意：对于2个float类型的参数，将其乘以1000并转换为整数
        public static TSVector2 ParseCommandFloat2(LSCommandData command)
        {
            return new TSVector2(command.Param1, command.Param2) / 1000;
        }
        
        public static long ParseCommandLong(LSCommandData command)
        {
            long param = (long)command.Param1 << 32;
            param |= (uint)command.Param2;
            return param;
        }
        
        public static (int, int) ParseCommandInt2(LSCommandData command)
        {
            return (command.Param1, command.Param2);
        }
        
        public static LSCommandData GenCommandMoveTo(byte seatIndex, MovementMode movementMode, float param1 = 0, float param2 = 0)
        {
            LSCommandData command = new LSCommandData();
            command.Header |= seatIndex << 24; // 前8位存储座位索引
            command.Header |= (int)OperateCommandType.MoveTo << 16; // 接下来的8位存储操作类型
            command.Header |= (int)movementMode << 8; // 接下来的8位存储移动模式
            
            command.Param1 = (int)(param1 * 1000);
            command.Param2 = (int)(param2 * 1000);
            return command;
        }
        
        public static (MovementMode, TSVector2) ParseCommandMoveTo(LSCommandData command)
        {
            MovementMode movementMode = (MovementMode)((command.Header >> 8) & 0xFF);
            return (movementMode, new TSVector2(command.Param1, command.Param2) / 1000);
        }

        public static LSCommandData GenCommandButton(byte seatIndex, CommandButtonType type, long param = 0)
        {
            LSCommandData command = new LSCommandData();
            command.Header |= seatIndex << 24; // 前8位存储座位索引
            command.Header |= (int)OperateCommandType.Button << 16; // 接下来的8位存储操作类型
            command.Header |= (int)type << 8; // 接下来的8位存储按钮类型
            
            command.Param1 = (int)((param >> 32) & 0xFFFFFFFF); // 存储高位参数
            command.Param2 = (int)(param & 0xFFFFFFFF); // 存储低位参数
            return command;
        }

        // 特别注意：对于按钮类型的参数，将其保留40位参数位 外部拼接好
        // 意为着原long取值范围需在-1099511627776 ~ 1099511627775 之间
        public static (CommandButtonType, long) ParseCommandButton(LSCommandData command)
        {
            CommandButtonType type = (CommandButtonType)((command.Header >> 8) & 0xFF);
            long param = (long)command.Param1 << 32;
            param |= (uint)command.Param2;
            return (type, param);
        }
        
        public static byte ParseCommandSeatIndex(LSCommandData command)
        {
            return (byte)(command.Header >> 24);
        }
        
        public static OperateCommandType ParseCommandType(LSCommandData command)
        {
            return (OperateCommandType)((command.Header >> 16) & 0xFF);
        }
        
#if ENABLE_DEBUG
        // 特别注意：对于GM类型的参数，将其保留40位参数位 外部拼接好
        // 意为着原long取值范围需在-1099511627776 ~ 1099511627775 之间
        public static LSCommandData GenCommandGm(byte seatIndex, CommandGMType type, long param = 0)
        {
            LSCommandData command = new LSCommandData();
            command.Header |= seatIndex << 24; // 前8位存储座位索引
            command.Header |= (int)OperateCommandType.Gm << 16; // 接下来的8位存储操作类型
            command.Header |= (int)type << 8; // 接下来的8位存储按钮类型
            
            command.Param1 = (int)((param >> 32) & 0xFFFFFFFF); // 存储高位参数
            command.Param2 = (int)(param & 0xFFFFFFFF); // 存储低位参数
            return command;
        }
        
        // 特别注意：对于GM类型的参数，将其保留40位参数位 外部拼接好
        // 意为着原long取值范围需在-1099511627776 ~ 1099511627775 之间
        public static (CommandGMType, long) ParseCommandGm(LSCommandData command)
        {
            CommandGMType type = (CommandGMType)((command.Header >> 8) & 0xFF);
            long param = (long)command.Param1 << 32;
            param |= (uint)command.Param2;
            return (type, param);
        }
#endif
        
    }
}
