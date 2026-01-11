using System;
using System.Collections.Generic;
using System.IO;

namespace ET
{
    public class FrameBuffer: Object
    {
        private int maxFrame;
        private readonly List<Room2C_FrameMessage> frameMessages;
        private readonly List<MemoryBuffer> snapshots;

        public FrameBuffer(int frame = 0, int capacity = LSConstValue.FrameCountPerSecond * 60)
        {
            maxFrame = capacity / 10 * 2;   // 80%历史+20%空闲
            
            this.frameMessages = new List<Room2C_FrameMessage>(capacity);
            for (int i = 0; i < capacity; ++i) {
                this.frameMessages.Add(Room2C_FrameMessage.Create());
            }
            
#if !DISABLE_FRAME_SNAPSHOT
            this.snapshots = new List<MemoryBuffer>(capacity);
            for (int i = 0; i < capacity; ++i)
            {
                MemoryBuffer memoryBuffer = new(204800);
                memoryBuffer.SetLength(0);
                memoryBuffer.Seek(0, SeekOrigin.Begin);
                this.snapshots.Add(memoryBuffer);
            }
#endif
        }

        private void EnsureFrame(int frame)
        {
            int minFrame = maxFrame - this.frameMessages.Capacity;
            if (frame < minFrame)
            {
                throw new Exception($"frame out: {frame}, minframe: {minFrame}");
            }
            if (frame > maxFrame)
            {
                throw new Exception($"frame out: {frame}, maxframe: {maxFrame}");
            }
        }
        
        public Room2C_FrameMessage GetFrameMessage(int frame)
        {
            EnsureFrame(frame);
            Room2C_FrameMessage frameMessage = this.frameMessages[frame % this.frameMessages.Capacity];
            return frameMessage;
        }

        public void MoveForward(int frame)
        {
            EnsureFrame(frame);
            ++maxFrame;
            Room2C_FrameMessage frameMessage = this.GetFrameMessage(maxFrame);
            frameMessage.Commands.Clear();
        }
        
#if !DISABLE_FRAME_SNAPSHOT
        public MemoryBuffer Snapshot(int frame)
        {
            EnsureFrame(frame);
            MemoryBuffer memoryBuffer = this.snapshots[frame % this.snapshots.Capacity];
            return memoryBuffer;
        }
#endif
    }
}