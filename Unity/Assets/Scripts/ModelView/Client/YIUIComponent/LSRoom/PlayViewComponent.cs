using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  YIUI
    /// Date    2025.3.6
    /// Desc
    /// </summary>
    public partial class PlayViewComponent: Entity, IUpdate, IYIUIEvent<OnCardDragStartEvent>, IYIUIEvent<OnCardDragEndEvent>, IYIUIEvent<OnCardViewResetEvent>
    {
        public string SaveName;
        public int PredictFrame = 0;

        public int SelectCardCount;
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
}