namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class LSGridDataResetEvent: AEvent<LSWorld, LSGridDataReset>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSGridDataReset args)
        {
            var room = lsWorld.GetParent<Room>();
            if (room == null)
                return; // 在不一致上报日志文件时，不是Add到Room组件，所以room可能为空
            LSViewGridMapComponent gridMapComponent = room.GetComponent<LSViewGridMapComponent>();
            if (gridMapComponent != null)
                gridMapComponent.RebindGridDataDraw(args.GridData, args.FlowField);
            await ETTask.CompletedTask;
        }
    }
    
}