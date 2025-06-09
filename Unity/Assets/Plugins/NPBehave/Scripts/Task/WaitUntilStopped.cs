using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace NPBehave
{
    [MemoryPackable]
    public partial class WaitUntilStopped : Task
    {
        [BsonElement][MemoryPackInclude] private bool successWhenStopped;
        
        public WaitUntilStopped(bool successWhenStopped = false) : base("WaitUntilStopped")
        {
            this.successWhenStopped = successWhenStopped;
        }

        protected override void DoStop()
        {
            Stopped(successWhenStopped);
        }
    }
}