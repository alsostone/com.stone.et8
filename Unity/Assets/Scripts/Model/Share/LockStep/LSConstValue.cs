namespace ET
{
    public static class LSConstValue
    {
        public const int MatchCount = 1;
        public const int UpdateInterval = 50;
        public const int Milliseconds = 1000;
        public const int FrameCountPerSecond = Milliseconds / UpdateInterval;
        
        public const int Probability = 10000;           // 概率配置以万分比
        public const int PropPrecision = 10000;         // 数值配置以10000倍 以整形替代浮点表示
        
        public const int PredictionFrameMaxCount = 15;  // 预测帧最大值
        public const int SaveLSWorldFrameCount = 60 * FrameCountPerSecond;

        public const int PrecisionMulMillsecond = PropPrecision * Milliseconds;    // 举例：攻速按照10000倍配置 * 毫秒1000 使用该值除攻速的商为CD（毫秒）
    }
    
    public static class LSConstButtonValue
    {
        public const int Jump = 16;
        public const int Attack = 32;
        public const int Skill1 = 64;
        public const int Skill2 = 128;
        public const int Skill3 = 256;
        public const int Skill4 = 512;
    }
}