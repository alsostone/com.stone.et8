namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class LSUnitCastingEvent: AEvent<LSWorld, LSUnitCasting>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSUnitCasting args)
        {
            var comp = lsWorld.GetParent<Room>().GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var viewSkillComponent = view.GetComponent<LSViewSkillComponent>();
            viewSkillComponent?.OnCastSkill(args.SkillId);
            await ETTask.CompletedTask;
        }
    }
}