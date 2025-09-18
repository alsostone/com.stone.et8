
namespace ET.Client
{
    /// <summary>
    /// Author  YIUI
    /// Date    2025.9.16
    /// Desc
    /// </summary>
    public partial class PlayCardItemComponent: Entity
    {
        public PlayCardItemData Data { get; set; }
        public int PointerId = -1;
    }
    
    public struct PlayCardItemData
    {
        public EUnitType Type;
        public int TableId;
        public int Index;
    }
}