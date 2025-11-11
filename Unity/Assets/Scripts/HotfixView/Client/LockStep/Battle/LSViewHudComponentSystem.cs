using System;
using UnityEngine;
using ST.HUD;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSViewHudComponent))]
    [FriendOf(typeof(LSViewHudComponent))]
    public static partial class LSViewHudComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSViewHudComponent self, Vector3 offset, Transform follow, float hp, float hpMax)
        {
            self.Offset = offset;
            self.FollowTransform = follow;
            self.Hp = hp;
            self.MaxHp = hpMax;
            Color color = self.Room().GetComponent<LSSettingsComponent>().GetHudColor(self.LSViewOwner());
            self.HudInstance = HudRenderer.Instance.AddInstance(self.FollowTransform.position + offset, color, hp / hpMax);
        }

        [EntitySystem]
        private static void Update(this LSViewHudComponent self)
        {
            HudRenderer.Instance.SetInstancePosition(self.HudInstance, self.FollowTransform.position + self.Offset);
        }
        
        [EntitySystem]
        private static void Destroy(this LSViewHudComponent self)
        {
            HudRenderer.Instance.RemoveInstance(self.HudInstance);
        }

        public static void SetHp(this LSViewHudComponent self, float hp)
        {
            self.Hp = hp;
            HudRenderer.Instance.SetInstanceProgress(self.HudInstance, hp / self.MaxHp);
        }

        public static void SetMaxHp(this LSViewHudComponent self, float maxHp)
        {
            self.MaxHp = maxHp;
            HudRenderer.Instance.SetInstanceProgress(self.HudInstance, self.Hp / maxHp);
        }
    }
}