using ST.Mono;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(ViewEffect))]
    [FriendOf(typeof(ViewEffect))]
    public static partial class ViewEffectSystem
    {
        [EntitySystem]
        private static void Awake(this ViewEffect self, GameObject go)
        {
            self.GameObject = go;
            self.IsLoop = false;
            self.Speed = 1;
            self.DurationTime = -1;
            
            self.Mono = go.GetComponent<EffectMono>();
            self.Reset();
        }

        [EntitySystem]
        private static void Update(this ViewEffect self)
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
        private static void Destroy(this ViewEffect self)
        {
            var poolComponent = self.Room().GetComponent<ResourcesPoolComponent>();
            poolComponent.Recycle(self.GameObject);
        }

        public static void Reset(this ViewEffect self)
        {
            if (self.Mono) {
                self.DurationTime = self.Mono.DurationTime;
                self.Mono.Play(self.Speed);
            }
        }
    }
};

