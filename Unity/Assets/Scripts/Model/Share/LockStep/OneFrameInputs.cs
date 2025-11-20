using System;
using System.Collections.Generic;

namespace ET
{
    public partial class Room2C_FrameMessage
    {
        public void CopyTo(Room2C_FrameMessage to)
        {
            to.Frame = Frame;
            to.FrameIndex = FrameIndex;
            to.Commands.Clear();
            to.Commands.AddRange(Commands);
        }
        
        // 用于将本地已执行但服务器未执行的指令移动到本地的下一帧
        // 如：本地执行指令1、2、3、4、5，服务器只执行3，那么认定1、2、3过期，4、5插入到下一帧
        public void InsertTo(Room2C_FrameMessage to, Room2C_FrameMessage authority)
        {
            if (authority.Commands.Count > 0)
            {
                byte diffVersion = LSCommand.ParseCommandVersion(authority.Commands[^1]);
                for (int i = Commands.Count - 1; i >= 0; i--)
                {
                    LSCommandData lsCommand = Commands[i];
                    byte cmdVersion = LSCommand.ParseCommandVersion(lsCommand);
                    int forwardDis = (cmdVersion - diffVersion + 256) % 256;
                    if (forwardDis == 0 || forwardDis > 128) // 过期指令
                        continue;
                    to.Commands.Insert(0, lsCommand);
                }
            }
            else
            {
                to.Commands.InsertRange(0, Commands);
            }
        }
        
        public bool Equals(Room2C_FrameMessage other)
        {
            if (other is null)
            {
                return false;
            }

            if (Frame != other.Frame || FrameIndex != other.FrameIndex)
            {
                return false;
            }

            if (Commands.Count != other.Commands.Count)
            {
                return false;
            }

            for (int index = 0; index < Commands.Count; index++)
            {
                if (Commands[index] != other.Commands[index])
                {
                    return false;
                }
            }

            return true;
        }
    }
}