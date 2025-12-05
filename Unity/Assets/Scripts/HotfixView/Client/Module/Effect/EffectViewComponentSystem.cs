using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(ViewEffectComponent))]
    [LSEntitySystemOf(typeof(ViewEffectComponent))]
    [FriendOf(typeof(ViewEffectComponent))]
    public static partial class EffectViewComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ViewEffectComponent self)
        {
            self.idGenerator = 0;
        }

        [EntitySystem]
        private static void Destroy(this ViewEffectComponent self)
        {
            self.SkillEffectViews.Clear();
            foreach (var @ref in self.EffectViews)
            {
                EffectView view = @ref.Value;
                view?.Dispose();
            }
            self.EffectViews.Clear();
        }
        
        private static long GetId(this ViewEffectComponent self)
        {
            return ++self.idGenerator;
        }
        
        public static async ETTask<GameObject> PlayFx(this ViewEffectComponent self, int fxId)
        {
            EffectView view = null;
            if (self.SkillEffectViews.TryGetValue(fxId, out var @ref)) {
                view = @ref;
                if (view != null) {
                    view.Reset();
                    return null;
                }
                self.SkillEffectViews.Remove(fxId);
            }
            
            var fxRow = TbSkillResource.Instance.Get(fxId);
            if (fxRow == null) { return null; }

            LSUnitView lsUnitView = self.LSViewOwner();
            LSViewTransformComponent viewTransformComponent = lsUnitView.GetComponent<LSViewTransformComponent>();
            Transform attachTransform = viewTransformComponent.GetAttachTransform((AttachPoint)fxRow.AttachPoint);

            ResourcesPoolComponent poolComponent = self.Room().GetComponent<ResourcesPoolComponent>();
            GameObject go = await poolComponent.FetchAsync(fxRow.Resource, attachTransform);
            view = self.AddChildWithId<EffectView, GameObject>(self.GetId(), go);
            self.SkillEffectViews.Add(fxId, view);
            return go;
        }
        
        public static async ETTask<GameObject> PlayFx(this ViewEffectComponent self, int fxResource, AttachPoint attachPoint)
        {
            EffectView view = null;
            if (self.EffectViews.TryGetValue(fxResource, out var @ref)) {
                view = @ref;
                if (view != null) {
                    view.Reset();
                    return null;
                }
                self.EffectViews.Remove(fxResource);
            }
            
            LSUnitView lsUnitView = self.LSViewOwner();
            LSViewTransformComponent viewTransformComponent = lsUnitView.GetComponent<LSViewTransformComponent>();
            Transform attachTransform = viewTransformComponent.GetAttachTransform(attachPoint);

            ResourcesPoolComponent poolComponent = self.Room().GetComponent<ResourcesPoolComponent>();
            GameObject go = await poolComponent.FetchAsync(fxResource, attachTransform);
            if (!self.EffectViews.ContainsKey(fxResource))  // 异步加载可能导致开始的TryGetValue未命中，这里再检查一次
            {
                view = self.AddChildWithId<EffectView, GameObject>(self.GetId(), go);
                self.EffectViews.Add(fxResource, view);
            }
            return go;
        }
        
        public static void StopFx(this ViewEffectComponent self, int fxResource)
        {
            if (self.EffectViews.TryGetValue(fxResource, out var @ref)) {
                EffectView view = @ref;
                view?.Dispose();
                self.EffectViews.Remove(fxResource);
            }
        }
    }
}