namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class LSUnitRemoveEvent: AEvent<LSWorld, LSUnitRemove>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSUnitRemove args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            comp.RemoveChild(args.Id);
            await ETTask.CompletedTask;
        }
    }
}