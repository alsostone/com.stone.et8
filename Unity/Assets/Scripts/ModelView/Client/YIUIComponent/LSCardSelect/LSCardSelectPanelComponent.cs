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
    public partial class LSCardSelectPanelComponent: Entity, IYIUIOpen<List<LSRandomDropItem>, int>
    {
        public int SelectCount;
        public YIUIListView<LSCardSelectItemComponent> CardsView;
    }
    
}