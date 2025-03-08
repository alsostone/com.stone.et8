using System.Net;

namespace ET.Server
{
    [Invoke((long)SceneType.NetInner)]
    public class FiberInit_NetInner: AInvokeHandler<FiberInit, ETTask>
    {
        public override async ETTask Handle(FiberInit fiberInit)
        {
            Scene root = fiberInit.Fiber.Root;
            root.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
            root.AddComponent<TimerComponent>();
            root.AddComponent<CoroutineLockComponent>();
            TbStartProcessRow tbStartProcessRow = TbStartProcess.Instance.Get(fiberInit.Fiber.Process);
            root.AddComponent<ProcessOuterSender, IPEndPoint>(tbStartProcessRow.IPEndPoint);
            root.AddComponent<ProcessInnerSender>();

            await ETTask.CompletedTask;
        }
    }
}