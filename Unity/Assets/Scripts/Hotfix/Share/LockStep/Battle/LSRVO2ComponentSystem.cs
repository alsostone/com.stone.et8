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
        {
            self.RVO2Simulator = new Simulator();
            self.RVO2Simulator.setTimeStep((FP)LSConstValue.UpdateInterval / LSConstValue.Milliseconds);
        }
        
        [EntitySystem]
        private static void Deserialize(this LSRVO2Component self)
        {
            self.RVO2Simulator.ClearAllAgents();
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this LSRVO2Component self)
        {
            self.RVO2Simulator.doStep();
            foreach (Agent agent in self.RVO2Simulator.GetAllAgents())
            {
                if (agent.isRemoved || agent.isStatic) { continue; }
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
        {
            TransformComponent transformComponent = lsUnit.GetComponent<TransformComponent>();
            TSVector2 position = new(transformComponent.Position.x, transformComponent.Position.z);
            
            PropComponent propComponent = lsUnit.GetComponent<PropComponent>();
            FP speed = propComponent.Get(NumericType.Speed);
            
            self.RVO2Simulator.addAgent(position, 3, 6, 4, 8, propComponent.Radius, speed, TSVector2.zero, false, lsUnit.Id);
        }
        
        public static void AddStaticAgent(this LSRVO2Component self, LSUnit lsUnit)
        {
            TransformComponent transformComponent = lsUnit.GetComponent<TransformComponent>();
            TSVector2 position = new(transformComponent.Position.x, transformComponent.Position.z);
            
            PropComponent propComponent = lsUnit.GetComponent<PropComponent>();
            self.RVO2Simulator.addAgent(position, 0, 0, 0, 0, propComponent.Radius, 0, TSVector2.zero, true, lsUnit.Id);
        }
        
        public static void RemoveAgent(this LSRVO2Component self, LSUnit lsUnit)
        {
            self.RVO2Simulator.removeAgent(lsUnit.Id);
        }
        
        public static void SetAgentPosition(this LSRVO2Component self, LSUnit lsUnit, TSVector position)
        {
            self.RVO2Simulator.setAgentPosition(lsUnit.Id, new TSVector2(position.x, position.z));
        }
        
        public static void setAgentPrefVelocity(this LSRVO2Component self, LSUnit lsUnit, TSVector2 velocity)
        {
            self.RVO2Simulator.setAgentPrefVelocity(lsUnit.Id, velocity);
        }

    }
}