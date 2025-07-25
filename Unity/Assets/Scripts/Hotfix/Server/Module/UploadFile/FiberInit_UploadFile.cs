﻿using System.Net;

namespace ET.Server
{
    [Invoke((long)SceneType.UploadFile)]
    public class FiberInit_UploadFile: AInvokeHandler<FiberInit, ETTask>
    {
        public override async ETTask Handle(FiberInit fiberInit)
        {
            Scene root = fiberInit.Fiber.Root;
            TbStartSceneRow tbStartSceneRow = TbStartScene.Instance.Get((int)root.Id);
            root.AddComponent<HttpComponent, string>($"http://*:{tbStartSceneRow.Port}/");

            await ETTask.CompletedTask;
        }
    }
}