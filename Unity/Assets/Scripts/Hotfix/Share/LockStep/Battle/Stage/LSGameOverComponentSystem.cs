
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
            LSTargetsComponent lsTargetsComponent = lsWorld.GetComponent<LSTargetsComponent>();
            
            // A方单位的全部死亡则B方获胜
            if (lsTargetsComponent.GetAliveCount(TeamType.TeamA) == 0)
            {
                self.SetGameOver(TeamType.TeamB);
                return;
            }
            
            // 波次结束且B方单位全部死亡则A方获胜
            LSStageComponent lsStageComponent = lsWorld.GetComponent<LSStageComponent>();
            if (!lsStageComponent.CheckStageWaveEnd())
                return;
            if (lsTargetsComponent.GetAliveCount(TeamType.TeamB) > 0)
                return;
            
            self.SetGameOver(TeamType.TeamA);
        }
        
        public static void SetGameOver(this LSGameOverComponent self, TeamType winTeam)
        {
            LSWorld lsWorld = self.LSWorld();
            if (lsWorld.EndFrame > 0 && lsWorld.EndFrame <= lsWorld.Frame)
                return;
            
            lsWorld.EndFrame = lsWorld.Frame;
            self.WinTeam = winTeam;
            EventSystem.Instance.Publish(lsWorld, new LSStageGameOver() {WinTeam = self.WinTeam});
        }
        
    }
}