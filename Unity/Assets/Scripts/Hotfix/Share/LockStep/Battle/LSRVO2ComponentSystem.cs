using System.Collections.Generic;
using RVO;
using TrueSync;

namespace ET
{
    [LSEntitySystemOf(typeof(LSRVO2Component))]
    [EntitySystemOf(typeof(LSRVO2Component))]
    [FriendOf(typeof(LSRVO2Component))]
    public static partial class LSRVO2ComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSRVO2Component self)
        {self.LSRoom()?.ProcessLog.LogFunction(25, self.LSParent().Id);
            self.RVO2Simulator = new Simulator();
            self.RVO2Simulator.setTimeStep((FP)LSConstValue.UpdateInterval / LSConstValue.Milliseconds);
        }
        
        [EntitySystem]
        private static void Deserialize(this LSRVO2Component self)
        {
            // TODO: 可能导致不一致，因为恢复Agent时 它所在的List位置可能会发生变化，需要在simulator保证插入顺序 后续再处理
            self.RVO2Simulator = new Simulator();
            self.RVO2Simulator.setTimeStep((FP)LSConstValue.UpdateInterval / LSConstValue.Milliseconds);
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this LSRVO2Component self)
        {self.LSRoom()?.ProcessLog.LogFunction(24, self.LSParent().Id);
            self.RVO2Simulator.doStep();
            foreach (Agent agent in self.RVO2Simulator.GetAllAgents())
            {
                if (agent.isRemoved) { continue; }
                agent.prefVelocity = TSVector2.zero;
                
                LSUnit lsUnit = self.LSUnit(agent.id);
                TransformComponent transformComponent = lsUnit.GetComponent<TransformComponent>();
                
                TSVector position = new TSVector(agent.position.x, transformComponent.Position.y, agent.position.y);
                if (TSVector.SqrDistance(transformComponent.Position, position) > FP.EN4)
                {
                    transformComponent.Position = position;
                }
            }
        }

        public static void AddDynamicAgent(this LSRVO2Component self, LSUnit lsUnit)
        {self.LSRoom()?.ProcessLog.LogFunction(23, self.LSParent().Id, lsUnit.Id);
            TransformComponent transformComponent = lsUnit.GetComponent<TransformComponent>();
            TSVector2 position = new(transformComponent.Position.x, transformComponent.Position.z);
            
            PropComponent propComponent = lsUnit.GetComponent<PropComponent>();
            FP speed = propComponent.Get(NumericType.Speed);
            
            self.RVO2Simulator.addAgent(position, FP.Two, 10, FP.Two, FP.EN2, propComponent.Radius, speed, TSVector2.zero, lsUnit.Id);
        }
        
        public static void AddStaticAgent(this LSRVO2Component self, LSUnit lsUnit)
        {self.LSRoom()?.ProcessLog.LogFunction(22, self.LSParent().Id, lsUnit.Id);
            TransformComponent transformComponent = lsUnit.GetComponent<TransformComponent>();
            TSVector2 position = new(transformComponent.Position.x, transformComponent.Position.z);
            
            PropComponent propComponent = lsUnit.GetComponent<PropComponent>();
            FP speed = propComponent.Get(NumericType.Speed);
            
            self.RVO2Simulator.addAgent(position, FP.Two, 0, FP.EN2, FP.EN2, propComponent.Radius, speed, TSVector2.zero, lsUnit.Id);
        }
        
        public static void RemoveAgent(this LSRVO2Component self, LSUnit lsUnit)
        {self.LSRoom()?.ProcessLog.LogFunction(21, self.LSParent().Id, lsUnit.Id);
            self.RVO2Simulator.removeAgent(lsUnit.Id);
        }
        
        public static void AddObstacle(this LSRVO2Component self, LSUnit lsUnit, List<TSVector2> vertices)
        {self.LSRoom()?.ProcessLog.LogFunction(20, self.LSParent().Id, lsUnit.Id);
            self.RVO2Simulator.addObstacle(vertices, lsUnit.Id);
        }
        
        public static void AddObstacle(this LSRVO2Component self, long obstacleId, List<TSVector2> vertices)
        {self.LSRoom()?.ProcessLog.LogFunction(19, self.LSParent().Id);
            self.RVO2Simulator.addObstacle(vertices, obstacleId);
        }
        
        public static void RemoveObstacle(this LSRVO2Component self, LSUnit lsUnit)
        {self.LSRoom()?.ProcessLog.LogFunction(18, self.LSParent().Id, lsUnit.Id);
            self.RVO2Simulator.removeObstacle(lsUnit.Id);
        }

        public static void SetAgentPosition(this LSRVO2Component self, LSUnit lsUnit, TSVector2 position)
        {self.LSRoom()?.ProcessLog.LogFunction(17, self.LSParent().Id, lsUnit.Id, position.x.V, position.y.V);
            self.RVO2Simulator.setAgentPosition(lsUnit.Id, position);
        }
        
        public static void setAgentPrefVelocity(this LSRVO2Component self, LSUnit lsUnit, TSVector2 velocity)
        {self.LSRoom()?.ProcessLog.LogFunction(16, self.LSParent().Id, lsUnit.Id, velocity.x.V, velocity.y.V);
            self.RVO2Simulator.setAgentPrefVelocity(lsUnit.Id, velocity);
        }

    }
}