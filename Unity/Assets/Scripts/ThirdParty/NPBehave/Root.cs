﻿using System;
using MemoryPack;
using MongoDB.Bson.Serialization.Attributes;

namespace NPBehave
{
    [MemoryPackable]
    public partial class Root : Decorator
    {
        [BsonElement][MemoryPackInclude] private int blackboardGuid;
        
        [BsonIgnore][MemoryPackIgnore] public Blackboard RootBlackboard { get; private set; }
        [BsonIgnore][MemoryPackIgnore] public Clock RootClock { get; private set; }
        [BsonIgnore][MemoryPackIgnore] public BehaveWorld RootBehaveWorld { get; private set; }
        [BsonIgnore][MemoryPackIgnore] public IAgent RootAgent { get; private set; }

#if UNITY_EDITOR
        [BsonIgnore][MemoryPackIgnore] public int TotalNumStartCalls = 0;
        [BsonIgnore][MemoryPackIgnore] public int TotalNumStopCalls = 0;
        [BsonIgnore][MemoryPackIgnore] public int TotalNumStoppedCalls = 0;
#endif
        
        [MemoryPackConstructor]
        private Root(Node decoratee) : base("Root", decoratee)
        {
        }

        public Root(BehaveWorld world, Node decoratee, IAgent agent = null) : base("Root", decoratee)
        {
            RootBlackboard = world.CreateBlackboard();
            RootClock = world.Clock;
            RootBehaveWorld = world;
            RootAgent = agent;
            blackboardGuid = RootBlackboard.Guid;
            base.SetRoot(this);
        }
        
        public Root(BehaveWorld world, Blackboard blackboard, Node decoratee, IAgent agent = null) : base("Root", decoratee)
        {
            RootBlackboard = blackboard;
            RootClock = world.Clock;
            RootBehaveWorld = world;
            RootAgent = agent;
            blackboardGuid = RootBlackboard.Guid;
            base.SetRoot(this);
        }
        
        // 反序列化后 手动调用该接口 以恢复上下文
        public void SetWorld(BehaveWorld world, IAgent agent = null)
        {
            RootBlackboard = world.GetBlackboard(blackboardGuid);
            RootClock = world.Clock;
            RootBehaveWorld = world;
            RootAgent = agent;
            base.SetRoot(this);
        }

        protected override void DoStart()
        {
            Blackboard.Enable();
            Decoratee.Start();
        }

        protected override void DoStop()
        {
            if (Decoratee.IsActive)
            {
                Decoratee.Stop();
            }
            else
            {
                RootClock.RemoveTimer(Guid);
            }
        }
        
        protected override void DoChildStopped(Node node, bool success)
        {
            if (!IsStopRequested)
            {
                // wait one tick, to prevent endless recursions
                Clock.AddTimer(0, 0, Guid);
            }
            else
            {
                Blackboard.Disable();
                Stopped(success);
            }
        }

        public override void OnTimerReached()
        {
            Decoratee.Start();
        }
    }
}
