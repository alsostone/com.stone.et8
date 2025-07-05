namespace ET
{
    public static class LSConstValue
    {
        public const int MatchCount = 1;
        public const int UpdateInterval = 50;
        public const int Milliseconds = 1000;
        public const int FrameCountPerSecond = Milliseconds / UpdateInterval;
        
        public const int Probability = 10000;           // 概率配置以万分比
        public const int PropRuntime2MaxOffset = 1000;

        public const int PredictionFrameMaxCount = 15;  // 预测帧最大值
        public const int SaveLSWorldFrameCount = 60 * FrameCountPerSecond;
        
        public const string ProcessFolderName = "/process_log";
        public const string ProcessFolderNameSvr = "/process_log_server";
        public const string LSWroldFolderName = "/frame_lsworld";
        public const string LSWroldFolderNameSvr = "/frame_lsworld_server";
    }
    
    // 操作类型枚举
    public enum OperateCommandType : byte
    {
        Invalid,        // 无效指令
        Move,           // 移动
        DragStart,      // 拖拽开始
        Drag,           // 拖拽
        DragEnd,        // 拖拽结束
        Button,         // 按钮指令
        Online,         // 上下线指令 服务器生成该指令
        Gm,             // GM指令 正式服中丢弃该指令
    }
    
    public enum CommandButtonType : byte
    {
        Jump,       // 跳跃
        Attack,     // 攻击
        Skill1,     // 技能1
        Skill2,     // 技能2
        Skill3,     // 技能3
        Skill4,     // 技能4
    }

}