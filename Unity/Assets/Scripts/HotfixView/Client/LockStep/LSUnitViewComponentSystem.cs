using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSUnitViewComponent))]
    public static partial class LSUnitViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSUnitViewComponent self)
        {

        }
        
        [EntitySystem]
        private static void Destroy(this LSUnitViewComponent self)
        {

        }

        public static async ETTask InitAsync(this LSUnitViewComponent self)
        {
            Room room = self.Room();
            LSUnitComponent lsUnitComponent = room.LSWorld.GetComponent<LSUnitComponent>();
            foreach (var pair in lsUnitComponent.Children)
            {
                LSUnit lsUnit = pair.Value as LSUnit;
                if (lsUnit == null)
                    continue;
                await LSUnitViewFactory.CreateLSUnitViewAsync(self.Scene(), lsUnit);
            }
        }
    }
}