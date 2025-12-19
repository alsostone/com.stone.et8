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
    
    public struct LSUnitPosition
    {
        public long Id;
        public TSVector Position;
        public bool Immediate;
    }
    
    public struct LSUnitRotation
    {
        public long Id;
        public TSQuaternion Rotation;
        public bool Immediate;
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

