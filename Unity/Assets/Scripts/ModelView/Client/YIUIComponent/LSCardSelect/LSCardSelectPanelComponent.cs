using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

namespace ET.Client
{
    /// <summary>
    /// Author  alsostone
    /// Date    2025.9.20
    /// Desc
    /// </summary>
    public partial class LSCardSelectPanelComponent: Entity, IYIUIOpenTween, IYIUICloseTween, IYIUIEvent<OnCardSelectClickEvent>, IYIUIEvent<OnCardSelectResetEvent>
    {
        public bool IsClickDone { get; set; } = false;
        public List<LSRandomDropItem> CachedCards;
        public YIUIListView<LSCardSelectItemComponent> CardsView;
    }
    
    public struct OnCardSelectClickEvent
    {
        public int Index;
    }
}