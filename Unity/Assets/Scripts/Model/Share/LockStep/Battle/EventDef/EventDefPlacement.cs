using TrueSync;

namespace ET
{
    public struct LSPlacementDragStart
    {
        public TeamType TeamPlacer;
        public long TargetId;
    }
    
    public struct LSPlacementDrag
    {
        public TeamType TeamPlacer;
        public TSVector2 Position;
    }
    
    public struct LSPlacementDragEnd
    {
        public TeamType TeamPlacer;
        public TSVector2 Position;
        public int Index;
    }
    
    public struct LSPlacementStart
    {
        public TeamType TeamPlacer;
        public EUnitType Type;
        public int TableId;
        public int Index;
    }
    
    public struct LSPlacementRotate
    {
        public TeamType TeamPlacer;
        public int Rotation;
    }
    
    public struct LSPlacementCancel
    {
        public TeamType TeamPlacer;
        public int Index;
    }
}

