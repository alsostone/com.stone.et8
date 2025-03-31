namespace ET
{
    public static class LSConstValue
    {
        public const int MatchCount = 1;
        public const int UpdateInterval = 50;
        public const int FrameCountPerSecond = 1000 / UpdateInterval;
        public const int SaveLSWorldFrameCount = 60 * FrameCountPerSecond;
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