using System.Collections.Generic;
using ST.GridBuilder;
using TrueSync;

namespace ET
{
    [EntitySystemOf(typeof(SelectionComponent))]
    [FriendOf(typeof(SelectionComponent))]
    public static partial class SelectionComponentSystem
    {
        [EntitySystem]
        private static void Awake(this SelectionComponent self)
        {
        }
        
        public static void SelectUnitsInBounds(this SelectionComponent self, TSBounds bounds)
        {
            LSTargetsComponent targetsComponent = self.LSWorld().GetComponent<LSTargetsComponent>();
            TeamType team = self.LSOwner().GetComponent<TeamComponent>().Type;
            targetsComponent.GetAttackTargets(team, EUnitType.Hero | EUnitType.Soldier, bounds, self.SelectedUnitIds);
            EventSystem.Instance.Publish(self.LSWorld(), new LSSelectionChanged() { Id = self.LSOwner().Id, SelectionIds = self.SelectedUnitIds });
        }
        
        public static void ClearSelection(this SelectionComponent self)
        {
            self.SelectedUnitIds.Clear();
            EventSystem.Instance.Publish(self.LSWorld(), new LSSelectionChanged() { Id = self.LSOwner().Id, SelectionIds = self.SelectedUnitIds });
        }
        
        public static void MoveToPosition(this SelectionComponent self, TSVector2 position, MovementMode movementMode)
        {
            if (self.SelectedUnitIds.Count == 0) {
                return;
            }
            
            LSGridMapComponent gridMapComponent = self.LSWorld().GetComponent<LSGridMapComponent>();
            LSUnitComponent unitComponent = self.LSWorld().GetComponent<LSUnitComponent>();
            List<MoveFlowFieldComponent> flowFieldComponents = ObjectPool.Instance.Fetch<List<MoveFlowFieldComponent>>();
            List<TSVector> positions = ObjectPool.Instance.Fetch<List<TSVector>>();

            TSVector center = TSVector.zero;
            foreach (long id in self.SelectedUnitIds)
            {
                LSUnit unit = unitComponent.GetChild<LSUnit>(id);
                if (unit != null) {
                    MoveFlowFieldComponent flowFieldComponent = unit.GetComponent<MoveFlowFieldComponent>();
                    flowFieldComponent.Stop();
                    TransformComponent transformComponent = unit.GetComponent<TransformComponent>();
                    positions.Add(transformComponent.Position);
                    center += transformComponent.Position;
                    flowFieldComponents.Add(flowFieldComponent);
                }
            }
            center /= flowFieldComponents.Count;
            
            TSVector destination = new TSVector(position.x, 0, position.y);
            TSVector forward = (destination - center).normalized;
            
            FormationPack.FormationGridPack(positions, center, destination, FP.Half, forward, TSVector.up);
            int flowFieldIndex = gridMapComponent.GenerateFlowField(positions);
            
            for (int index = 0; index < flowFieldComponents.Count; index++) {
                flowFieldComponents[index].MoveStart(flowFieldIndex, positions[index], movementMode);
            }
            
            positions.Clear();
            ObjectPool.Instance.Recycle(positions);
            flowFieldComponents.Clear();
            ObjectPool.Instance.Recycle(flowFieldComponents);
        }
        
    }
}