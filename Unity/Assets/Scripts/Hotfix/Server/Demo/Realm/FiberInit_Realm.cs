﻿using System.Net;

namespace ET.Server
{
    [Invoke((long)SceneType.Realm)]
    public class FiberInit_Realm: AInvokeHandler<FiberInit, ETTask>
    {
        public override async ETTask Handle(FiberInit fiberInit)
        {
            Scene root = fiberInit.Fiber.Root;
            root.AddComponent<MailBoxComponent, MailBoxType>(MailBoxType.UnOrderedMessage);
            root.AddComponent<TimerComponent>();
            root.AddComponent<CoroutineLockComponent>();
            root.AddComponent<ProcessInnerSender>();
            root.AddComponent<MessageSender>();
            TbStartSceneRow tbStartSceneRow = TbStartScene.Instance.Get(root.Fiber.Id);
            root.AddComponent<NetComponent, IPEndPoint, NetworkProtocol>(tbStartSceneRow.InnerIPPort, NetworkProtocol.UDP);

            await ETTask.CompletedTask;
        }
    }
}