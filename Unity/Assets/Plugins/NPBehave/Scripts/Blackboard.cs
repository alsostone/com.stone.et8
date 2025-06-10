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
        [BsonElement("P")][BsonIgnoreIfDefault][MemoryPackInclude] private int parentGuid;
        [BsonElement("C")][BsonIgnoreIfNull][MemoryPackInclude] private HashSet<int> childrenGuid;
        [BsonElement("DB")][BsonIgnoreIfNull][MemoryPackInclude] private Dictionary<string, bool> dataBool;
        [BsonElement("DI")][BsonIgnoreIfNull][MemoryPackInclude] private Dictionary<string, int> dataInt;
        [BsonElement("DF")][BsonIgnoreIfNull][MemoryPackInclude] private Dictionary<string, FP> dataFloat;
        
        [BsonElement("IS")][BsonIgnoreIfDefault][MemoryPackInclude] private bool isNotifying = false;
        [BsonElement("NF")][MemoryPackInclude] private List<Notification> notifications = new List<Notification>();
        [BsonElement("ND")][MemoryPackInclude] private List<Notification> notificationsDispatch = new List<Notification>();
        
        [BsonElement("OB")][BsonIgnoreIfNull][MemoryPackInclude] private Dictionary<string, List<int>> keyObserversMapping;
        [BsonElement("AB")][BsonIgnoreIfNull][MemoryPackInclude] private Dictionary<string, List<int>> keyAddObserversMapping;
        [BsonElement("RB")][BsonIgnoreIfNull][MemoryPackInclude] private Dictionary<string, List<int>> keyRemoveObserversMapping;
        
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
            this.parentGuid = parent?.Guid ?? 0;
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
                if (parent.childrenGuid == null)
                    parent.childrenGuid = new HashSet<int>();
                parent.childrenGuid.Add(Guid);
            }
        }

        public void Disable()
        {
            if (parent != null)
            {
                parent.childrenGuid?.Remove(Guid);
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
                if (dataBool == null)
                {
                    dataBool = new Dictionary<string, bool>();
                }
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
                if (dataInt == null)
                {
                    dataInt = new Dictionary<string, int>();
                }
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
                if (dataFloat == null)
                {
                    dataFloat = new Dictionary<string, FP>();
                }
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
            if (dataBool != null && dataBool.ContainsKey(key))
            {
                dataBool.Remove(key);
                notifications.Add(new Notification(key, BlackboardChangeType.REMOVE));
                clock.AddTimer(FP.Zero, 0, Guid);
            }
        }

        public void UnSetInt(string key)
        {
            if (dataInt != null && dataInt.ContainsKey(key))
            {
                dataInt.Remove(key);
                notifications.Add(new Notification(key, BlackboardChangeType.REMOVE));
                clock.AddTimer(FP.Zero, 0, Guid);
            }
        }
        
        public void UnSetFloat(string key)
        {
            if (dataFloat != null && dataFloat.ContainsKey(key))
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
            return dataBool != null && (dataBool.ContainsKey(key) || (parent != null && parent.IsSetBool(key)));
        }
        public bool IsSetInt(string key)
        {
            return dataInt != null && (dataInt.ContainsKey(key) || (parent != null && parent.IsSetInt(key)));
        }
        public bool IsSetFloat(string key)
        {
            return dataFloat != null && (dataFloat.ContainsKey(key) || (parent != null && parent.IsSetFloat(key)));
        }
        #endregion

        #region Get
        public bool GetBool(string key)
        {
            if (dataBool != null && dataBool.TryGetValue(key, out var value))
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
            if (dataInt != null && dataInt.TryGetValue(key, out var value))
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
            if (dataFloat != null && dataFloat.TryGetValue(key, out var value))
            {
                return value;
            }
            if (parent != null)
            {
                return parent.GetFloat(key);
            }
            return 0;
        }
        #endregion
        
#if UNITY_EDITOR
        public void ForeachBool(System.Action<string, bool> action)
        {
            if (parent != null)
            {
                parent.ForeachBool(action);
            }
            if (dataBool == null)
            {
                return;
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
            if (dataInt == null)
            {
                return;
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
            if (dataFloat == null)
            {
                return;
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
                if (keyObserversMapping != null)
                {
                    foreach (var pair in keyObserversMapping)
                    {
                        count += pair.Value.Count;
                    }
                }
                return count;
            }
        }
        
        [BsonIgnore][MemoryPackIgnore] public int NumData
        {
            get
            {
                int count = 0;
                count += dataBool?.Count??0;
                count += dataInt?.Count??0;
                count += dataFloat?.Count??0;
                return count;
            }
        }
#endif
        
        public void AddObserver(string key, int observer)
        {
            var observers = GetObservers(ref keyObserversMapping, key);
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
                    var addObservers = GetObservers(ref keyAddObserversMapping, key);
                    if (!addObservers.Contains(observer))
                    {
                        addObservers.Add(observer);
                    }
                }

                var removeObservers = GetObservers(ref keyRemoveObserversMapping, key);
                if (removeObservers.Contains(observer))
                {
                    removeObservers.Remove(observer);
                }
            }
        }

        public void RemoveObserver(string key, int observer)
        {
            var observers = GetObservers(ref keyObserversMapping, key);
            if (!isNotifying)
            {
                if (observers.Contains(observer))
                {
                    observers.Remove(observer);
                }
            }
            else
            {
                var removeObservers = GetObservers(ref keyRemoveObserversMapping, key);
                if (!removeObservers.Contains(observer))
                {
                    if (observers.Contains(observer))
                    {
                        removeObservers.Add(observer);
                    }
                }

                var addObservers = GetObservers(ref keyAddObserversMapping, key);
                if (addObservers.Contains(observer))
                {
                    addObservers.Remove(observer);
                }
            }
        }

        private List<int> GetObservers(ref Dictionary<string, List<int>> targetMapping, string key)
        {
            if (targetMapping == null)
            {
                targetMapping = new Dictionary<string, List<int>>();
            }
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
            if (childrenGuid != null)
            {
                foreach (var child in childrenGuid)
                {
                    var childBlackboard = behaveWorld.GetBlackboard(child);
                    childBlackboard.notifications.AddRange(notifications);
                    childBlackboard.clock.AddTimer(FP.Zero, 0, child);
                }
            }
            notifications.Clear();

            isNotifying = true;
            foreach (var notification in notificationsDispatch)
            {
                if (keyObserversMapping == null || !keyObserversMapping.ContainsKey(notification.blackboardKey))
                {
                    continue;
                }

                var observers = GetObservers(ref keyObserversMapping, notification.blackboardKey);
                foreach (var observer in observers)
                {
                    if (keyRemoveObserversMapping.TryGetValue(notification.blackboardKey, out var removeObservers) && removeObservers.Contains(observer))
                    {
                        continue;
                    }
                    behaveWorld.GuidReceiverMapping[observer].OnObservingChanged(notification.changeType);
                }
            }

            if (keyAddObserversMapping != null)
            {
                foreach (var pair in keyAddObserversMapping)
                {
                    GetObservers(ref keyObserversMapping, pair.Key).AddRange(pair.Value);
                }
                keyAddObserversMapping.Clear();
            }

            if (keyRemoveObserversMapping != null)
            {
                foreach (var pair in keyRemoveObserversMapping)
                {
                    var observers = GetObservers(ref keyObserversMapping, pair.Key);
                    foreach (var action in pair.Value)
                    {
                        observers.Remove(action);
                    }
                }
                keyRemoveObserversMapping.Clear();
            }
            isNotifying = false;
        }

    }
}
