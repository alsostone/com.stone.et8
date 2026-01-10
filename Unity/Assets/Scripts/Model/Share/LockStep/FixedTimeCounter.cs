using TrueSync;

namespace ET
{
    public class FixedTimeCounter: Object
    {
        private long startTime;
        private int startFrame;
        private FP Interval;

        public FixedTimeCounter(long startTime, int startFrame, FP interval)
        {
            this.startTime = startTime;
            this.startFrame = startFrame;
            this.Interval = interval;
        }
        
        public void ChangeInterval(FP interval, int frame)
        {
            this.startTime += ((frame - this.startFrame) * this.Interval).AsLong();
            this.startFrame = frame;
            this.Interval = interval;
        }

        public long FrameTime(int frame)
        {
            return this.startTime + ((frame - this.startFrame) * this.Interval).AsLong();
        }
        
        public void Reset(long time, int frame)
        {
            this.startTime = time;
            this.startFrame = frame;
        }
    }
}