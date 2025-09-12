
namespace ET.Client
{
    [EntitySystemOf(typeof(LSViewGameOverComponent))]
    [FriendOf(typeof(LSViewGameOverComponent))]
    public static partial class LSViewGameOverComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSViewGameOverComponent self)
        {
        }
        
        public static void SetGameOver(this LSViewGameOverComponent self, TeamType winTeam)
        {
            LSWorld lsWorld = self.Room().LSWorld;
            
            self.WinTeam = winTeam;
            self.EndFrame = lsWorld.EndFrame;
            
            LSUnitComponent unitComponent = lsWorld.GetComponent<LSUnitComponent>();
            LSUnit lsUnit = unitComponent.GetChild<LSUnit>(self.Room().OwnerPlayerId);
            self.IsWin = lsUnit.GetComponent<TeamComponent>().Type == winTeam;
            
            // 结算UI用到的统计信息
            // ...
        }
        
    }
}