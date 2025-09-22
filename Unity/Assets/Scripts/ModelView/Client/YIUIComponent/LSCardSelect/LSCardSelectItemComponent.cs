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
    public partial class LSCardSelectItemComponent: Entity
    {
        public LSRandomDropItem ItemData { get; set; }
        public int Index;
    }
}