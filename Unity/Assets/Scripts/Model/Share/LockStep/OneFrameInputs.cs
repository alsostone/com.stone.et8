using System;
using System.Collections.Generic;

namespace ET
{
    public partial class Room2C_FrameMessage
    {
        protected bool Equals(Room2C_FrameMessage other)
        {
            return Equals(this.Inputs, other.Inputs);
        }

        public void CopyTo(Room2C_FrameMessage to)
        {
            to.Inputs.Clear();
            foreach (var kv in this.Inputs)
            {
                to.Inputs.Add(kv.Key, kv.Value);
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Room2C_FrameMessage) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Inputs);
        }

        public static bool operator==(Room2C_FrameMessage a, Room2C_FrameMessage b)
        {
            if (a is null || b is null)
            {
                if (a is null && b is null)
                {
                    return true;
                }
                return false;
            }
            
            if (a.Inputs.Count != b.Inputs.Count)
            {
                return false;
            }

            foreach (var kv in a.Inputs)
            {
                if (!b.Inputs.TryGetValue(kv.Key, out LSInput inputInfo))
                {
                    return false;
                }

                if (kv.Value != inputInfo)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool operator !=(Room2C_FrameMessage a, Room2C_FrameMessage b)
        {
            return !(a == b);
        }
    }
}