using System;
using TrueSync;

namespace ET.Client
{
    [MessageHandler(SceneType.LockStep)]
    public class Room2C_TimeAdjustHandler: MessageHandler<Scene, Room2C_TimeAdjust>
    {
        protected override async ETTask Run(Scene root, Room2C_TimeAdjust message)
        {
            using Room2C_TimeAdjust _ = message;  // 让消息回到池中
            
            Room room = root.GetComponent<Room>();
            if (room.LockStepMode <= LockStepMode.Local)
                return; // 防御 非联网模式不处理这个消息
            
            FP diff = (message.DiffTime - LSConstValue.UpdateInterval) / 1000;  // 额外慢一帧 以确保客户端快于服务器一帧
            FP interval = LSConstValue.UpdateInterval * (1 + diff);
            room.FixedTimeCounter.ChangeInterval(TSMath.Clamp(interval, 40, 66), room.PredictionFrame);
            await ETTask.CompletedTask;
        }
    }
}