using System;
using ST.GridBuilder;
using TrueSync;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSViewGridMapComponent))]
    [FriendOf(typeof(LSViewGridMapComponent))]
    public static partial class LSViewGridMapComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSViewGridMapComponent self)
        {
            self.GridMap = UnityEngine.Object.FindObjectOfType<GridMap>();
            self.GridMapIndicator = UnityEngine.Object.FindObjectOfType<GridMapIndicator>();
        }
    }
}