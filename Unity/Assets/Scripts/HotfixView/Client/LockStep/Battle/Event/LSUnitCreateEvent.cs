namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class LSUnitCreateEvent: AEvent<LSWorld, LSUnitCreate>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSUnitCreate args)
        {
            Room room = lsWorld.GetParent<Room>();
            LSUnitViewComponent viewComponent = room.GetComponent<LSUnitViewComponent>();
            if (viewComponent == null)
                return;
            await LSUnitViewFactory.CreateLSUnitViewAsync(viewComponent, args.LSUnit);
        }
    }
}