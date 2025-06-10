using System;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace NPBehave
{
    [MemoryPackable(GenerateType.NoGenerate)]
    public abstract partial class Node : Receiver, IDisposable
    {
        public enum State
        {
            INACTIVE,
            ACTIVE,
            STOP_REQUESTED,
        }
        
        [BsonElement][MemoryPackInclude] protected State currentState = State.INACTIVE;
        
        [BsonIgnore][MemoryPackIgnore] private string label;
        [BsonIgnoreIfNull][BsonElement][MemoryPackInclude] public string Label { get => label; set => label = value; }
        
        [BsonIgnore][MemoryPackIgnore] private string name;
        [BsonIgnore][MemoryPackIgnore] public string Name => name;
        
        [BsonIgnore][MemoryPackIgnore] public State CurrentState => currentState;
        [BsonIgnore][MemoryPackIgnore] protected Root RootNode { get; set; }
        [BsonIgnore][MemoryPackIgnore] public Container ParentNode { get; private set; }
        [BsonIgnore][MemoryPackIgnore] public Blackboard Blackboard => RootNode.RootBlackboard;
        [BsonIgnore][MemoryPackIgnore] public Clock Clock => RootNode.RootClock;
        [BsonIgnore][MemoryPackIgnore] public bool IsStopRequested => currentState == State.STOP_REQUESTED;
        [BsonIgnore][MemoryPackIgnore] public bool IsActive => currentState == State.ACTIVE;
        
        protected Node(string name)
        {
            this.name = name;
            NodeFormatter.TryAddFormatter(this);
        }

        // 防止因为循环依赖导致无法GC
        public virtual void Dispose()
        {
            RootNode = null;
            ParentNode = null;
        }

        public virtual void SetRoot(Root rootNode)
        {
            RootNode = rootNode;
            
            // 注册到黑板的意义：通过Guid找到该节点，后调用该节点的方法
            if (Guid <= 0)
                Guid = RootNode.RootBehaveWorld.GetNextGuid();
            RootNode.RootBehaveWorld.GuidReceiverMapping.Add(Guid, this);
        }

        public void SetParent(Container parent)
        {
            ParentNode = parent;
        }

#if UNITY_EDITOR
        [BsonIgnore][MemoryPackIgnore] public FP DebugLastStopRequestAt = FP.Zero;
        [BsonIgnore][MemoryPackIgnore] public FP DebugLastStoppedAt = FP.Zero;
        [BsonIgnore][MemoryPackIgnore] public int DebugNumStartCalls = 0;
        [BsonIgnore][MemoryPackIgnore] public int DebugNumStopCalls = 0;
        [BsonIgnore][MemoryPackIgnore] public int DebugNumStoppedCalls = 0;
        [BsonIgnore][MemoryPackIgnore] public bool DebugLastResult = false;
#endif

        public void Start()
        {
#if UNITY_EDITOR
            RootNode.TotalNumStartCalls++;
            DebugNumStartCalls++;
#endif
            currentState = State.ACTIVE;
            DoStart();
        }

        /// <summary>
        /// TODO: Rename to "Cancel" in next API-Incompatible version
        /// </summary>
        public void Stop()
        {
            currentState = State.STOP_REQUESTED;
#if UNITY_EDITOR
            RootNode.TotalNumStopCalls++;
            DebugLastStopRequestAt = UnityEngine.Time.time;
            DebugNumStopCalls++;
#endif
            DoStop();
        }

        protected virtual void DoStart()
        {

        }

        protected virtual void DoStop()
        {

        }


        /// THIS ABSOLUTLY HAS TO BE THE LAST CALL IN YOUR FUNCTION, NEVER MODIFY
        /// ANY STATE AFTER CALLING Stopped !!!!
        protected virtual void Stopped(bool success)
        {
            currentState = State.INACTIVE;
#if UNITY_EDITOR
            RootNode.TotalNumStoppedCalls++;
            DebugNumStoppedCalls++;
            DebugLastStoppedAt = UnityEngine.Time.time;
            DebugLastResult = success;
#endif
            if (ParentNode != null)
            {
                ParentNode.ChildStopped(this, success);
            }
        }

        public virtual void ParentCompositeStopped(Composite composite)
        {
            DoParentCompositeStopped(composite);
        }

        /// THIS IS CALLED WHILE YOU ARE INACTIVE, IT's MEANT FOR DECORATORS TO REMOVE ANY PENDING
        /// OBSERVERS
        protected virtual void DoParentCompositeStopped(Composite composite)
        {
            /// be careful with this!
        }
        
        public override string ToString()
        {
            return !string.IsNullOrEmpty(Label) ? (Name + "{"+Label+"}") : Name;
        }

        protected string GetPath()
        {
            if (ParentNode != null)
            {
                return ParentNode.GetPath() + "/" + Name;
            }
            else
            {
                return Name;
            }
        }
    }
}