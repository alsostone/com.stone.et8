using TrueSync;

namespace ET
{
    public struct LSEscape
    {
        public long Id;
    }
    
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
    
    public struct LSTouchDragCancel
    {
        public long Id;
    }
    
    public struct LSPlacementDrag
    {
        public long Id;
        public long TargetId;
    }

    public struct LSPlacementNew
    {
        public long Id;
        public long ItemId;
    }
    
    public struct LSPlacementRotate
    {
        public long Id;
        public int Rotation;
    }
    
}

