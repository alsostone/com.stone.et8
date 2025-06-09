
using System.Collections.Generic;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSUnitViewComponent))]
    [LSEntitySystemOf(typeof(LSUnitViewComponent))]
    [FriendOf(typeof(LSUnitViewComponent))]
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

        [LSEntitySystem]
        private static void LSRollback(this LSUnitViewComponent self)
        {
            Room room = self.Room();
            LSUnitComponent lsUnitComponent = room.LSWorld.GetComponent<LSUnitComponent>();

            // 1. 销毁表现层没有实体的LSUnitView
            List<LSUnitView> removeViews = ObjectPool.Instance.Fetch<List<LSUnitView>>();
            foreach (var pair in self.Children)
            {
                var lsUnit = lsUnitComponent.GetChild<LSUnit>(pair.Value.Id);
                if (lsUnit == null)
                {
                    removeViews.Add(pair.Value as LSUnitView);
                }
            }
            foreach (LSUnitView removeView in removeViews)
            {
                removeView.Dispose();
            }
            removeViews.Clear();
            ObjectPool.Instance.Recycle(removeViews);
            
            // 2. 创建逻辑层有实体的LSUnitView
            foreach (var pair in lsUnitComponent.Children)
            {
                var lsUnitView = self.GetChild<LSUnitView>(pair.Value.Id);
                if (lsUnitView == null)
                {
                    LSUnitViewFactory.CreateLSUnitViewAsync(self, pair.Value as LSUnit).Coroutine();
                }
            }
        }

        public static async ETTask InitAsync(this LSUnitViewComponent self)
        {
            Room room = self.Room();
            LSUnitComponent lsUnitComponent = room.LSWorld.GetComponent<LSUnitComponent>();
            foreach (var pair in lsUnitComponent.Children)
            {
                LSUnit lsUnit = pair.Value as LSUnit;
                await LSUnitViewFactory.CreateLSUnitViewAsync(self, lsUnit);
            }
        }
    }
}