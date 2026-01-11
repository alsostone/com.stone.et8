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
    
    [Event(SceneType.LockStepClient)]
    public class LSTimeScaleChangedEvent: AEvent<LSWorld, LSTimeScaleChanged>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSTimeScaleChanged args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            
            float timeScale = room.TimeScale;
            LSUnitViewComponent lsUnitViewComponent = room.GetComponent<LSUnitViewComponent>();
            lsUnitViewComponent.ResetTimeScale(timeScale);
            
            
            await ETTask.CompletedTask;
        }
    }

}