namespace ET
{
    public static class AIConstValue
    {
        public const int AwarenessSearchEnemy = 98010001;            // AI中感知敌人使用的索敌配置
        public const int AwarenessSearchFriend = 98010002;           // AI中感知友军使用的索敌配置

        public const string IsStateOfAlert = "IsStateOfAlert";      // 是否处于警戒状态，即发现警戒范围内有敌人
        public const string HasEnemyCount = "HasEnemyCount";        // 周围敌人数量
        public const string HasEnemyEntityId = "HasEnemyEntityId";  // 周围的第一个敌人Id
    }
}
