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
    }
}