using TrueSync;

namespace ET
{
    public struct LSPlacementDragStart
    {
        public long Id;
        public long TargetId;
    }
    
    public struct LSPlacementDrag
    {
        public long Id;
        public TSVector2 Position;
    }
    
    public struct LSPlacementDragEnd
    {
        public long Id;
        public TSVector2 Position;
    }
    
    public struct LSPlacementStart
    {
        public long Id;
        public EUnitType Type;
        public int TableId;
    }
    
    public struct LSPlacementRotate
    {
        public long Id;
        public int Rotation;
    }
    
    public struct LSPlacementCancel
    {
        public long Id;
    }
}

