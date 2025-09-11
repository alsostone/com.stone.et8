
namespace ET
{
    [LSEntitySystemOf(typeof(LSGameOverComponent))]
    [EntitySystemOf(typeof(LSGameOverComponent))]
    [FriendOf(typeof(LSGameOverComponent))]
    public static partial class LSGameOverComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSGameOverComponent self, TbStageRow stageRow)
        {
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this LSGameOverComponent self)
        {
            LSWorld lsWorld = self.LSWorld();
            // if (lsWorld.Frame == 100)
            // {
            //     self.WinTeam = TeamType.TeamA;
            //     EventSystem.Instance.Publish(lsWorld, new LSStageGameOver() {WinTeam = self.WinTeam});
            // }
        }
        
    }
}