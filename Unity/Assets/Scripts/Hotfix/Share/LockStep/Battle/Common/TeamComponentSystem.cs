namespace ET
{
    [EntitySystemOf(typeof(TeamComponent))]
    [FriendOf(typeof(TeamComponent))]
    public static partial class TeamComponentSystem
    {
        [EntitySystem]
        private static void Awake(this TeamComponent self, TeamType type)
        {self.LSRoom()?.ProcessLog.LogFunction(41, self.LSParent().Id);
            self.Type = type;
        }
        
        public static TeamType GetFriendTeam(this TeamComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(98, self.LSParent().Id);
            return self.Type;
        }
        
        public static TeamType GetEnemyTeam(this TeamComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(97, self.LSParent().Id);
            if (self.Type == TeamType.TeamA)
            {
                return TeamType.TeamB;
            }
            if (self.Type == TeamType.TeamB)
            {
                return TeamType.TeamA;
            }
            return TeamType.None;
        }
    }
}