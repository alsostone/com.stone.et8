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
    public partial class PlayViewComponent: Entity, IUpdate, IYIUIEvent<OnCardDragStartEvent>, IYIUIEvent<OnCardDragEvent>, IYIUIEvent<OnCardDragEndEvent>, IYIUIEvent<OnCardViewResetEvent>, IYIUIEvent<OnCardSelectResetEvent>
    {
        public string SaveName;
        public int PredictFrame = 0;

        public List<List<LSRandomDropItem>> CachedCards;
        public YIUIListView<PlayCardItemComponent> CardsView;
        
        public bool IsDragging = false;
        public Vector3 DragStartPosition;
        public Dictionary<Transform, Image> CachedBodyImages = new Dictionary<Transform, Image>();
    }
    
    public struct OnCardDragStartEvent
    {
        public long PlayerId;
        public long ItemId;
    }
    
    public struct OnCardDragEvent
    {
        public long PlayerId;
        public Vector3 Position;
    }
    
    public struct OnCardDragEndEvent
    {
        public long PlayerId;
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