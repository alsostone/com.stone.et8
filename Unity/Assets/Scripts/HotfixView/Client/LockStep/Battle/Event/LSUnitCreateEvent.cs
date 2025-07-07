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
            LSUnitViewFactory.CreateLSUnitView(viewComponent, args.LSUnit);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSUnitPlacedEvent: AEvent<LSWorld, LSUnitPlaced>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSUnitPlaced args)
        {
            Room room = lsWorld.GetParent<Room>();
            LSUnitViewComponent comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            LSViewPlacementComponent placementComponent = view.GetComponent<LSViewPlacementComponent>();
            if (placementComponent == null)
                return;
            placementComponent.Placed(args.X, args.Z);
            await ETTask.CompletedTask;
        }
    }
}