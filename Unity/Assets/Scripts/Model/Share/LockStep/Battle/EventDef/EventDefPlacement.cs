using TrueSync;

namespace ET
{
    public struct LSTouchDragStart
    {
        public long Id;
        public TSVector2 Position;
    }
    
    public struct LSTouchDrag
    {
        public long Id;
        public TSVector2 Position;
    }
    
    public struct LSTouchDragEnd
    {
        public long Id;
    }
    
    public struct LSPlacementDragStart
    {
        public long Id;
        public long TargetId;
    }

    public struct LSPlacementStart
    {
        public long Id;
        public long ItemId;
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

