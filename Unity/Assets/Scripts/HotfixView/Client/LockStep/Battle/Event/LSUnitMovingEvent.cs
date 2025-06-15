namespace ET.Client
{
    [Event(SceneType.LockStepClient)]
    public class LSUnitMovingEvent: AEvent<LSWorld, LSUnitMoving>
    {
        protected override async ETTask Run(LSWorld lsWorld, LSUnitMoving args)
        {
            var comp = lsWorld.GetParent<Room>().GetComponent<LSUnitViewComponent>();
            if (comp == null)
                return;
            var view = comp.GetChild<LSUnitView>(args.Id);
            if (view == null) return;   // 创建是异步的 可能未完毕
            
            var animationComponent = view.GetComponent<LSAnimationComponent>();
            if (args.IsMoving) {
                animationComponent?.AddAnimation(AnimationNames.Run);
            } else {
                animationComponent?.RemoveAnimation(AnimationNames.Run, 1);
            }
            await ETTask.CompletedTask;
        }
    }
}