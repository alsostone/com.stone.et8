﻿using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace NPBehave
{
    [MemoryPackable]
    public partial class Sequence : Composite
    {
        [BsonElement][MemoryPackInclude] private int currentIndex = -1;

        public Sequence(params Node[] children) : base("Sequence", children)
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
                ProcessChildren();
            }
            else
            {
                Stopped(false);
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
                Stopped(true);
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