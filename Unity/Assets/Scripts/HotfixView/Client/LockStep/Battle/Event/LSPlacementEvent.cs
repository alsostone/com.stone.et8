namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class LSPlacementDragStartEvent: AEvent<LSWorld, LSPlacementDragStart>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSPlacementDragStart args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var builderComponent = room.GetComponent<LSViewGridBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnPlacementDragStart(args.TeamPlacer, args.TargetId);
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
            var builderComponent = room.GetComponent<LSViewGridBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnPlacementDrag(args.TeamPlacer, args.Position);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSPlacementDragEndEvent: AEvent<LSWorld, LSPlacementDragEnd>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSPlacementDragEnd args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var builderComponent = room.GetComponent<LSViewGridBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnPlacementDragEnd(args.TeamPlacer, args.Position);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSPlacementStartEvent : AEvent<LSWorld, LSPlacementStart>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSPlacementStart args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var builderComponent = room.GetComponent<LSViewGridBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnPlacementStart(args.TeamPlacer, args.Type, args.TableId);
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
            var builderComponent = room.GetComponent<LSViewGridBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnPlacementRotate(args.TeamPlacer, args.Rotation);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSPlacementCancelEvent : AEvent<LSWorld, LSPlacementCancel>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSPlacementCancel args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var builderComponent = room.GetComponent<LSViewGridBuilderComponent>();
            if (builderComponent == null)
                return;
            builderComponent.OnPlacementCancel(args.TeamPlacer);
            await ETTask.CompletedTask;
        }
    }
}