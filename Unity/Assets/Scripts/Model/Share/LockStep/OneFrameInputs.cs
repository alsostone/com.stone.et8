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