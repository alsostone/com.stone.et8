using ST.GridBuilder;

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
            if (self.GridMapIndicator) self.GridMapIndicator.SetGridMap(self.GridMap);
        }
    }
}