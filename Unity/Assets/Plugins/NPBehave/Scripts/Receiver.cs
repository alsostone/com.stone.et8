using MemoryPack;

namespace NPBehave
{
    /// 接收器只支持 1个Timer + 1个Observer
    /// 可不用但不可多
    public abstract class Receiver
    {
        [MemoryPackInclude] public int Guid { get; protected set; } = -1;

        /// 定时器到达时被调用
        /// 使用Blackboard.AddTimer注册当前节点后才能被调用
        /// Override this method
        public virtual void OnTimerReached()
        {
        }

        /// 监视的值发生变化时被调用
        /// 使用Blackboard.AddObserver注册当前节点后才能被调用
        /// Override this method
        public virtual void OnObservingChanged(NotifyType type)
        {
            
        }
    }

    
}