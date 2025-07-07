namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class LSPlacementDragStartEvent: AEvent<LSWorld, LSPlacementDragStart>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSPlacementDragStart args)
        {
            var comp = lsWorld.GetParent<Room>().GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var builderComponent = view.GetComponent<LSViewBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnPlacementDragStart(args.TargetId);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSPlacementDragEvent: AEvent<LSWorld, LSPlacementDrag>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSPlacementDrag args)
        {
            var comp = lsWorld.GetParent<Room>().GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var builderComponent = view.GetComponent<LSViewBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnPlacementDrag(args.Position);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSPlacementDragEndEvent: AEvent<LSWorld, LSPlacementDragEnd>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSPlacementDragEnd args)
        {
            var comp = lsWorld.GetParent<Room>().GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var builderComponent = view.GetComponent<LSViewBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnPlacementDragEnd(args.Position);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSPlacementStartEvent : AEvent<LSWorld, LSPlacementStart>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSPlacementStart args)
        {
            var comp = lsWorld.GetParent<Room>().GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var builderComponent = view.GetComponent<LSViewBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnPlacementStart(args.Type, args.TableId, args.Level);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSPlacementRotateEvent : AEvent<LSWorld, LSPlacementRotate>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSPlacementRotate args)
        {
            var comp = lsWorld.GetParent<Room>().GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var builderComponent = view.GetComponent<LSViewBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnPlacementRotate(args.Rotation);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSPlacementCancelEvent : AEvent<LSWorld, LSPlacementCancel>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSPlacementCancel args)
        {
            var comp = lsWorld.GetParent<Room>().GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var builderComponent = view.GetComponent<LSViewBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnPlacementCancel();
            await ETTask.CompletedTask;
        }
    }
}