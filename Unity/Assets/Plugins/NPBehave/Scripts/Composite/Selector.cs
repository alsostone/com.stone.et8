using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class Selector : Composite
    {
        [MemoryPackInclude] private int currentIndex = -1;

        public Selector(params Node[] children) : base("Selector", children)
        {
        }
        
        protected override void DoStart()
        {
            currentIndex = -1;

            ProcessChildren();
        }

        protected override void DoStop()
        {
            Children[currentIndex].Stop();
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            if (result)
            {
                Stopped(true);
            }
            else
            {
                ProcessChildren();
            }
        }

        private void ProcessChildren()
        {
            if (++currentIndex < Children.Length)
            {
                if (IsStopRequested)
                {
                    Stopped(false);
                }
                else
                {
                    Children[currentIndex].Start();
                }
            }
            else
            {
                Stopped(false);
            }
        }

        public override void StopLowerPriorityChildrenForChild(Node abortForChild, bool immediateRestart)
        {
            int indexForChild = 0;
            bool found = false;
            foreach (Node currentChild in Children)
            {
                if (currentChild == abortForChild)
                {
                    found = true;
                }
                else if (!found)
                {
                    indexForChild++;
                }
                else if (found && currentChild.IsActive)
                {
                    if (immediateRestart)
                    {
                        currentIndex = indexForChild - 1;
                    }
                    else
                    {
                        currentIndex = Children.Length;
                    }
                    currentChild.Stop();
                    break;
                }
            }
        }

        public override string ToString()
        {
            return base.ToString() + "[" + currentIndex + "]";
        }
    }
}