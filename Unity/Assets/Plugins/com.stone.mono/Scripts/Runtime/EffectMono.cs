using UnityEngine;

namespace ST.Mono
{
    public class EffectMono : MonoBehaviour
    {
        [Tooltip("特效时长(秒)")] public float DurationTime;
        [HideInInspector] public ParticleSystem[] ParticleSystems;
        [HideInInspector] public Animator[] Animators;
        [HideInInspector] public Animation[] Animations;
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            ParticleSystems = gameObject.GetComponentsInChildren<ParticleSystem>(true);
            Animators = gameObject.GetComponentsInChildren<Animator>(true);
            Animations = gameObject.GetComponentsInChildren<Animation>(true);
            DurationTime = ParticleSystems.GetDuration();
        }
        private void Reset()
        {
            OnValidate();
        }
#endif
        
        public void Play(float speed = 1, float time = 0)
        {
            for (int i = 0, length = ParticleSystems.Length; i < length; i++) {
                var particle = ParticleSystems[i];
                if (particle == default) { continue; }
                
                var main = particle.main;
                main.simulationSpeed = speed;
                particle.Simulate(time, false, true, false);
                particle.Play(false);
            }
            for (int i = 0, length = Animators.Length; i < length; i++) {
                var animator = Animators[i];
                if (animator == default) { continue; }

                animator.enabled = true;
                animator.speed = speed;
                var info = animator.GetCurrentAnimatorStateInfo(0);
                animator.Play(info.shortNameHash, 0, time / info.length);
            }
            for (int i = 0, length = Animations.Length; i < length; i++) {
                var anim = Animations[i];
                if (anim == default || anim.clip == default) { continue; }

                anim.enabled = true;
                var state = anim[anim.clip.name];
                state.speed = speed;
                state.time = time;
                anim.Play();
            }
        }
        
        public void Stop()
        {
            for (int i = 0, length = ParticleSystems.Length; i < length; i++) {
                var particle = ParticleSystems[i];
                if (particle == default) { continue; }
                
                particle.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            for (int i = 0, length = Animators.Length; i < length; i++) {
                var animator = Animators[i];
                if (animator == default) { continue; }
                
                animator.enabled = false;
                animator.Rebind();
            }
            for (int i = 0, length = Animations.Length; i < length; i++) {
                var anim = Animations[i];
                if (anim == default || anim.clip == default) { continue; }
                
                anim.Stop();
                anim.Rewind();
            }
        }
        
        public void SetSpeed(float speed)
        {
            for (var i = ParticleSystems.Length - 1; i >= 0; i--) {
                var particle = ParticleSystems[i];
                if (particle == default) { continue; }
                
                var main = particle.main;
                main.simulationSpeed = speed;
            }
            for (var i = Animators.Length - 1; i >= 0; i--) {
                var animator = Animators[i];
                if (animator == default) { continue; }
                
                animator.speed = speed;
            }
            for (var i = Animations.Length - 1; i >= 0; i--) {
                var anim = Animations[i];
                if (anim == default || anim.clip == default) { continue; }

                var state = anim[anim.clip.name];
                state.speed = speed;
            }
        }
        
    }
}
