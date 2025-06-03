using System.Collections.Generic;
using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class Parallel : Composite
    {
        public enum Policy
        {
            ONE,
            ALL,
        }
        
        [MemoryPackInclude] private Policy failurePolicy;
        [MemoryPackInclude] private Policy successPolicy;
        [MemoryPackInclude] private int childrenCount = 0;
        [MemoryPackInclude] private int runningCount = 0;
        [MemoryPackInclude] private int succeededCount = 0;
        [MemoryPackInclude] private int failedCount = 0;
        [MemoryPackInclude] private Dictionary<Node, bool> childrenResults;
        [MemoryPackInclude] private bool successState;
        [MemoryPackInclude] private bool childrenAborted;

        public Parallel(Policy successPolicy, Policy failurePolicy, params Node[] children) : base("Parallel", children)
        {
            this.successPolicy = successPolicy;
            this.failurePolicy = failurePolicy;
            this.childrenCount = children.Length;
            this.childrenResults = new Dictionary<Node, bool>();
        }

        protected override void DoStart()
        {
            childrenAborted = false;
            runningCount = 0;
            succeededCount = 0;
            failedCount = 0;
            foreach (Node child in this.Children)
            {
                runningCount++;
                child.Start();
            }
        }

        protected override void DoStop()
        {
            foreach (Node child in this.Children)
            {
                if (child.IsActive)
                {
                    child.Stop();
                }
            }
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            runningCount--;
            if (result)
            {
                succeededCount++;
            }
            else
            {
                failedCount++;
            }
            this.childrenResults[child] = result;

            bool allChildrenStarted = runningCount + succeededCount + failedCount == childrenCount;
            if (allChildrenStarted)
            {
                if (runningCount == 0)
                {
                    if (!this.childrenAborted) // if children got aborted because rule was evaluated previously, we don't want to override the successState 
                    {
                        if (failurePolicy == Policy.ONE && failedCount > 0)
                        {
                            successState = false;
                        }
                        else if (successPolicy == Policy.ONE && succeededCount > 0)
                        {
                            successState = true;
                        }
                        else if (successPolicy == Policy.ALL && succeededCount == childrenCount)
                        {
                            successState = true;
                        }
                        else
                        {
                            successState = false;
                        }
                    }
                    Stopped(successState);
                }
                else if (!this.childrenAborted)
                {
                    if (failurePolicy == Policy.ONE && failedCount > 0)
                    {
                        successState = false;
                        childrenAborted = true;
                    }
                    else if (successPolicy == Policy.ONE && succeededCount > 0)
                    {
                        successState = true;
                        childrenAborted = true;
                    }

                    if (childrenAborted)
                    {
                        foreach (Node currentChild in this.Children)
                        {
                            if (currentChild.IsActive)
                            {
                                currentChild.Stop();
                            }
                        }
                    }
                }
            }
        }

        public override void StopLowerPriorityChildrenForChild(Node abortForChild, bool immediateRestart)
        {
            if (immediateRestart)
            {
                if (childrenResults[abortForChild])
                {
                    succeededCount--;
                }
                else
                {
                    failedCount--;
                }
                runningCount++;
                abortForChild.Start();
            }
            else
            {
                throw new Exception("On Parallel Nodes all children have the same priority, thus the method does nothing if you pass false to 'immediateRestart'!");
            }
        }
    }
}