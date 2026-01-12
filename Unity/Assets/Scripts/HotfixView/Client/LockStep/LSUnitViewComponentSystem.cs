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
            Room room = self.Room();
            LSUnitComponent lsUnitComponent = room.LSWorld.GetComponent<LSUnitComponent>();
            foreach (var pair in lsUnitComponent.Children)
            {
                LSUnit lsUnit = pair.Value as LSUnit;
                LSUnitViewFactory.CreateLSUnitView(self, lsUnit);
            }
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
            float speed = room.TimeScale;

            // 1. 销毁表现层没有实体的LSUnitView
            List<LSUnitView> removeViews = ObjectPool.Instance.Fetch<List<LSUnitView>>();
            foreach (var pair in self.Children)
            {
                LSUnitView lsUnitView = pair.Value as LSUnitView;
                var lsUnit = lsUnitComponent.GetChild<LSUnit>(lsUnitView.Id);
                if (lsUnit == null) {
                    removeViews.Add(lsUnitView);
                } else {
                    lsUnitView.GetComponent<LSAnimationComponent>()?.SetSpeed(speed);
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
                    LSUnitViewFactory.CreateLSUnitView(self, pair.Value as LSUnit);
                }
            }
        }

        public static void SetSpeed(this LSUnitViewComponent self, float speed)
        {
            foreach (var pair in self.Children)
            {
                LSUnitView lsUnitView = pair.Value as LSUnitView;
                lsUnitView.GetComponent<LSAnimationComponent>()?.SetSpeed(speed);
            }
        }
    }
}