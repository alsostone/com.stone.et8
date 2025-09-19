namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class LSUnitCardSelectAddEvent: AEvent<LSWorld, LSCardSelectAdd>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSCardSelectAdd args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var viewCardSelectComponent = view.GetComponent<LSViewCardSelectComponent>();
            viewCardSelectComponent.AddCards(args.Cards);
            
            PlayViewComponent viewComponent = YIUIMgrComponent.Inst.GetPanelView<LSRoomPanelComponent, PlayViewComponent>();
            viewComponent?.ResetSelectCards(viewCardSelectComponent.CardsQueue);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSUnitCardSelectDoneEvent: AEvent<LSWorld, LSCardSelectDone>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSCardSelectDone args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var viewCardSelectComponent = view.GetComponent<LSViewCardSelectComponent>();
            viewCardSelectComponent.SelectCards(args.Index);
            
            PlayViewComponent viewComponent = YIUIMgrComponent.Inst.GetPanelView<LSRoomPanelComponent, PlayViewComponent>();
            viewComponent?.ResetSelectCards(viewCardSelectComponent.CardsQueue);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSUnitCardBagAddEvent: AEvent<LSWorld, LSCardBagAdd>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSCardBagAdd args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var viewCardBagComponent = view.GetComponent<LSViewCardBagComponent>();
            viewCardBagComponent.AddItem(args.Item);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSUnitCardBagRemoveEvent: AEvent<LSWorld, LSCardBagRemove>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSCardBagRemove args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room.IsRollback)
                return; // 不响应回滚过程中的消息。原因：1.RollbackSystem还未执行，单位可能不存在；2.回滚相关的所有恢复操作都应由RollbackSystem处理。
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var viewCardBagComponent = view.GetComponent<LSViewCardBagComponent>();
            viewCardBagComponent.RemoveItem(args.ItemId);
            await ETTask.CompletedTask;
        }
    }
    
}