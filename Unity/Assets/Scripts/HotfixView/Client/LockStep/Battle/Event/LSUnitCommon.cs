namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class LSUnitCreateEvent: AEvent<LSWorld, LSUnitCreate>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSUnitCreate args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            LSUnitViewFactory.CreateLSUnitView(comp, args.LSUnit);
            await ETTask.CompletedTask;
        }
    }
    
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
    
    [Event(SceneType.LockStepClient)]
    public class LSUnitPositionEvent: AEvent<LSWorld, LSUnitPosition>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSUnitPosition args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            view.GetComponent<LSViewTransformComponent>().SetPosition(args.Position.ToVector(), args.Immediate);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSUnitRotationEvent: AEvent<LSWorld, LSUnitRotation>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSUnitRotation args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            view.GetComponent<LSViewTransformComponent>().SetRotation(args.Rotation.ToQuaternion(), args.Immediate);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSUnitMovingEvent: AEvent<LSWorld, LSUnitMoving>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSUnitMoving args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var animationComponent = view.GetComponent<LSAnimationComponent>();
            if (args.IsMoving) {
                animationComponent?.AddAnimation(AnimationNames.Run);
            } else {
                animationComponent?.RemoveAnimation(AnimationNames.Run, 1);
            }
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSUnitPlacedEvent: AEvent<LSWorld, LSUnitPlaced>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSUnitPlaced args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
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