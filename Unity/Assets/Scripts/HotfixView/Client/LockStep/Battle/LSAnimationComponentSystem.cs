using System;
using UnityEngine;

namespace ET.Client
{
	[EntitySystemOf(typeof(LSAnimationComponent))]
	[FriendOf(typeof(LSAnimationComponent))]
	public static partial class LSAnimationComponentSystem
	{
		[EntitySystem]
		private static void Awake(this LSAnimationComponent self, Animation animation, float speed)
		{
			self.Animation = animation;
			self.MontionSpeed = speed;
			self.PlayAnimtation(AnimationNames.Idle);
		}
		
		[EntitySystem]
		private static void Destroy(this LSAnimationComponent self)
		{
			self.Room().GetComponent<LSViewTimerComponent>().RemoveTimer(ref self.AnimationTimer);
		}

		public static void PlayAnimtation(this LSAnimationComponent self, string name, bool force = false)
		{
			// 利用此方式规避快由不同功能切同一动作带来的抖动问题
			if (force || self.AnimationName != name)
			{
				self.Animation[name].speed = self.MontionSpeed;
				self.Animation.CrossFade(name, 0.2f);
				self.AnimationName = name;
			}
		}
		
		public static void AddAnimation(this LSAnimationComponent self, string name, bool force = false)
		{
			self.Room().GetComponent<LSViewTimerComponent>().RemoveTimer(ref self.AnimationTimer);

			// 不强制切换时 判断当前是否正在播该动作
			if (!force) {
				var index = self.mAnimationStack.IndexOf(name);
				if (index != -1 && index == self.mAnimationStack.Count - 1)
					return;
			}
			self.mAnimationStack.Add(name);
            
			self.Animation[name].speed = self.MontionSpeed;
			self.Animation.CrossFade(name, 0.2f);
			self.AnimationName = name;
		}
        
		// 支持延迟 防止Remove后立即又Add造成动画抖动
		// 例：准备攻击动作->取消准备攻击(待机动作)->技能动作，中间的待机状态从体验上来说多余
		public static void RemoveAnimation(this LSAnimationComponent self, string name, int delay = 0)
		{
			var index = self.mAnimationStack.IndexOf(name);
			if (index == -1)
				return;
            
			self.mAnimationStack.RemoveAt(index);
			if (index == self.mAnimationStack.Count)
			{
				LSViewTimerComponent timerComponent = self.Room().GetComponent<LSViewTimerComponent>();
				timerComponent.RemoveTimer(ref self.AnimationTimer);
				if (delay > 0) {
					self.AnimationTimer = timerComponent.AddTimer(delay, TimerInvokeType.AnimationTimer, self);
				} else {
					self.PlayAnimtation(self.GetLastAnimation());
				}
			}
		}
		
		public static string GetLastAnimation(this LSAnimationComponent self)
		{
			if (self.mAnimationStack.Count > 0)
			{
				return self.mAnimationStack[^1];
			}
			return AnimationNames.Idle;
		}
		
		public static void SetSpeed(this LSAnimationComponent self, float speed)
		{
			if (Math.Abs(self.MontionSpeed - speed) > float.Epsilon) {
				self.MontionSpeed = speed;
				self.Animation[self.AnimationName].speed = speed;
			}
		}
		
	}
	
	[Invoke(TimerInvokeType.AnimationTimer)]
	[FriendOf(typeof(LSAnimationComponent))]
	public class AnimationTimer : ATimer<LSAnimationComponent>
	{
		protected override void Run(LSAnimationComponent self)
		{
			self.PlayAnimtation(self.GetLastAnimation());
		}
	}
}