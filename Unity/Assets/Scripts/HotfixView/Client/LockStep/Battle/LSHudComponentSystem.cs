using System;
using UnityEngine;
using ST.HUD;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSHudComponent))]
    [FriendOf(typeof(LSHudComponent))]
    public static partial class LSHudComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSHudComponent self, Vector3 offset, Transform follow, float hp, float hpMax)
        {
            self.Offset = offset;
            self.FollowTransform = follow;
            self.Hp = hp;
            self.MaxHp = hpMax;
            self.HudInstance = HudRenderer.Instance.AddInstance(self.FollowTransform.position + offset, hp / hpMax);
        }

        [EntitySystem]
        private static void Update(this LSHudComponent self)
        {
            HudRenderer.Instance.SetInstancePosition(self.HudInstance, self.FollowTransform.position + self.Offset);
        }
        
        [EntitySystem]
        private static void Destroy(this LSHudComponent self)
        {
            HudRenderer.Instance.RemoveInstance(self.HudInstance);
        }

        public static void SetHp(this LSHudComponent self, float hp)
        {
            self.Hp = hp;
            HudRenderer.Instance.SetInstanceProgress(self.HudInstance, hp / self.MaxHp);
        }

        public static void SetMaxHp(this LSHudComponent self, float maxHp)
        {
            self.MaxHp = maxHp;
            HudRenderer.Instance.SetInstanceProgress(self.HudInstance, self.Hp / maxHp);
        }
    }
}