using System;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSViewSkillComponent))]
    [LSEntitySystemOf(typeof(LSViewSkillComponent))]
    [FriendOf(typeof(LSViewSkillComponent))]
    public static partial class LSViewSkillComponentSystem
    {
        [Invoke(TimerInvokeType.SkillAniPreTimer)]
        [FriendOf(typeof(LSViewSkillComponent))]
        public class SkillAniPreTimer : ATimer<LSViewSkillComponent>
        {
            protected override void Run(LSViewSkillComponent self)
            {
                self.LSViewOwner().GetComponent<LSAnimationComponent>().RemoveAnimation(self.TbSkillRow.AniNamePre, 1);
                self.PlayAnimation();
            }
        }
        [Invoke(TimerInvokeType.SkillAniTimer)]
        [FriendOf(typeof(LSViewSkillComponent))]
        public class SkillAniTimer : ATimer<LSViewSkillComponent>
        {
            protected override void Run(LSViewSkillComponent self)
            {
                self.LSViewOwner().GetComponent<LSAnimationComponent>().RemoveAnimation(self.TbSkillRow.AniName, 1);
                self.PlayAnimationAfter();
            }
        }
        [Invoke(TimerInvokeType.SkillAniAfterTimer)]
        [FriendOf(typeof(LSViewSkillComponent))]
        public class SkillAniAfterTimer : ATimer<LSViewSkillComponent>
        {
            protected override void Run(LSViewSkillComponent self)
            {
                self.LSViewOwner().GetComponent<LSAnimationComponent>().RemoveAnimation(self.TbSkillRow.AniNameAfter, 1);
                self.TbSkillRow = null;
            }
        }
        
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
            var timerComponent = self.Root().GetComponent<TimerComponent>();
            timerComponent.Remove(ref self.AniPreTimer);
            timerComponent.Remove(ref self.AniTimer);
            timerComponent.Remove(ref self.AniAfterTimer);
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
                self.LSViewOwner().GetComponent<LSAnimationComponent>().AddAnimation(self.TbSkillRow.AniNamePre, true);
                var tillTime = TimeInfo.Instance.ServerFrameTime() + self.TbSkillRow.DurationPre;
                self.AniPreTimer = self.Root().GetComponent<TimerComponent>().NewOnceTimer(tillTime, TimerInvokeType.SkillAniPreTimer, self);
            }
            else
            {
                self.PlayAnimation();
            }
        }
        
        private static void PlayAnimation(this LSViewSkillComponent self)
        {
            if (self.TbSkillRow.AniName.Length > 0)
            {
                self.IsSkillRunning = true;
                self.LSViewOwner().GetComponent<LSAnimationComponent>().AddAnimation(self.TbSkillRow.AniName, true);
                var tillTime = TimeInfo.Instance.ServerFrameTime() + self.TbSkillRow.Duration;
                self.AniTimer = self.Root().GetComponent<TimerComponent>().NewOnceTimer(tillTime, TimerInvokeType.SkillAniTimer, self);
            }
            else
            {
                self.PlayAnimationAfter();
            }
        }
        
        private static void PlayAnimationAfter(this LSViewSkillComponent self)
        {
            if (self.TbSkillRow.AniNameAfter.Length > 0)
            {
                self.IsSkillRunning = true;
                self.LSViewOwner().GetComponent<LSAnimationComponent>().AddAnimation(self.TbSkillRow.AniNameAfter, true);
                var tillTime = TimeInfo.Instance.ServerFrameTime() + self.TbSkillRow.DurationAfter;
                self.AniAfterTimer = self.Root().GetComponent<TimerComponent>().NewOnceTimer(tillTime, TimerInvokeType.SkillAniAfterTimer, self);
            }
            else
            {
                self.IsSkillRunning = false;
                self.TbSkillRow = null;
            }
        }

    }
}