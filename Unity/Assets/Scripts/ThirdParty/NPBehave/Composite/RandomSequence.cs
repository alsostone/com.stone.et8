using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace NPBehave
{
    [MemoryPackable]
    public partial class RandomSequence : Composite
    {
        [BsonElement][MemoryPackInclude] private int currentIndex = -1;
        [BsonElement][MemoryPackInclude] private int[] randomizedOrder;

        public RandomSequence(params Node[] children) : base("Random Sequence", children)
        {
            randomizedOrder = new int[children.Length];
            for (int i = 0; i < Children.Length; i++)
            {
                randomizedOrder[i] = i;
            }
        }

        protected override void DoStart()
        {
            currentIndex = -1;

            // Shuffling
            int n = randomizedOrder.Length;
            while (n > 1)
            {
                int k = RootNode.RootBehaveWorld.GetRandomNext(n--);
                (randomizedOrder[n], randomizedOrder[k]) = (randomizedOrder[k], randomizedOrder[n]);
            }

            ProcessChildren();
        }

        protected override void DoStop()
        {
            Children[randomizedOrder[currentIndex]].Stop();
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
                    Children[randomizedOrder[currentIndex]].Start();
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