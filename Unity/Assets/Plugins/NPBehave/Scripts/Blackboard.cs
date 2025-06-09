using System;
using System.Collections.Generic;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;
using TrueSync;

namespace NPBehave
{
    public enum BlackboardChangeType
    {
        ADD,
        REMOVE,
        CHANGE
    }
    
    [MemoryPackable]
    public partial struct Notification
    {
        public string blackboardKey;
        public BlackboardChangeType changeType;

        public Notification(string blackboardKey, BlackboardChangeType changeType)
        {
            this.blackboardKey = blackboardKey;
            this.changeType = changeType;
        }
    }
    
    [MemoryPackable]
    public partial class Blackboard : Receiver, IDisposable
    {
        [BsonElement][MemoryPackInclude] private int parentGuid;
        [BsonElement][MemoryPackInclude] private HashSet<int> childrenGuid = new HashSet<int>();
        [BsonElement][MemoryPackInclude] private Dictionary<string, bool> dataBool = new Dictionary<string, bool>();
        [BsonElement][MemoryPackInclude] private Dictionary<string, int> dataInt = new Dictionary<string, int>();
        [BsonElement][MemoryPackInclude] private Dictionary<string, FP> dataFloat = new Dictionary<string, FP>();
        
        [BsonElement][MemoryPackInclude] private bool isNotifying = false;
        [BsonElement][MemoryPackInclude] private List<Notification> notifications = new List<Notification>();
        [BsonElement][MemoryPackInclude] private List<Notification> notificationsDispatch = new List<Notification>();
        
        [BsonElement][MemoryPackInclude] private Dictionary<string, List<int>> keyObserversMapping = new Dictionary<string, List<int>>();
        [BsonElement][MemoryPackInclude] private Dictionary<string, List<int>> keyAddObserversMapping = new Dictionary<string, List<int>>();
        [BsonElement][MemoryPackInclude] private Dictionary<string, List<int>> keyRemoveObserversMapping = new Dictionary<string, List<int>>();
        
        [BsonIgnore][MemoryPackIgnore] private BehaveWorld behaveWorld;
        [BsonIgnore][MemoryPackIgnore] private Blackboard parent;
        [BsonIgnore][MemoryPackIgnore] private Clock clock;

        [MemoryPackConstructor]
        private Blackboard() { }

        internal Blackboard(BehaveWorld world, Blackboard parent)
        {
            this.Guid = world.GetNextGuid();
            this.behaveWorld = world;
            this.parent = parent;
            this.clock = world.Clock;
            this.parentGuid = parent?.Guid ?? -1;
            world.GuidReceiverMapping.Add(Guid, this);
        }

        public void Dispose()
        {
            behaveWorld = null;
            parent = null;
            clock = null;
        }

        internal void SetWorld(BehaveWorld world)
        {
            behaveWorld = world;
            parent = world.GetBlackboard(parentGuid);
            clock = world.Clock;
            world.GuidReceiverMapping.Add(Guid, this);
        }

        public void Enable()
        {
            if (parent != null)
            {
                parent.childrenGuid.Add(Guid);
            }
        }

        public void Disable()
        {
            if (parent != null)
            {
                parent.childrenGuid.Remove(Guid);
            }
            if (clock != null)
            {
                clock.RemoveTimer(Guid);
            }
        }
        
        #region Set 
        public void SetBool(string key, bool value)
        {
            if (parent != null && parent.IsSetBool(key))
            {
                parent.SetBool(key, value);
            }
            else
            {
                if (!dataBool.TryGetValue(key, out var dataValue))
                {
                    dataBool[key] = value;
                    notifications.Add(new Notification(key, BlackboardChangeType.ADD));
                    clock.AddTimer(FP.Zero, 0, Guid);
                }
                else
                {
                    if (!dataValue.Equals(value))
                    {
                        dataBool[key] = value;
                        notifications.Add(new Notification(key, BlackboardChangeType.CHANGE));
                        clock.AddTimer(FP.Zero, 0, Guid);
                    }
                }
            }
        }
        
        public void SetInt(string key, int value)
        {
            if (parent != null && parent.IsSetInt(key))
            {
                parent.SetInt(key, value);
            }
            else
            {
                if (!dataInt.TryGetValue(key, out var dataValue))
                {
                    dataInt[key] = value;
                    notifications.Add(new Notification(key, BlackboardChangeType.ADD));
                    clock.AddTimer(FP.Zero, 0, Guid);
                }
                else
                {
                    if (!dataValue.Equals(value))
                    {
                        dataInt[key] = value;
                        notifications.Add(new Notification(key, BlackboardChangeType.CHANGE));
                        clock.AddTimer(FP.Zero, 0, Guid);
                    }
                }
            }
        }
        
        public void SetFloat(string key, FP value)
        {
            if (parent != null && parent.IsSetFloat(key))
            {
                parent.SetFloat(key, value);
            }
            else
            {
                if (!dataFloat.TryGetValue(key, out var dataValue))
                {
                    dataFloat[key] = value;
                    notifications.Add(new Notification(key, BlackboardChangeType.ADD));
                    clock.AddTimer(FP.Zero, 0, Guid);
                }
                else
                {
                    if (!dataValue.Equals(value))
                    {
                        dataFloat[key] = value;
                        notifications.Add(new Notification(key, BlackboardChangeType.CHANGE));
                        clock.AddTimer(FP.Zero, 0, Guid);
                    }
                }
            }
        }
        #endregion
        
        #region UnSet
        public void UnSetBool(string key)
        {
            if (dataBool.ContainsKey(key))
            {
                dataBool.Remove(key);
                notifications.Add(new Notification(key, BlackboardChangeType.REMOVE));
                clock.AddTimer(FP.Zero, 0, Guid);
            }
        }

        public void UnSetInt(string key)
        {
            if (dataInt.ContainsKey(key))
            {
                dataInt.Remove(key);
                notifications.Add(new Notification(key, BlackboardChangeType.REMOVE));
                clock.AddTimer(FP.Zero, 0, Guid);
            }
        }
        
        public void UnSetFloat(string key)
        {
            if (dataFloat.ContainsKey(key))
            {
                dataFloat.Remove(key);
                notifications.Add(new Notification(key, BlackboardChangeType.REMOVE));
                clock.AddTimer(FP.Zero, 0, Guid);
            }
        }
        #endregion
        
        #region IsSet
        public bool IsSetBool(string key)
        {
            return dataBool.ContainsKey(key) || (parent != null && parent.IsSetBool(key));
        }
        public bool IsSetInt(string key)
        {
            return dataInt.ContainsKey(key) || (parent != null && parent.IsSetInt(key));
        }
        public bool IsSetFloat(string key)
        {
            return dataFloat.ContainsKey(key) || (parent != null && parent.IsSetFloat(key));
        }
        #endregion

        #region Get
        public bool GetBool(string key)
        {
            if (dataBool.TryGetValue(key, out var value))
            {
                return value;
            }
            if (parent != null)
            {
                return parent.GetBool(key);
            }
            return false;
        }
        public int GetInt(string key)
        {
            if (dataInt.TryGetValue(key, out var value))
            {
                return value;
            }
            if (parent != null)
            {
                return parent.GetInt(key);
            }
            return 0;
        }
        public FP GetFloat(string key)
        {
            if (dataFloat.TryGetValue(key, out var value))
            {
                return value;
            }
            if (parent != null)
            {
                return parent.GetFloat(key);
            }
            return FP.Zero;
        }
        #endregion
        
#if UNITY_EDITOR
        public void ForeachBool(System.Action<string, bool> action)
        {
            if (parent != null)
            {
                parent.ForeachBool(action);
            }
            foreach (var pair in dataBool)
            {
                action(pair.Key, pair.Value);
            }
        }
        
        public void ForeachInt(System.Action<string, int> action)
        {
            if (parent != null)
            {
                parent.ForeachInt(action);
            }
            foreach (var pair in dataInt)
            {
                action(pair.Key, pair.Value);
            }
        }
        
        public void ForeachFloat(System.Action<string, FP> action)
        {
            if (parent != null)
            {
                parent.ForeachFloat(action);
            }
            foreach (var pair in dataFloat)
            {
                action(pair.Key, pair.Value);
            }
        }
        
        [BsonIgnore][MemoryPackIgnore] public int NumObservers
        {
            get
            {
                int count = 0;
                foreach (var pair in keyObserversMapping)
                {
                    count += pair.Value.Count;
                }
                return count;
            }
        }
        
        [BsonIgnore][MemoryPackIgnore] public int NumData
        {
            get
            {
                int count = 0;
                count += dataBool.Count;
                count += dataInt.Count;
                count += dataFloat.Count;
                return count;
            }
        }
#endif
        
        public void AddObserver(string key, int observer)
        {
            var observers = GetObservers(keyObserversMapping, key);
            if (!isNotifying)
            {
                if (!observers.Contains(observer))
                {
                    observers.Add(observer);
                }
            }
            else
            {
                if (!observers.Contains(observer))
                {
                    var addObservers = GetObservers(keyAddObserversMapping, key);
                    if (!addObservers.Contains(observer))
                    {
                        addObservers.Add(observer);
                    }
                }

                var removeObservers = GetObservers(keyRemoveObserversMapping, key);
                if (removeObservers.Contains(observer))
                {
                    removeObservers.Remove(observer);
                }
            }
        }

        public void RemoveObserver(string key, int observer)
        {
            var observers = GetObservers(keyObserversMapping, key);
            if (!isNotifying)
            {
                if (observers.Contains(observer))
                {
                    observers.Remove(observer);
                }
            }
            else
            {
                var removeObservers = GetObservers(keyRemoveObserversMapping, key);
                if (!removeObservers.Contains(observer))
                {
                    if (observers.Contains(observer))
                    {
                        removeObservers.Add(observer);
                    }
                }

                var addObservers = GetObservers(keyAddObserversMapping, key);
                if (addObservers.Contains(observer))
                {
                    addObservers.Remove(observer);
                }
            }
        }

        private List<int> GetObservers(Dictionary<string, List<int>> targetMapping, string key)
        {
            if (!targetMapping.TryGetValue(key, out var observers))
            {
                observers = new List<int>();
                targetMapping.Add(key, observers);
            }
            return observers;
        }
        
        public override void OnTimerReached()
        {
            if (notifications.Count == 0)
            {
                return;
            }

            notificationsDispatch.Clear();
            notificationsDispatch.AddRange(notifications);
            foreach (var child in childrenGuid)
            {
                var childBlackboard = behaveWorld.GetBlackboard(child);
                childBlackboard.notifications.AddRange(notifications);
                childBlackboard.clock.AddTimer(FP.Zero, 0, child);
            }
            notifications.Clear();

            isNotifying = true;
            foreach (var notification in notificationsDispatch)
            {
                if (!keyObserversMapping.ContainsKey(notification.blackboardKey))
                {
                    continue;
                }

                var observers = GetObservers(keyObserversMapping, notification.blackboardKey);
                foreach (var observer in observers)
                {
                    if (keyRemoveObserversMapping.TryGetValue(notification.blackboardKey, out var removeObservers) && removeObservers.Contains(observer))
                    {
                        continue;
                    }
                    behaveWorld.GuidReceiverMapping[observer].OnObservingChanged(notification.changeType);
                }
            }

            foreach (var pair in keyAddObserversMapping)
            {
                GetObservers(keyObserversMapping, pair.Key).AddRange(pair.Value);
            }
            foreach (var pair in keyRemoveObserversMapping)
            {
                var observers = GetObservers(keyObserversMapping, pair.Key);
                foreach (var action in pair.Value)
                {
                    observers.Remove(action);
                }
            }
            keyAddObserversMapping.Clear();
            keyRemoveObserversMapping.Clear();

            isNotifying = false;
        }

    }
}
