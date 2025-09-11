namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class LSStageGameOverEvent: AEvent<LSWorld, LSStageGameOver>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSStageGameOver args)
        {
            var room = lsWorld.GetParent<Room>();
            
            // 显示结算界面
            YIUIRootComponent yiuiRootComponent = room.GetComponent<YIUIRootComponent>();
            await yiuiRootComponent.OpenPanelAsync<SettlePanelComponent>();
            
            await ETTask.CompletedTask;
        }
    }
    
}