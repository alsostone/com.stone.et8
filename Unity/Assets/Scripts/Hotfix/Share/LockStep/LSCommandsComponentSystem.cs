namespace ET
{
    [EntitySystemOf(typeof(LSCommandsComponent))]
    [FriendOf(typeof(LSCommandsComponent))]
    public static partial class LSCommandsComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSCommandsComponent self, byte seatIndex)
        {
            self.SeatIndex = seatIndex;
        }

        public static void AddCommand(this LSCommandsComponent self, ulong command)
        {
            // 覆盖座位索引 防止客户端模拟他人消息
            command |= (ulong)self.SeatIndex << 56;
            
            OperateCommandType type = (OperateCommandType)((command >> 48) & 0xFF);
            switch (type)
            {
                case OperateCommandType.Move:
                {
                    // 移动指令最多缓存预测帧的数量 超过则移除最先收到的
                    // 当前还未采用移动单独预表现的方案，所以这里保留最新即可
                    if (self.MoveCommands.Count > 0 /*LSConstValue.PredictionFrameMaxCount*/)
                        self.MoveCommands.Dequeue();
                    self.MoveCommands.Enqueue(command);
                    break;
                }
                case OperateCommandType.DragStart:
                {
                    // 指令DragStart新来时，移除缓存的DragStart和Drag
                    for (int i = self.DragCommands.Count - 1; i >= 0; i++) {
                        OperateCommandType cmdType = (OperateCommandType)((self.DragCommands[i] >> 48) & 0xFF);
                        if (cmdType == OperateCommandType.DragStart || cmdType == OperateCommandType.Drag)
                            self.DragCommands.RemoveAt(i);
                    }
                    self.DragCommands.Add(command);
                    break;
                }
                case OperateCommandType.Drag:
                case OperateCommandType.DragEnd:
                {
                    // 指令Drag和DragEnd新来时，移除缓存的Drag
                    for (int i = self.DragCommands.Count - 1; i >= 0; i++) {
                        OperateCommandType cmdType = (OperateCommandType)((self.DragCommands[i] >> 48) & 0xFF);
                        if (cmdType == OperateCommandType.Drag)
                            self.DragCommands.RemoveAt(i);
                    }
                    self.DragCommands.Add(command);
                    break;
                }
                case OperateCommandType.Button:
                {
                    // 按钮指令新来时，移除缓存的低优先级按钮指令
                    ulong button = (command >> 40) & 0xFF;
                    for (int i = self.Commands.Count - 1; i >= 0; i--) {
                        OperateCommandType cmdType = (OperateCommandType)((self.Commands[i] >> 48) & 0xFF);
                        if (cmdType == OperateCommandType.Button && button >= ((self.Commands[i] >> 40) & 0xFF))
                            self.Commands.RemoveAt(i);
                    }
                    self.Commands.Add(command);
                    break;
                }
                case OperateCommandType.Gm:
                {
                    // GM指令时 若当前是否正式服直接丢弃
                    if (Options.Instance.Develop != 0)
                    {
                        self.Commands.Add(command);
                    }
                    break;
                }
                default: break;
            }
        }
        
    }
}