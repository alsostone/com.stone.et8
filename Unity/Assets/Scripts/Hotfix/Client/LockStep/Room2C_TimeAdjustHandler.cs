namespace ET.Client
{
    [MessageHandler(SceneType.LockStep)]
    public class Room2C_TimeAdjustHandler: MessageHandler<Scene, Room2C_TimeAdjust>
    {
        protected override async ETTask Run(Scene root, Room2C_TimeAdjust message)
        {
            using Room2C_TimeAdjust _ = message;  // 让消息回到池中
            
            Room room = root.GetComponent<Room>();
            int newInterval = (1000 + (message.DiffTime - LSConstValue.UpdateInterval)) * LSConstValue.UpdateInterval / 1000;

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