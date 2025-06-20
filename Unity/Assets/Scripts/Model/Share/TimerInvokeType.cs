﻿namespace ET
{
    [UniqueId(100, 10000)]
    public static class TimerInvokeType
    {
        // 框架层100-200，逻辑层的timer type从200起
        public const int WaitTimer = 100;
        public const int SessionIdleChecker = 101;
        public const int MessageLocationSenderChecker = 102;
        public const int MessageSenderChecker = 103;
        
        // 框架层100-200，逻辑层的timer type 200-300
        public const int MoveTimer = 201;
        public const int AITimer = 202;
        public const int SessionAcceptTimeout = 203;
        public const int AnimationTimer = 204;
        
        public const int SkillAniPreTimer = 205;
        public const int SkillAniTimer = 206;
        public const int SkillAniAfterTimer = 207;
        
        public const int RoomUpdate = 301;
    }
}