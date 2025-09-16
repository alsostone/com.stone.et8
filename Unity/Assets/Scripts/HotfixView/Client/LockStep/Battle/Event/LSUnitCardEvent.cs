namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class LSUnitCardSelectAddEvent: AEvent<LSWorld, LSCardSelectAdd>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSCardSelectAdd args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room == null)
                return; // 在不一致上报日志文件时，不是Add到Room组件，所以room可能为空
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var viewCardSelectComponent = view.GetComponent<LSViewCardSelectComponent>();
            viewCardSelectComponent.AddCards(args.Cards);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSUnitCardSelectDoneEvent: AEvent<LSWorld, LSCardSelectDone>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSCardSelectDone args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room == null)
                return; // 在不一致上报日志文件时，不是Add到Room组件，所以room可能为空
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var viewCardSelectComponent = view.GetComponent<LSViewCardSelectComponent>();
            viewCardSelectComponent.SelectCards(args.Index);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSUnitCardBagAddEvent: AEvent<LSWorld, LSCardBagAdd>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSCardBagAdd args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room == null)
                return; // 在不一致上报日志文件时，不是Add到Room组件，所以room可能为空
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var viewCardBagComponent = view.GetComponent<LSViewCardBagComponent>();
            viewCardBagComponent.AddCard(args.Type, args.TableId, args.Count);
            await ETTask.CompletedTask;
        }
    }
    
    [Event(SceneType.LockStepClient)]
    public class LSUnitCardBagRemoveEvent: AEvent<LSWorld, LSCardBagRemove>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSCardBagRemove args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room == null)
                return; // 在不一致上报日志文件时，不是Add到Room组件，所以room可能为空
            var comp = room.GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            var viewCardBagComponent = view.GetComponent<LSViewCardBagComponent>();
            viewCardBagComponent.RemoveCard(args.Type, args.TableId, args.Count);
            await ETTask.CompletedTask;
        }
    }
    
}