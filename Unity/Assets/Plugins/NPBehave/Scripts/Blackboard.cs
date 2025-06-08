using System.Collections.Generic;
using MemoryPack;

namespace NPBehave
{
    public enum NotifyType
    {
        ADD,
        REMOVE,
        CHANGE
    }
    
    [MemoryPackable]
    public partial struct Notification
    {
        public readonly string key;
        public readonly NotifyType type;

        public Notification(string key, NotifyType type)
        {
            this.key = key;
            this.type = type;
        }
    }
    
    [MemoryPackable]
    public partial class Blackboard : Receiver
    {
        [MemoryPackInclude] private int parentGuid;
        [MemoryPackInclude] private HashSet<int> childrenGuid = new HashSet<int>();
        [MemoryPackInclude] private Dictionary<string, bool> dataBool = new Dictionary<string, bool>();
        [MemoryPackInclude] private Dictionary<string, int> dataInt = new Dictionary<string, int>();
        [MemoryPackInclude] private Dictionary<string, float> dataFloat = new Dictionary<string, float>();
        
        [MemoryPackInclude] private bool isNotifying = false;
        [MemoryPackInclude] private List<Notification> notifications = new List<Notification>();
        [MemoryPackInclude] private List<Notification> notificationsDispatch = new List<Notification>();
        
        [MemoryPackInclude] private Dictionary<string, List<int>> keyObserversMapping = new Dictionary<string, List<int>>();
        [MemoryPackInclude] private Dictionary<string, List<int>> keyAddObserversMapping = new Dictionary<string, List<int>>();
        [MemoryPackInclude] private Dictionary<string, List<int>> keyRemoveObserversMapping = new Dictionary<string, List<int>>();
        
        [MemoryPackIgnore] private BehaveWorld behaveWorld;
        [MemoryPackIgnore] private Blackboard parent;
        [MemoryPackIgnore] private Clock clock;

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
                    notifications.Add(new Notification(key, NotifyType.ADD));
                    clock.AddTimer(0f, 0, Guid);
                }
                else
                {
                    if (!dataValue.Equals(value))
                    {
                        dataBool[key] = value;
                        notifications.Add(new Notification(key, NotifyType.CHANGE));
                        clock.AddTimer(0f, 0, Guid);
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
                    notifications.Add(new Notification(key, NotifyType.ADD));
                    clock.AddTimer(0f, 0, Guid);
                }
                else
                {
                    if (!dataValue.Equals(value))
                    {
                        dataInt[key] = value;
                        notifications.Add(new Notification(key, NotifyType.CHANGE));
                        clock.AddTimer(0f, 0, Guid);
                    }
                }
            }
        }
        
        public void SetFloat(string key, float value)
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
                    notifications.Add(new Notification(key, NotifyType.ADD));
                    clock.AddTimer(0f, 0, Guid);
                }
                else
                {
                    if (!dataValue.Equals(value))
                    {
                        dataFloat[key] = value;
                        notifications.Add(new Notification(key, NotifyType.CHANGE));
                        clock.AddTimer(0f, 0, Guid);
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
                notifications.Add(new Notification(key, NotifyType.REMOVE));
                clock.AddTimer(0f, 0, Guid);
            }
        }

        public void UnSetInt(string key)
        {
            if (dataInt.ContainsKey(key))
            {
                dataInt.Remove(key);
                notifications.Add(new Notification(key, NotifyType.REMOVE));
                clock.AddTimer(0f, 0, Guid);
            }
        }
        
        public void UnSetFloat(string key)
        {
            if (dataFloat.ContainsKey(key))
            {
                dataFloat.Remove(key);
                notifications.Add(new Notification(key, NotifyType.REMOVE));
                clock.AddTimer(0f, 0, Guid);
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
        public float GetFloat(string key)
        {
            if (dataFloat.TryGetValue(key, out var value))
            {
                return value;
            }
            if (parent != null)
            {
                return parent.GetFloat(key);
            }
            return 0f;
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
        
        public void ForeachFloat(System.Action<string, float> action)
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
        
        [MemoryPackIgnore] public int NumObservers
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
        
        [MemoryPackIgnore] public int NumData
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
                childBlackboard.clock.AddTimer(0f, 0, child);
            }
            notifications.Clear();

            isNotifying = true;
            foreach (var notification in notificationsDispatch)
            {
                if (!keyObserversMapping.ContainsKey(notification.key))
                {
                    continue;
                }

                var observers = GetObservers(keyObserversMapping, notification.key);
                foreach (var observer in observers)
                {
                    if (keyRemoveObserversMapping.TryGetValue(notification.key, out var removeObservers) && removeObservers.Contains(observer))
                    {
                        continue;
                    }
                    behaveWorld.GuidReceiverMapping[observer].OnObservingChanged(notification.type);
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
