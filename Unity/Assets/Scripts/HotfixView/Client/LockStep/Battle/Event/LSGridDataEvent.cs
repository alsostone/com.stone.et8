namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class LSGridDataResetEvent: AEvent<LSWorld, LSGridDataReset>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSGridDataReset args)
        {
            Room room = lsWorld.GetParent<Room>();
            LSViewGridMapComponent gridMapComponent = room.GetComponent<LSViewGridMapComponent>();
            if (gridMapComponent != null)
                gridMapComponent.RebindGridDataDraw();
            await ETTask.CompletedTask;
        }
    }
    
}