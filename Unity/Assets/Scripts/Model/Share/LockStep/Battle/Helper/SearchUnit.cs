namespace ET
{
    public class SearchUnit : Object, IPool
    {
        public LSUnit Target;
        public long Distance;
        
        public bool IsFromPool { get; set; }
    }
}
