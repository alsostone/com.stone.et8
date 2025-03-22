namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class LSUnitCreateEvent: AEvent<LSWorld, LSUnitCreate>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSUnitCreate args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.GetComponent<LSUnitViewComponent>() == null)
                return;
            await LSUnitViewFactory.CreateLSUnitViewAsync(room, lsWorld, args.LSUnit);
        }
    }
}