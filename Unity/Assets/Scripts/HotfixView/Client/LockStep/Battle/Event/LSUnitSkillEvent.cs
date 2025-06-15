namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class LSUnitSkillEvent: AEvent<LSWorld, LSUnitSkill>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSUnitSkill args)
        {
            // Room room = lsWorld.GetParent<Room>();
            // LSUnitViewComponent viewComponent = room.GetComponent<LSUnitViewComponent>();
            // if (viewComponent == null)
            //     return;
            // await LSUnitViewFactory.CreateLSUnitViewAsync(viewComponent, args.LSUnit);
        }
    }
}