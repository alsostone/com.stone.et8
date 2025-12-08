using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// Author  YIUI
    /// Date    2025.9.16
    /// Desc
    /// </summary>
    public partial class PlayCardItemComponent: Entity, IYIUIEvent<UICardDragEndEvent>, IYIUIEvent<OnCardItemHighlightEvent>
    {
        public CardBagItem ItemData { get; set; }
        public Vector3 Position { get; set; }
        public int PointerId = -1;
        
        public bool IsCliickEnter = false;
        public bool IsClickPress = false;
        public bool IsHighlight = false;
        public int SiblingIndex = -1;
    }
    
    public struct OnCardItemHighlightEvent
    {
        public long ItemId;
    }
}