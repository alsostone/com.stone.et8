namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class LSStageGameOverEvent: AEvent<LSWorld, LSStageGameOver>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSStageGameOver args)
        {
            var room = lsWorld.GetParent<Room>();
            
            // 设置结算数据 (结算UI所用的一切信息都缓存到 LSViewGameOverComponent 中)
            LSViewGameOverComponent viewGameOverComponent = room.GetComponent<LSViewGameOverComponent>();
            viewGameOverComponent.SetGameOver(args.WinTeam);
            
            // 显示结算界面
            YIUIRootComponent yiuiRootComponent = room.GetComponent<YIUIRootComponent>();
            await yiuiRootComponent.OpenPanelAsync<SettlePanelComponent>();
            
            await ETTask.CompletedTask;
        }
    }
    
}