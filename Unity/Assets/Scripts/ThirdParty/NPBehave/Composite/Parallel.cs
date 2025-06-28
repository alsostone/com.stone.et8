using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

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
        
        [BsonElement][MemoryPackInclude] private Policy failurePolicy;
        [BsonElement][MemoryPackInclude] private Policy successPolicy;
        [BsonElement][MemoryPackInclude] private int childrenCount = 0;
        [BsonElement][MemoryPackInclude] private int runningCount = 0;
        [BsonElement][MemoryPackInclude] private int succeededCount = 0;
        [BsonElement][MemoryPackInclude] private int failedCount = 0;
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        [BsonElement][MemoryPackInclude] private Dictionary<int, bool> childrenResults;
        [BsonElement][MemoryPackInclude] private bool successState;
        [BsonElement][MemoryPackInclude] private bool childrenAborted;

        public Parallel(Policy successPolicy, Policy failurePolicy, params Node[] children) : base("Parallel", children)
        {
            this.successPolicy = successPolicy;
            this.failurePolicy = failurePolicy;
            childrenCount = children.Length;
            childrenResults = new Dictionary<int, bool>();
        }

        protected override void DoStart()
        {
            childrenAborted = false;
            runningCount = 0;
            succeededCount = 0;
            failedCount = 0;
            foreach (Node child in Children)
            {
                runningCount++;
                child.Start();
            }
        }

        protected override void DoStop()
        {
            foreach (Node child in Children)
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
            childrenResults[child.Guid] = result;

            bool allChildrenStarted = runningCount + succeededCount + failedCount == childrenCount;
            if (allChildrenStarted)
            {
                if (runningCount == 0)
                {
                    if (!childrenAborted) // if children got aborted because rule was evaluated previously, we don't want to override the successState 
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
                else if (!childrenAborted)
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
                        foreach (Node currentChild in Children)
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
                if (childrenResults[abortForChild.Guid])
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