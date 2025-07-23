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
        
        public const string ProcessFolderName = "process_log";
        public const string ProcessFolderNameSvr = "process_log_server";
        public const string LSWroldFolderName = "frame_lsworld";
        public const string LSWroldFolderNameSvr = "frame_lsworld_server";
    }

}