using System.Collections.Generic;

namespace ET
{
    [EntitySystemOf(typeof(TeamComponent))]
    [FriendOf(typeof(TeamComponent))]
    public static partial class TeamComponentSystem
    {
        [EntitySystem]
        private static void Awake(this TeamComponent self, TeamType type)
        {self.LSRoom()?.ProcessLog.LogFunction(84, self.LSParent().Id);
            self.Type = type;
        }
        
        public static TeamType GetOwnerTeam(this TeamComponent self)
        {
            return self.Type;
        }
        
        public static TeamType GetOppositeTeam(this TeamComponent self)
        {
            return self.Type == TeamType.TeamA ? TeamType.TeamB : TeamType.TeamA;
        }
        
        public static IList<TeamType> GetFriendTeams(this TeamComponent self)
        {
            var teams = ObjectPool.Instance.Fetch<List<TeamType>>();
            teams.Add(self.Type);
            return teams;
        }
        
        public static IList<TeamType> GetEnemyTeams(this TeamComponent self)
        {
            var teams = ObjectPool.Instance.Fetch<List<TeamType>>();
            switch (self.Type)
            {
                case TeamType.TeamNeutral:
                    teams.Add(TeamType.TeamA);
                    teams.Add(TeamType.TeamB);
                    break;
                case TeamType.TeamA:
                    teams.Add(TeamType.TeamB);
                    teams.Add(TeamType.TeamNeutral);
                    break;
                case TeamType.TeamB:
                    teams.Add(TeamType.TeamA);
                    teams.Add(TeamType.TeamNeutral);
                    break;
            }
            return teams;
        }
        
        public static IList<TeamType> GetAllTeams(this TeamComponent self)
        {
            var teams = ObjectPool.Instance.Fetch<List<TeamType>>();
            teams.Add(TeamType.TeamA);
            teams.Add(TeamType.TeamB);
            teams.Add(TeamType.TeamNeutral);
            return teams;
        }
    }
}