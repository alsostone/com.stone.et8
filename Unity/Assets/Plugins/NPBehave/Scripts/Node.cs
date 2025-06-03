using MemoryPack;

namespace NPBehave
{
    [MemoryPackable(GenerateType.NoGenerate)]
    public abstract partial class Node
    {
        public enum State
        {
            INACTIVE,
            ACTIVE,
            STOP_REQUESTED,
        }

        [MemoryPackInclude]
        protected State currentState = State.INACTIVE;

        [MemoryPackIgnore]
        public State CurrentState
        {
            get { return currentState; }
        }

        [MemoryPackIgnore]
        protected Root RootNode;

        private Container parentNode;
        [MemoryPackIgnore]
        public Container ParentNode
        {
            get
            {
                return parentNode;
            }
        }

        private string label;
        [MemoryPackInclude]
        public string Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
            }
        }

        private readonly string name;

        [MemoryPackIgnore]
        public string Name
        {
            get
            {
                return name;
            }
        }

        [MemoryPackIgnore]
        public Blackboard Blackboard
        {
            get
            {
                return RootNode.RootBlackboard;
            }
        }

        [MemoryPackIgnore]
        public Clock Clock
        {
            get
            {
                return RootNode.RootClock;
            }
        }

        [MemoryPackIgnore]
        public bool IsStopRequested
        {
            get
            {
                return this.currentState == State.STOP_REQUESTED;
            }
        }

        [MemoryPackIgnore]
        public bool IsActive
        {
            get
            {
                return this.currentState == State.ACTIVE;
            }
        }


        protected Node(string name)
        {
            this.name = name;
        }

        public virtual void SetRoot(Root rootNode)
        {
            this.RootNode = rootNode;
        }

        public void SetParent(Container parent)
        {
            this.parentNode = parent;
        }

#if UNITY_EDITOR
        [MemoryPackIgnore] public float DebugLastStopRequestAt = 0.0f;
        [MemoryPackIgnore] public float DebugLastStoppedAt = 0.0f;
        [MemoryPackIgnore] public int DebugNumStartCalls = 0;
        [MemoryPackIgnore] public int DebugNumStopCalls = 0;
        [MemoryPackIgnore] public int DebugNumStoppedCalls = 0;
        [MemoryPackIgnore] public bool DebugLastResult = false;
#endif

        public void Start()
        {
#if UNITY_EDITOR
            RootNode.TotalNumStartCalls++;
            this.DebugNumStartCalls++;
#endif
            this.currentState = State.ACTIVE;
            DoStart();
        }

        /// <summary>
        /// TODO: Rename to "Cancel" in next API-Incompatible version
        /// </summary>
        public void Stop()
        {
            this.currentState = State.STOP_REQUESTED;
#if UNITY_EDITOR
            RootNode.TotalNumStopCalls++;
            this.DebugLastStopRequestAt = UnityEngine.Time.time;
            this.DebugNumStopCalls++;
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
            this.currentState = State.INACTIVE;
#if UNITY_EDITOR
            RootNode.TotalNumStoppedCalls++;
            this.DebugNumStoppedCalls++;
            this.DebugLastStoppedAt = UnityEngine.Time.time;
            DebugLastResult = success;
#endif
            if (this.ParentNode != null)
            {
                this.ParentNode.ChildStopped(this, success);
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
            return !string.IsNullOrEmpty(Label) ? (this.Name + "{"+Label+"}") : this.Name;
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