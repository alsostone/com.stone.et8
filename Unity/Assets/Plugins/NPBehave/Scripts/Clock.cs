using System.Collections.Generic;
using MemoryPack;

namespace NPBehave
{
    [MemoryPackable]
    public partial class Timer
    {
        public double scheduledTime = 0f;
        public int repeat = 0;
        public double delay = 0f;
        public float randomVariance = 0.0f;

        public void ScheduleAbsoluteTime(double elapsedTime)
        {
            scheduledTime = elapsedTime + delay - randomVariance * 0.5f + randomVariance * UnityEngine.Random.value;
        }
    }
    
    [MemoryPackable]
    public partial class Clock
    {
        [MemoryPackInclude] private double elapsedTime = 0f;
        [MemoryPackInclude] private bool isInUpdate = false;
        
        [MemoryPackInclude] private Dictionary<int, Timer> timers = new Dictionary<int, Timer>();
        [MemoryPackInclude] private Dictionary<int, Timer> addTimers = new Dictionary<int, Timer>();
        [MemoryPackInclude] private HashSet<int> removeTimers = new HashSet<int>();
        
        [MemoryPackInclude] private HashSet<int> updateObservers = new HashSet<int>();
        [MemoryPackInclude] private HashSet<int> addObservers = new HashSet<int>();
        [MemoryPackInclude] private HashSet<int> removeObservers = new HashSet<int>();
        
        [MemoryPackIgnore] private BehaveWorld behaveWorld;
        [MemoryPackIgnore] private readonly Queue<Timer> timerPool = new Queue<Timer>();

        [MemoryPackConstructor]
        private Clock() { }

        internal Clock(BehaveWorld world)
        {
            behaveWorld = world;
        }
        
        internal void SetWorld(BehaveWorld world)
        {
            behaveWorld = world;
        }
        
        /// <summary>Register a timer function</summary>
        /// <param name="time">time in milliseconds</param>
        /// <param name="repeat">number of times to repeat, set to -1 to repeat until unregistered.</param>
        /// <param name="action">method to invoke</param>
        public void AddTimer(float time, int repeat, int action)
        {
            AddTimer(time, 0f, repeat, action);
        }

        /// <summary>Register a timer function with random variance</summary>
        /// <param name="delay">time in milliseconds</param>
        /// <param name="randomVariance">deviate from time on a random basis</param>
        /// <param name="repeat">number of times to repeat, set to -1 to repeat until unregistered.</param>
        /// <param name="action">method to invoke</param>
        public void AddTimer(float delay, float randomVariance, int repeat, int action)
        {
            Timer timer = null;

            if (!isInUpdate)
            {
                if (!timers.ContainsKey(action))
                {
					timers[action] = GetTimerFromPool();
                }
				timer = timers[action];
            }
            else
            {
                if (!addTimers.ContainsKey(action))
                {
					addTimers[action] = GetTimerFromPool();
                }
				timer = addTimers [action];

                if (removeTimers.Contains(action))
                {
                    removeTimers.Remove(action);
                }
            }

			timer.delay = delay;
			timer.randomVariance = randomVariance;
			timer.repeat = repeat;
			timer.ScheduleAbsoluteTime(elapsedTime);
        }

        public void RemoveTimer(int action)
        {
            if (!isInUpdate)
            {
                if (timers.ContainsKey(action))
                {
                    timerPool.Enqueue(timers[action]);
                    timers.Remove(action);
                }
            }
            else
            {
                if (timers.ContainsKey(action))
                {
                    removeTimers.Add(action);
                }
                if (addTimers.ContainsKey(action))
                {
                    timerPool.Enqueue(addTimers[action]);
                    addTimers.Remove(action);
                }
            }
        }

        public bool HasTimer(int action)
        {
            if (!isInUpdate)
            {
                return timers.ContainsKey(action);
            }
            else
            {
                if (removeTimers.Contains(action))
                {
                    return false;
                }
                else if (addTimers.ContainsKey(action))
                {
                    return true;
                }
                else
                {
                    return timers.ContainsKey(action);
                }
            }
        }

        /// <summary>Register a function that is called every frame</summary>
        /// <param name="action">function to invoke</param>
        public void AddUpdateObserver(int action)
        {
            if (!isInUpdate)
            {
                updateObservers.Add(action);
            }
            else
            {
                if (!updateObservers.Contains(action))
                {
                    addObservers.Add(action);
                }
                if (removeObservers.Contains(action))
                {
                    removeObservers.Remove(action);
                }
            }
        }

        public void RemoveUpdateObserver(int action)
        {
            if (!isInUpdate)
            {
                updateObservers.Remove(action);
            }
            else
            {
                if (updateObservers.Contains(action))
                {
                    removeObservers.Add(action);
                }
                if (addObservers.Contains(action))
                {
                    addObservers.Remove(action);
                }
            }
        }

        public bool HasUpdateObserver(int action)
        {
            if (!isInUpdate)
            {
                return updateObservers.Contains(action);
            }
            else
            {
                if (removeObservers.Contains(action))
                {
                    return false;
                }
                else if (addObservers.Contains(action))
                {
                    return true;
                }
                else
                {
                    return updateObservers.Contains(action);
                }
            }
        }

        internal void Update(float deltaTime)
        {
            elapsedTime += deltaTime;
            isInUpdate = true;

            foreach (var action in updateObservers)
            {
                if (!removeObservers.Contains(action))
                {
                    behaveWorld.GuidReceiverMapping[action].OnTimerReached();
                }
            }

            var keys = timers.Keys;
			foreach (var callback in keys)
            {
                if (removeTimers.Contains(callback))
                {
                    continue;
                }

				Timer timer = timers[callback];
                if (timer.scheduledTime <= elapsedTime)
                {
                    if (timer.repeat == 0)
                    {
                        RemoveTimer(callback);
                    }
                    else if (timer.repeat >= 0)
                    {
                        timer.repeat--;
                    }
                    
                    behaveWorld.GuidReceiverMapping[callback].OnTimerReached();
					timer.ScheduleAbsoluteTime(elapsedTime);
                }
            }

            foreach (var action in addObservers)
            {
                updateObservers.Add(action);
            }
            foreach (var action in removeObservers)
            {
                updateObservers.Remove(action);
            }
            foreach (var pair in addTimers)
            {
                if (timers.TryGetValue(pair.Key, out var timer))
                {
                    timerPool.Enqueue(timer);
                }
                timers[pair.Key] = pair.Value;
            }
            foreach (var action in removeTimers)
            {
                timerPool.Enqueue(timers[action]);
                timers.Remove(action);
            }
            addObservers.Clear();
            removeObservers.Clear();
            addTimers.Clear();
            removeTimers.Clear();

            isInUpdate = false;
        }
        
#if UNITY_EDITOR
        [MemoryPackIgnore] public int NumUpdateObservers => updateObservers.Count;
        [MemoryPackIgnore] public int NumTimers => timers.Count;
        [MemoryPackIgnore] public double ElapsedTime => elapsedTime;
        [MemoryPackIgnore] public int DebugPoolSize => timerPool.Count;
#endif
        
        private Timer GetTimerFromPool()
        {
            if (!timerPool.TryDequeue(out var timer))
            {
                timer = new Timer();
            }
            return timer;
        }

    }
}