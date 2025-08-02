using System;
using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using TrueSync;

namespace NPBehave
{
    [MemoryPackable]
    public partial class Timer
    {
        [BsonElement("ST")][BsonIgnoreIfDefault] public FP scheduledTime;
        [BsonElement("R")][BsonIgnoreIfDefault] public int repeat;
        [BsonElement("D")][BsonIgnoreIfDefault] public FP delay;
        [BsonElement("RV")][BsonIgnoreIfDefault] public FP randomVariance;

        public void ScheduleAbsoluteTime(BehaveWorld world, FP elapsedTime)
        {
            scheduledTime = elapsedTime + delay - randomVariance * FP.Half + randomVariance * world.GetRandomNext(10000) / 10000;
        }
    }
    
    [MemoryPackable]
    public partial class Clock : IDisposable
    {
        [BsonElement("ES")][BsonIgnoreIfDefault][MemoryPackInclude] private FP elapsedTime = FP.Zero;
        [BsonElement("IS")][BsonIgnoreIfDefault][MemoryPackInclude] private bool isInUpdate = false;
        
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        [BsonElement("TM")][MemoryPackInclude] private SortedDictionary<long, Timer> sortedTimers = new SortedDictionary<long, Timer>();
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        [BsonElement("AM")][MemoryPackInclude] private Dictionary<long, Timer> addTimers = new Dictionary<long, Timer>();
        [BsonElement("RM")][MemoryPackInclude] private HashSet<long> removeTimers = new HashSet<long>();
        
        [BsonElement("OB")][MemoryPackInclude] private HashSet<int> updateObservers = new HashSet<int>();
        [BsonElement("AB")][MemoryPackInclude] private HashSet<int> addObservers = new HashSet<int>();
        [BsonElement("RB")][MemoryPackInclude] private HashSet<int> removeObservers = new HashSet<int>();
        
        [BsonIgnore][MemoryPackIgnore] private BehaveWorld behaveWorld;
        [BsonIgnore][MemoryPackIgnore] private Queue<Timer> timerPool = new Queue<Timer>();

        [MemoryPackConstructor]
        private Clock() { }

        internal Clock(BehaveWorld world)
        {
            behaveWorld = world;
        }
        
        public void Dispose()
        {
            this.sortedTimers.Clear();
            addTimers.Clear();
            timerPool.Clear();
        }
        
        internal void SetWorld(BehaveWorld world)
        {
            behaveWorld = world;
        }
        
        /// <summary>Register a timer function</summary>
        /// <param name="time">time in milliseconds</param>
        /// <param name="repeat">number of times to repeat, set to -1 to repeat until unregistered.</param>
        /// <param name="action">method to invoke</param>
        public void AddTimer(FP time, int repeat, int action)
        {
            AddTimer(time, FP.Zero, repeat, action);
        }

        /// <summary>Register a timer function with random variance</summary>
        /// <param name="delay">time in milliseconds</param>
        /// <param name="randomVariance">deviate from time on a random basis</param>
        /// <param name="repeat">number of times to repeat, set to -1 to repeat until unregistered.</param>
        /// <param name="action">method to invoke</param>
        public void AddTimer(FP delay, FP randomVariance, int repeat, int action)
        {
            Timer timer = null;

            if (!isInUpdate)
            {
                if (!this.sortedTimers.ContainsKey(action))
                {
					this.sortedTimers[action] = GetTimerFromPool();
                }
				timer = this.sortedTimers[action];
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
			timer.ScheduleAbsoluteTime(behaveWorld, elapsedTime);
        }

        public void RemoveTimer(long action)
        {
            if (!isInUpdate)
            {
                if (this.sortedTimers.ContainsKey(action))
                {
                    timerPool.Enqueue(this.sortedTimers[action]);
                    this.sortedTimers.Remove(action);
                }
            }
            else
            {
                if (this.sortedTimers.ContainsKey(action))
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
                return this.sortedTimers.ContainsKey(action);
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
                    return this.sortedTimers.ContainsKey(action);
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

        internal void Update(FP deltaTime)
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
            
			foreach (var kv in this.sortedTimers)
            {
                if (removeTimers.Contains(kv.Key))
                {
                    continue;
                }

				Timer timer = kv.Value;
                if (timer.scheduledTime <= elapsedTime)
                {
                    if (timer.repeat == 0)
                    {
                        RemoveTimer(kv.Key);
                    }
                    else if (timer.repeat >= 0)
                    {
                        timer.repeat--;
                    }
                    
                    behaveWorld.GuidReceiverMapping[kv.Key].OnTimerReached();
					timer.ScheduleAbsoluteTime(behaveWorld, elapsedTime);
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
                if (this.sortedTimers.TryGetValue(pair.Key, out var timer))
                {
                    timerPool.Enqueue(timer);
                }
                this.sortedTimers[pair.Key] = pair.Value;
            }
            foreach (var action in removeTimers)
            {
                timerPool.Enqueue(this.sortedTimers[action]);
                this.sortedTimers.Remove(action);
            }
            addObservers.Clear();
            removeObservers.Clear();
            addTimers.Clear();
            removeTimers.Clear();

            isInUpdate = false;
        }
        
#if UNITY_EDITOR
        [BsonIgnore][MemoryPackIgnore] public int NumUpdateObservers => updateObservers.Count;
        [BsonIgnore][MemoryPackIgnore] public int NumTimers => this.sortedTimers.Count;
        [BsonIgnore][MemoryPackIgnore] public FP ElapsedTime => elapsedTime;
        [BsonIgnore][MemoryPackIgnore] public int DebugPoolSize => timerPool.Count;
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