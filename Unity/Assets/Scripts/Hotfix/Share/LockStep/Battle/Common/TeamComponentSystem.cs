namespace ET
{
    [EntitySystemOf(typeof(TeamComponent))]
    [FriendOf(typeof(TeamComponent))]
    public static partial class TeamComponentSystem
    {
        [EntitySystem]
        private static void Awake(this TeamComponent self, TeamType type)
        {
            self.Type = type;
        }
        
        public static TeamType GetTeamType(this TeamComponent self)
        {
            return self.Type;
        }
    }
}