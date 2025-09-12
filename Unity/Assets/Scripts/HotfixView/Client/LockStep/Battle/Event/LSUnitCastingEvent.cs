namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class LSUnitCastingEvent: AEvent<LSWorld, LSUnitCasting>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSUnitCasting args)
        {
            var room = lsWorld.Room();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var viewSkillComponent = view.GetComponent<LSViewSkillComponent>();
            viewSkillComponent?.OnCastSkill(args.SkillId);
            await ETTask.CompletedTask;
        }
    }
}