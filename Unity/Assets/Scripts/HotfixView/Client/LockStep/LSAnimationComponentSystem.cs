using UnityEngine;

namespace ET.Client
{
	[EntitySystemOf(typeof(LSAnimationComponent))]
	[FriendOf(typeof(LSAnimationComponent))]
	public static partial class LSAnimationComponentSystem
	{
        [Invoke(TimerInvokeType.AnimationTimer)]
        [FriendOf(typeof(LSAnimationComponent))]
        public class AnimationTimer : ATimer<LSAnimationComponent>
        {
            protected override void Run(LSAnimationComponent self)
            {
	            string name = self.mAnimationStack.Count > 0 ? self.mAnimationStack[^1] : AnimationNames.Idle;
	            self.PlayAnimtation(name);
            }
        }

        [EntitySystem]
		private static void Destroy(this LSAnimationComponent self)
		{
			self.Root().GetComponent<TimerComponent>().Remove(ref self.AnimationTimer);
		}
		
		[EntitySystem]
		private static void Awake(this LSAnimationComponent self, Animation animation)
		{
			self.Animation = animation;
		}
		
		public static void PlayAnimtation(this LSAnimationComponent self, string name, bool force = false)
		{
			// 利用此方式规避快由不同功能切同一动作带来的抖动问题
			if (force || self.AnimationName != name)
			{
				self.Animation.CrossFade(name, 0.2f);
				self.AnimationName = name;
			}
		}
		
		public static void AddAnimation(this LSAnimationComponent self, string name, bool force = false)
		{
			self.Root().GetComponent<TimerComponent>().Remove(ref self.AnimationTimer);

			// 不强制切换时 判断当前是否正在播该动作
			if (!force) {
				var index = self.mAnimationStack.IndexOf(name);
				if (index != -1 && index == self.mAnimationStack.Count - 1)
					return;
			}
			self.mAnimationStack.Add(name);
            
			self.Animation.CrossFade(name, 0.2f);
			self.AnimationName = name;
		}
        
		// 支持延迟 防止Remove后立即又Add造成动画抖动
		// 例：准备攻击动作->取消准备攻击(待机动作)->技能动作，中间的待机状态从体验上来说多余
		public static void RemoveAnimation(this LSAnimationComponent self, string name, int delay = 0, bool auto = true)
		{
			var index = self.mAnimationStack.IndexOf(name);
			if (index == -1)
				return;
            
			self.mAnimationStack.RemoveAt(index);
			if (index == self.mAnimationStack.Count)
			{
				self.Root().GetComponent<TimerComponent>().Remove(ref self.AnimationTimer);
				if (auto)
				{
					if (delay > 0)
					{
						var tillTime = TimeInfo.Instance.ServerFrameTime() + delay;
						self.AnimationTimer = self.Root().GetComponent<TimerComponent>().NewOnceTimer(tillTime, TimerInvokeType.AnimationTimer, self);
					}
					else
					{
						name = self.mAnimationStack.Count > 0 ? self.mAnimationStack[^1] : AnimationNames.Idle;
						self.PlayAnimtation(name);
					}
				}
			}
		}
		
	}
}