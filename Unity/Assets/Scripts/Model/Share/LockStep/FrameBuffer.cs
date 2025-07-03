using System;
using System.Collections.Generic;
using System.IO;

namespace ET
{
    public class FrameBuffer: Object
    {
        public int MaxFrame { get; private set; }
        private readonly List<Room2C_FrameMessage> frameMessages;
        private readonly List<MemoryBuffer> snapshots;
        private readonly List<long> hashs;

        public FrameBuffer(int frame = 0, int capacity = LSConstValue.FrameCountPerSecond * 60)
        {
            this.MaxFrame = frame + LSConstValue.FrameCountPerSecond * 30;
            this.frameMessages = new List<Room2C_FrameMessage>(capacity);
            this.snapshots = new List<MemoryBuffer>(capacity);
            this.hashs = new List<long>(capacity);
            
            for (int i = 0; i < this.snapshots.Capacity; ++i)
            {
                this.hashs.Add(0);
                this.frameMessages.Add(Room2C_FrameMessage.Create());
                MemoryBuffer memoryBuffer = new(10240);
                memoryBuffer.SetLength(0);
                memoryBuffer.Seek(0, SeekOrigin.Begin);
                this.snapshots.Add(memoryBuffer);
            }
        }

        public void SetHash(int frame, long hash)
        {
            EnsureFrame(frame);
            this.hashs[frame % this.frameMessages.Capacity] = hash;
        }
        
        public long GetHash(int frame)
        {
            EnsureFrame(frame);
            return this.hashs[frame % this.frameMessages.Capacity];
        }

        public bool CheckFrame(int frame)
        {
            if (frame < 0)
            {
                return false;
            }

            if (frame > this.MaxFrame)
            {
                return false;
            }

            return true;
        }

        private void EnsureFrame(int frame)
        {
            if (!CheckFrame(frame))
            {
                throw new Exception($"frame out: {frame}, maxframe: {this.MaxFrame}");
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
            if (this.MaxFrame - frame > LSConstValue.FrameCountPerSecond) // 至少留出1秒的空间
            {
                return;
            }
            
            ++this.MaxFrame;
            
            Room2C_FrameMessage frameMessage = this.GetFrameMessage(this.MaxFrame);
            frameMessage.Inputs.Clear();
        }

        public MemoryBuffer Snapshot(int frame)
        {
            EnsureFrame(frame);
            MemoryBuffer memoryBuffer = this.snapshots[frame % this.snapshots.Capacity];
            return memoryBuffer;
        }
    }
}