using MemoryPack;
using TrueSync;

namespace NPBehave
{
    [MemoryPackable]
    public partial class Cooldown : Decorator
    {
        [MemoryPackInclude] private bool startAfterDecoratee = false;
        [MemoryPackInclude] private bool resetOnFailiure = false;
	    [MemoryPackInclude] private bool failOnCooldown = false;
        [MemoryPackInclude] private readonly FP cooldownTime;
        [MemoryPackInclude] private readonly FP randomVariation;
        [MemoryPackInclude] private bool isReady = true;
        
        /// <summary>
        /// The Cooldown decorator ensures that the branch can not be started twice within the given cooldown time.
        /// 
        /// The decorator can start the cooldown timer right away or wait until the child stopps, you can control this behavior with the
        /// `startAfterDecoratee` parameter.
        /// 
        /// The default behavior in case the cooldown timer is active and this node is started again is, that the decorator waits until
        /// the cooldown is reached and then executes the underlying node.
        /// You can change this behavior with the `failOnCooldown` parameter to make the decorator immediately fail instead.
        /// 
        /// </summary>
        /// <param name="cooldownTime">time until next execution</param>
        /// <param name="randomVariation">random variation added to the cooldown time</param>
        /// <param name="startAfterDecoratee">If set to <c>true</c> the cooldown timer is started from the point after the decoratee has been started, else it will be started right away.</param>
        /// <param name="resetOnFailiure">If set to <c>true</c> the timer will be reset in case the underlying node fails.</param>
        /// <param name="failOnCooldown">If currently on cooldown and this parameter is set to <c>true</c>, the decorator will immmediately fail instead of waiting for the cooldown.</param>
        /// <param name="decoratee">Decoratee node.</param>
        [MemoryPackConstructor]
        public Cooldown(FP cooldownTime, FP randomVariation, bool startAfterDecoratee, bool resetOnFailiure, bool failOnCooldown, Node decoratee) : base("TimeCooldown", decoratee)
        {
        	this.startAfterDecoratee = startAfterDecoratee;
        	this.cooldownTime = cooldownTime;
        	this.resetOnFailiure = resetOnFailiure;
        	this.randomVariation = randomVariation;
        	this.failOnCooldown = failOnCooldown;
        }

        public Cooldown(FP cooldownTime, bool startAfterDecoratee, bool resetOnFailiure, bool failOnCooldown, Node decoratee) : base("TimeCooldown", decoratee)
        {
        	this.startAfterDecoratee = startAfterDecoratee;
        	this.cooldownTime = cooldownTime;
        	randomVariation = cooldownTime * FP.EN1;
        	this.resetOnFailiure = resetOnFailiure;
        	this.failOnCooldown = failOnCooldown;
        }

        public Cooldown(FP cooldownTime, FP randomVariation, bool startAfterDecoratee, bool resetOnFailiure, Node decoratee) : base("TimeCooldown", decoratee)
        {
        	this.startAfterDecoratee = startAfterDecoratee;
        	this.cooldownTime = cooldownTime;
        	this.resetOnFailiure = resetOnFailiure;
        	this.randomVariation = randomVariation;
        }

        public Cooldown(FP cooldownTime, bool startAfterDecoratee, bool resetOnFailiure, Node decoratee) : base("TimeCooldown", decoratee)
        {
            this.startAfterDecoratee = startAfterDecoratee;
            this.cooldownTime = cooldownTime;
            randomVariation = cooldownTime * FP.EN1;
            this.resetOnFailiure = resetOnFailiure;
        }

        public Cooldown(FP cooldownTime, FP randomVariation, Node decoratee) : base("TimeCooldown", decoratee)
        {
            startAfterDecoratee = false;
            this.cooldownTime = cooldownTime;
            resetOnFailiure = false;
            this.randomVariation = randomVariation;
        }

        public Cooldown(FP cooldownTime, Node decoratee) : base("TimeCooldown", decoratee)
        {
            startAfterDecoratee = false;
            this.cooldownTime = cooldownTime;
            resetOnFailiure = false;
            randomVariation = cooldownTime * FP.EN1;
        }

        protected override void DoStart()
        {
            if (isReady)
            {
                isReady = false;
                if (!startAfterDecoratee)
                {
                    Clock.AddTimer(cooldownTime, randomVariation, 0, Guid);
                }
                Decoratee.Start();
            }
            else
            {
                if (failOnCooldown)
                {
                    Stopped(false);
                }
            }
        }

        protected override void DoStop()
        {
            if (Decoratee.IsActive)
            {
                isReady = true;
                Clock.RemoveTimer(Guid);
                Decoratee.Stop();
            }
            else
            {
                isReady = true;
                Clock.RemoveTimer(Guid);
                Stopped(false);
            }
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            if (resetOnFailiure && !result)
            {
                isReady = true;
                Clock.RemoveTimer(Guid);
            }
            else if (startAfterDecoratee)
            {
                Clock.AddTimer(cooldownTime, randomVariation, 0, Guid);
            }
            Stopped(result);
        }

        public override void OnTimerReached()
        {
            if (IsActive && !Decoratee.IsActive)
            {
                Clock.AddTimer(cooldownTime, randomVariation, 0, Guid);
                Decoratee.Start();
            }
            else
            {
                isReady = true;
            }
        }
    }
}