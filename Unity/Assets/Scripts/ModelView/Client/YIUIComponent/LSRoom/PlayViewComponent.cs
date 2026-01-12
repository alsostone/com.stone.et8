using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// Author  YIUI
    /// Date    2025.3.6
    /// Desc
    /// </summary>
    public partial class PlayViewComponent: Entity, IUpdate, IYIUIEvent<OnCardViewResetEvent>, IYIUIEvent<OnCardSelectResetEvent>, 
            IYIUIEvent<UICardDragStartEvent>, IYIUIEvent<UICardDragEndEvent>,
            IYIUIEvent<UIArrowDragStartEvent>, IYIUIEvent<UIArrowDragEvent>, IYIUIEvent<UIArrowDragEndEvent>,
            IYIUIEvent<UISelectDragStartEvent>, IYIUIEvent<UISelectDragEvent>, IYIUIEvent<UISelectDragEndEvent>
    {
        public List<List<LSRandomDropItem>> CachedCards;
        public YIUIListView<PlayCardItemComponent> CardsView;
        
        public bool IsArrowDragging = false;
        public Vector3 ArrowDragStartPosition;
        public Dictionary<Transform, Image> CachedBodyImages = new Dictionary<Transform, Image>();
        
        public Vector3 SelectDragStartPosition;
    }
    
    public struct UICardDragStartEvent
    {
        public long PlayerId;
    }
    
    public struct UICardDragEndEvent
    {
        public long PlayerId;
    }
    
    public struct UIArrowDragStartEvent
    {
        public long PlayerId;
        public long ItemId;
    }
    
    public struct UIArrowDragEvent
    {
        public long PlayerId;
        public Vector3 Position;
    }
    
    public struct UIArrowDragEndEvent
    {
        public long PlayerId;
    }

    public struct UISelectDragStartEvent
    {
        public long PlayerId;
        public Vector3 Position;
    }
    
    public struct UISelectDragEvent
    {
        public long PlayerId;
        public Vector3 Position;
    }
    
    public struct UISelectDragEndEvent
    {
        public long PlayerId;
        public Vector3 Position;
    }

    public struct OnCardViewResetEvent
    {
        public long PlayerId;
    }
    
    public struct OnCardSelectResetEvent
    {
        public long PlayerId;
    }
}