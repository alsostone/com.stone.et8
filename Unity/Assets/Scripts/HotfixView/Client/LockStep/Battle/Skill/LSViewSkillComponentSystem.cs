using System;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSViewSkillComponent))]
    [LSEntitySystemOf(typeof(LSViewSkillComponent))]
    [FriendOf(typeof(LSViewSkillComponent))]
    public static partial class LSViewSkillComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSViewSkillComponent self)
        {

        }

        [LSEntitySystem]
        private static void LSRollback(this LSViewSkillComponent self)
        {
        }
        
        [EntitySystem]
        private static void Destroy(this LSViewSkillComponent self)
        {
            LSViewTimerComponent timerComponent = self.Room().GetComponent<LSViewTimerComponent>();
            timerComponent.RemoveTimer(ref self.AniPreTimer);
            timerComponent.RemoveTimer(ref self.AniTimer);
            timerComponent.RemoveTimer(ref self.AniAfterTimer);
        }

        public static void OnCastSkill(this LSViewSkillComponent self, int skillId)
        {
            if (self.TbSkillRow != null)
                return;
            
            self.TbSkillRow = TbSkill.Instance.Get(skillId);
            self.PlayAnimationPre();
        }
        
        private static void PlayAnimationPre(this LSViewSkillComponent self)
        {
            if (self.TbSkillRow.DurationPre > 0)
            {
                self.IsSkillRunning = true;
                var animationComponent = self.LSViewOwner().GetComponent<LSAnimationComponent>();
                animationComponent?.AddAnimation(self.TbSkillRow.AniNamePre, true);
                LSViewTimerComponent timerComponent = self.Room().GetComponent<LSViewTimerComponent>();
                self.AniPreTimer = timerComponent.AddTimer(self.TbSkillRow.DurationPre - 200, TimerInvokeType.SkillAniPreTimer, self);
            }
            else
            {
                self.PlayAnimation();
            }
        }
        
        public static void PlayAnimation(this LSViewSkillComponent self)
        {
            if (self.TbSkillRow.AniName.Length > 0)
            {
                self.IsSkillRunning = true;
                var animationComponent = self.LSViewOwner().GetComponent<LSAnimationComponent>();
                animationComponent?.AddAnimation(self.TbSkillRow.AniName, true);
                LSViewTimerComponent timerComponent = self.Room().GetComponent<LSViewTimerComponent>();
                self.AniTimer = timerComponent.AddTimer(self.TbSkillRow.Duration - 200, TimerInvokeType.SkillAniTimer, self);
            }
            else
            {
                self.PlayAnimationAfter();
            }
        }
        
        public static void PlayAnimationAfter(this LSViewSkillComponent self)
        {
            if (self.TbSkillRow.AniNameAfter.Length > 0)
            {
                self.IsSkillRunning = true;
                var animationComponent = self.LSViewOwner().GetComponent<LSAnimationComponent>();
                animationComponent?.AddAnimation(self.TbSkillRow.AniNameAfter, true);
                LSViewTimerComponent timerComponent = self.Room().GetComponent<LSViewTimerComponent>();
                self.AniAfterTimer = timerComponent.AddTimer(self.TbSkillRow.DurationAfter - 200, TimerInvokeType.SkillAniAfterTimer, self);
            }
            else
            {
                self.IsSkillRunning = false;
                self.TbSkillRow = null;
            }
        }

    }
    
    [Invoke(TimerInvokeType.SkillAniPreTimer)]
    [FriendOf(typeof(LSViewSkillComponent))]
    public class SkillAniPreTimer : ATimer<LSViewSkillComponent>
    {
        protected override void Run(LSViewSkillComponent self)
        {
            var animationComponent = self.LSViewOwner().GetComponent<LSAnimationComponent>();
            animationComponent?.RemoveAnimation(self.TbSkillRow.AniNamePre, 1);
            self.PlayAnimation();
        }
    }
    
    [Invoke(TimerInvokeType.SkillAniTimer)]
    [FriendOf(typeof(LSViewSkillComponent))]
    public class SkillAniTimer : ATimer<LSViewSkillComponent>
    {
        protected override void Run(LSViewSkillComponent self)
        {
            var animationComponent = self.LSViewOwner().GetComponent<LSAnimationComponent>();
            animationComponent?.RemoveAnimation(self.TbSkillRow.AniName, 1);
            self.PlayAnimationAfter();
        }
    }
    
    [Invoke(TimerInvokeType.SkillAniAfterTimer)]
    [FriendOf(typeof(LSViewSkillComponent))]
    public class SkillAniAfterTimer : ATimer<LSViewSkillComponent>
    {
        protected override void Run(LSViewSkillComponent self)
        {
            var animationComponent = self.LSViewOwner().GetComponent<LSAnimationComponent>();
            animationComponent?.RemoveAnimation(self.TbSkillRow.AniNameAfter, 1);
            self.TbSkillRow = null;
        }
    }

}