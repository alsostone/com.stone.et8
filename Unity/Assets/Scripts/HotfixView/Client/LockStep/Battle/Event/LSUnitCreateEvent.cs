namespace ET.Client
{
    [Event(SceneType.LockStep)]
    public class LSUnitCreateEvent: AEvent<Scene, LSUnitCreate>
    {
        protected override async ETTask Run(Scene scene, LSUnitCreate args)
        {
            if (scene.Room().GetComponent<LSUnitViewComponent>() ==null)
                return;
            await LSUnitViewFactory.CreateLSUnitViewAsync(scene, args.LSUnit);
        }
    }
}