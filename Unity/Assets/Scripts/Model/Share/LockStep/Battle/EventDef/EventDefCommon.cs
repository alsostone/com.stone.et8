using TrueSync;

namespace ET
{
    public struct LSUnitCreate
    {
        public LSUnit LSUnit;
    }
    
    public struct LSUnitRemove
    {
        public long Id;
    }

    public struct LSUnitMoving
    {
        public long Id;
        public bool IsMoving;
    }
    
    public struct LSUnitPlaced
    {
        public long Id;
        public int X;
        public int Z;
    }
}

