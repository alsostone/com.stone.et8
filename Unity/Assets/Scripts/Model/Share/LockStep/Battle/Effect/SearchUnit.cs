using TrueSync;

namespace ET
{
    public class SearchUnit : Object, IPool
    {
        public LSUnit Target;
        public FP Distance;
        
        public bool IsFromPool { get; set; }
    }
}
