using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(ViewIndicatorComponent))]
    [LSEntitySystemOf(typeof(ViewIndicatorComponent))]
    [FriendOf(typeof(ViewIndicatorComponent))]
    public static partial class ViewIndicatorComponentSystem
    {
        [EntitySystem]
        private static void Awake(this ViewIndicatorComponent self, int rangeIndicator, float attackRange)
        {
            self.RangeIndicator = rangeIndicator;
            self.AttackRange = attackRange;
        }

        [EntitySystem]
        private static void Destroy(this ViewIndicatorComponent self)
        {
            self.LSViewOwner().GetComponent<ViewEffectComponent>()?.StopFx(self.RangeIndicator);
        }
        
        public static async ETTask ShowRangeIndicator(this ViewIndicatorComponent self)
        {
            ViewEffectComponent viewEffectComponent = self.LSViewOwner().GetComponent<ViewEffectComponent>();
            if (viewEffectComponent == null) return;
            
            GameObject go = await viewEffectComponent.PlayFx(self.RangeIndicator, AttachPoint.None);
            go.transform.localScale = Vector3.one * (self.AttackRange * 2);
        }
        
        public static void HideRangeIndicator(this ViewIndicatorComponent self)
        {
            self.LSViewOwner().GetComponent<ViewEffectComponent>()?.StopFx(self.RangeIndicator);
        }
        
    }
}