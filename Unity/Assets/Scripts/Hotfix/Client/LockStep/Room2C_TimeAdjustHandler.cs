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
            
            int diff = message.DiffTime - LSConstValue.UpdateInterval;  // 额外慢一帧 以确保客户端快于服务器一帧
            int newInterval = (1000 + diff) * LSConstValue.UpdateInterval / 1000;

            if (newInterval < 40)
            {
                newInterval = 40;
            }

            if (newInterval > 66)
            {
                newInterval = 66;
            }
            
            room.FixedTimeCounter.ChangeInterval(newInterval, room.PredictionFrame);
            await ETTask.CompletedTask;
        }
    }
}