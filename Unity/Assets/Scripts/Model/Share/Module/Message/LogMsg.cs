﻿using System.Collections.Generic;
using System.Diagnostics;

namespace ET
{
    public class LogMsg: Singleton<LogMsg>, ISingletonAwake
    {
        private readonly HashSet<ushort> ignore = new()
        {
            OuterMessage.C2G_Ping, 
            OuterMessage.G2C_Ping, 
            OuterMessage.C2G_Benchmark, 
            OuterMessage.G2C_Benchmark,
            LockStepOuter.Room2C_FrameMessage,
            LockStepOuter.C2Room_FrameMessage,
            LockStepOuter.C2Room_CheckHash,
            LockStepOuter.Room2C_TimeAdjust,
            LockStepOuter.C2Room_TimeAdjust,
            LockStepOuter.Room2C_CheckHashFail,
        };

        public void Awake()
        {
        }

        [Conditional("DEBUG")]
        public void Debug(Fiber fiber, object msg)
        {
            ushort opcode = OpcodeType.Instance.GetOpcode(msg.GetType());
            if (this.ignore.Contains(opcode))
            {
                return;
            }
            fiber.Log.Debug(msg.ToString());
        }
    }
}