namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class LSUnitRemoveEvent: AEvent<LSWorld, LSUnitRemove>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSUnitRemove args)
        {
            var comp = lsWorld.GetParent<Room>().GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            comp.RemoveChild(args.Id);
            await ETTask.CompletedTask;
        }
    }
}