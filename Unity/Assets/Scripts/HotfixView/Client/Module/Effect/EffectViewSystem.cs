using ST.Mono;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(EffectView))]
    [FriendOf(typeof(EffectView))]
    public static partial class EffectViewSystem
    {
        [EntitySystem]
        private static void Awake(this EffectView self, GameObject go)
        {
            self.GameObject = go;
            self.IsLoop = false;
            self.Speed = 1;
            self.DurationTime = -1;
            
            self.Mono = go.GetComponent<EffectMono>();
            self.Reset();
        }

        [EntitySystem]
        private static void Update(this EffectView self)
        {
            if (self.DurationTime <= 0) {
                return;
            }
            
            self.DurationTime -= Time.deltaTime;
            if (self.DurationTime <= 0) {
                if (self.IsLoop) {
                    self.Reset();
                } else {
                    self.Dispose();
                }
            }
        }
        
        [EntitySystem]
        private static void Destroy(this EffectView self)
        {
            var poolComponent = self.Room().GetComponent<ResourcesPoolComponent>();
            poolComponent.Recycle(self.GameObject);
        }

        public static void Reset(this EffectView self)
        {
            if (self.Mono) {
                self.DurationTime = self.Mono.DurationTime;
                self.Mono.Play(self.Speed);
            }
        }
    }
};

