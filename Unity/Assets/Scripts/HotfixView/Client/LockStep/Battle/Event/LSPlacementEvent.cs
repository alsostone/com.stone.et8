namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class LSEscapeEvent: AEvent<LSWorld, LSEscape>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSEscape args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var builderComponent = view.GetComponent<LSViewGridBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnEscape();
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSTouchDragStartEvent: AEvent<LSWorld, LSTouchDragStart>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSTouchDragStart args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var builderComponent = view.GetComponent<LSViewGridBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnTouchDragStart(args.Position);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSTouchDragEvent: AEvent<LSWorld, LSTouchDrag>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSTouchDrag args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var builderComponent = view.GetComponent<LSViewGridBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnTouchDrag(args.Position);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSTouchDragEndEvent: AEvent<LSWorld, LSTouchDragEnd>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSTouchDragEnd args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var builderComponent = view.GetComponent<LSViewGridBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnTouchDragEnd();
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSTouchDragCancelEvent : AEvent<LSWorld, LSTouchDragCancel>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSTouchDragCancel args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var builderComponent = view.GetComponent<LSViewGridBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnTouchDragCancel();
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSPlacementDragEvent: AEvent<LSWorld, LSPlacementDrag>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSPlacementDrag args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var builderComponent = view.GetComponent<LSViewGridBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnPlacementDrag(args.TargetId);
            await ETTask.CompletedTask;
        }
    }

    [Event(SceneType.LockStepClient)]
    public class LSPlacementNewEvent : AEvent<LSWorld, LSPlacementNew>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSPlacementNew args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var builderComponent = view.GetComponent<LSViewGridBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnPlacementNew(args.ItemId);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSPlacementRotateEvent : AEvent<LSWorld, LSPlacementRotate>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSPlacementRotate args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var builderComponent = view.GetComponent<LSViewGridBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnPlacementRotate(args.Rotation);
            await ETTask.CompletedTask;
        }
    }
    
}