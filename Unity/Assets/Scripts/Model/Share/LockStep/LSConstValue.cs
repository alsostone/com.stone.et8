namespace ET
{
    public static class LSConstValue
    {
        public const int MatchCount = 1;
        public const int UpdateInterval = 50;
        public const int Milliseconds = 1000;
        public const int FrameCountPerSecond = Milliseconds / UpdateInterval;
        
        public const long GlobalIdOffset = long.MaxValue; // 约定特殊实体的ID 全局物体-0，中立队伍全局-1 队伍A全局-2 队伍B全局-3...
        public const long PlayerIdOffset = long.MaxValue - 10000; // 约定玩家实体ID段（只在无法获得玩家Id的场景下使用）
        
        public const int PropValueScale = 10000;        // 属性配置放大比例
        public const int Probability = 10000;           // 概率配置以万分比
        public const int PropRuntime2MaxOffset = 1000;

        public const int PredictionFrameMaxCount = 15;  // 预测帧最大值
        public const int SaveLSWorldFrameCount = 60 * FrameCountPerSecond;
        
        public const string ProcessFolderName = "process_log";
        public const string ProcessFolderNameSvr = "process_log_server";
        public const string LSWroldFolderName = "frame_lsworld";
        public const string LSWroldFolderNameSvr = "frame_lsworld_server";
    }
    
    public struct LSRandomDropItem
    {
        public LSRandomDropItem(EUnitType type, int tableId, int count)
        {
            this.Type = type;
            this.TableId = tableId;
            this.Count = count;
        }
        
        public EUnitType Type;
        public int TableId;
        public int Count;
    }
    
}