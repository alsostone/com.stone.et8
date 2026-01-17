using MongoDB.Bson.Serialization.Attributes;
using System;
using MemoryPack;
using TrueSync;
using ST.GridBuilder;

namespace ET
{
    [EnableMethod]
    [ChildOf]
    [MemoryPackable]
    public partial class LSWorld: Entity, IAwake, IScene
    {
        [MemoryPackConstructor]
        public LSWorld()
        {
        }
        
        public LSWorld(SceneType sceneType)
        {
            this.Id = this.GetId();

            this.SceneType = sceneType;
        }

        private readonly LSUpdater updater = new();
        
        [BsonIgnore]
        [MemoryPackIgnore]
        public Fiber Fiber { get; set; }
        
        [BsonElement]
        [MemoryPackInclude]
        private long idGenerator;

        public long GetId()
        {
            return ++this.idGenerator;
        }

        public TSRandom Random { get; set; }
        
        [BsonIgnore]
        [MemoryPackIgnore]
        public SceneType SceneType { get; set; } = SceneType.LockStepClient;
        
        public OperationMode OperationMode { get; set; } = OperationMode.Shooting;
        
        public int Frame { get; set; }
        public int EndFrame { get; set; } = -1;
        
        public FP DeltaTime { get; private set; }
        public FP ElapsedTime { get; private set; }
        public FP TimeScale { get; set; } = 1; // 用于加速/减速游戏 必须保证所有客户端一致

        public void Update()
        {
            DeltaTime = FP.One / LSConstValue.FrameCountPerSecond * TimeScale;
            ElapsedTime += DeltaTime;
            this.updater.Update();
        }

        public void RegisterSystem(LSEntity component)
        {
            Type type = component.GetType();

            TypeSystems.OneTypeSystems oneTypeSystems = LSEntitySystemSingleton.Instance.TypeSystems.GetOneTypeSystems(type);
            if (oneTypeSystems == null)
            {
                return;
            }
            for (int i = 0; i < oneTypeSystems.QueueFlag.Length; ++i)
            {
                if (!oneTypeSystems.QueueFlag[i])
                {
                    continue;
                }
                this.updater.Add(component);
            }
        }
        
        public new K AddComponent<K>(bool isFromPool = false) where K : LSEntity, IAwake, new()
        {
            return this.AddComponentWithId<K>(this.GetId(), isFromPool);
        }

        public new K AddComponent<K, P1>(P1 p1, bool isFromPool = false) where K : LSEntity, IAwake<P1>, new()
        {
            return this.AddComponentWithId<K, P1>(this.GetId(), p1, isFromPool);
        }

        public new K AddComponent<K, P1, P2>(P1 p1, P2 p2, bool isFromPool = false) where K : LSEntity, IAwake<P1, P2>, new()
        {
            return this.AddComponentWithId<K, P1, P2>(this.GetId(), p1, p2, isFromPool);
        }

        public new K AddComponent<K, P1, P2, P3>(P1 p1, P2 p2, P3 p3, bool isFromPool = false) where K : LSEntity, IAwake<P1, P2, P3>, new()
        {
            return this.AddComponentWithId<K, P1, P2, P3>(this.GetId(), p1, p2, p3, isFromPool);
        }

        public new T AddChild<T>(bool isFromPool = false) where T : LSEntity, IAwake
        {
            return this.AddChildWithId<T>(this.GetId(), isFromPool);
        }

        public new T AddChild<T, A>(A a, bool isFromPool = false) where T : LSEntity, IAwake<A>
        {
            return this.AddChildWithId<T, A>(this.GetId(), a, isFromPool);
        }

        public new T AddChild<T, A, B>(A a, B b, bool isFromPool = false) where T : LSEntity, IAwake<A, B>
        {
            return this.AddChildWithId<T, A, B>(this.GetId(), a, b, isFromPool);
        }

        public new T AddChild<T, A, B, C>(A a, B b, C c, bool isFromPool = false) where T : LSEntity, IAwake<A, B, C>
        {
            return this.AddChildWithId<T, A, B, C>(this.GetId(), a, b, c, isFromPool);
        }
        
        protected override long GetLongHashCode(Type type)
        {
            return LSEntitySystemSingleton.Instance.GetLongHashCode(type);
        }
    }
}