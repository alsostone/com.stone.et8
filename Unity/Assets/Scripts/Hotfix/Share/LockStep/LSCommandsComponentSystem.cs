using System.Collections.Generic;

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
            
            // 按帧号缓存指令的意义
            // 客户端会动态调整自己的Tick频率，尽可能保证服务器收到指令时，服务器还没有Tick到客户端发送指令时的客户端帧号
            // 这样就尽可能的保证了客户端在对比预测帧和服务器帧的指令时，它们是一致的，也就不用回滚了
            // 但当网络时延较大时，客户端的预测帧数量会增大，也就增加了回滚的概率，势必导致表现抖动，同时手机端实时回滚压力会很大
            // 也就可以下一个定论：该套预测机制只能适合PC游戏或时延很低的场景，比如同局域网内
            self.FramesCommandsMove = new List<Queue<ulong>>(LSConstValue.PredictionFrameMaxCount);
            self.FramesCommandsDrag = new List<List<ulong>>(LSConstValue.PredictionFrameMaxCount);
            self.FramesCommandsNormal = new List<List<ulong>>(LSConstValue.PredictionFrameMaxCount);
            for (int i = 0; i < LSConstValue.PredictionFrameMaxCount; ++i)
            {
                self.FramesCommandsMove.Add(new Queue<ulong>());
                self.FramesCommandsDrag.Add(new List<ulong>());
                self.FramesCommandsNormal.Add(new List<ulong>());
            }
        }

        public static void AddCommand(this LSCommandsComponent self, int frame, ulong command)
        {
            // 覆盖座位索引 防止客户端模拟他人消息
            command |= (ulong)self.SeatIndex << 56;
            int index = frame % self.FramesCommandsNormal.Count;
            
            OperateCommandType type = (OperateCommandType)((command >> 48) & 0xFF);
            switch (type)
            {
                case OperateCommandType.Move:
                {
                    // 移动指令最多缓存预测帧的数量 超过则移除最先收到的
                    // 当前还未采用移动单独预表现的方案，所以这里保留最新即可
                    Queue<ulong> commands = self.FramesCommandsMove[index];
                    if (commands.Count > 0 /*LSConstValue.PredictionFrameMaxCount*/)
                        commands.Dequeue();
                    commands.Enqueue(command);
                    break;
                }
                case OperateCommandType.PlacementDragStart:
                {
                    // 指令DragStart新来时，移除缓存的DragStart和Drag
                    List<ulong> commands = self.FramesCommandsDrag[index];
                    for (int i = commands.Count - 1; i >= 0; i++) {
                        OperateCommandType cmdType = (OperateCommandType)((commands[i] >> 48) & 0xFF);
                        if (cmdType == OperateCommandType.PlacementDragStart || cmdType == OperateCommandType.PlacementDrag)
                            commands.RemoveAt(i);
                    }
                    commands.Add(command);
                    break;
                }
                case OperateCommandType.PlacementDrag:
                case OperateCommandType.PlacementDragEnd:
                {
                    // 指令Drag和DragEnd新来时，移除缓存的Drag
                    List<ulong> commands = self.FramesCommandsDrag[index];
                    for (int i = commands.Count - 1; i >= 0; i--) {
                        OperateCommandType cmdType = (OperateCommandType)((commands[i] >> 48) & 0xFF);
                        if (cmdType == OperateCommandType.PlacementDrag)
                            commands.RemoveAt(i);
                    }
                    commands.Add(command);
                    break;
                }
                case OperateCommandType.Button:
                {
                    // 按钮指令新来时，移除缓存的低优先级按钮指令
                    List<ulong> commands = self.FramesCommandsNormal[index];
                    ulong button = (command >> 40) & 0xFF;
                    for (int i = commands.Count - 1; i >= 0; i--) {
                        OperateCommandType cmdType = (OperateCommandType)((commands[i] >> 48) & 0xFF);
                        if (cmdType == OperateCommandType.Button && button >= ((commands[i] >> 40) & 0xFF))
                            commands.RemoveAt(i);
                    }
                    commands.Add(command);
                    break;
                }
                case OperateCommandType.Gm:
                {
                    // GM指令时 若当前是否正式服直接丢弃
                    if (Options.Instance.Develop != 0)
                    {
                        List<ulong> commands = self.FramesCommandsNormal[index];
                        commands.Add(command);
                    }
                    break;
                }
                default: break;
            }
        }
        
        public static void AppendToFrameMessage(this LSCommandsComponent self, int frame, Room2C_FrameMessage frameMessage)
        {
            int index = frame % self.FramesCommandsNormal.Count;
            
            Queue<ulong> commandsMove = self.FramesCommandsMove[index];
            foreach (ulong command in commandsMove)
            {
                frameMessage.Commands.Add(command);
            }
            commandsMove.Clear();
            
            List<ulong> commandsDrag = self.FramesCommandsDrag[index];
            foreach (ulong command in commandsDrag)
            {
                frameMessage.Commands.Add(command);
            }
            commandsDrag.Clear();
            
            List<ulong> commandsNormal = self.FramesCommandsNormal[index];
            foreach (ulong command in commandsNormal)
            {
                frameMessage.Commands.Add(command);
            }
            commandsNormal.Clear();
        }
        
    }
}