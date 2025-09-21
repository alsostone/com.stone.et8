using System.Collections.Generic;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// Author  YIUI
    /// Date    2025.3.6
    /// Desc
    /// </summary>
    public partial class PlayViewComponent: Entity, IUpdate, IYIUIEvent<OnCardDragStartEvent>, IYIUIEvent<OnCardDragEndEvent>, IYIUIEvent<OnCardViewResetEvent>, IYIUIEvent<OnCardSelectResetEvent>
    {
        public string SaveName;
        public int PredictFrame = 0;

        public List<List<LSRandomDropItem>> CachedCards;
        public YIUIListView<PlayCardItemComponent> CardsView;
    }
    
    public struct OnCardDragStartEvent
    {
        public long PlayerId;
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