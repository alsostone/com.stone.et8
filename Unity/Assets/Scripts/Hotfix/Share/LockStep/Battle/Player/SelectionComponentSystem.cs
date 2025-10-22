
using System.Collections.Generic;
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
        }
        
        public static void MoveToPosition(this SelectionComponent self, TSVector2 position)
        {
            if (self.SelectedUnitIds.Count == 0) {
                return;
            }
            LSUnitComponent unitComponent = self.LSWorld().GetComponent<LSUnitComponent>();
            foreach (long id in self.SelectedUnitIds)
            {
                LSUnit unit = unitComponent.GetChild<LSUnit>(id);
                if (unit != null) {
                    unit.GetComponent<MovePathFindingComponent>()?.PathFinding(position);
                }
            }
        }
        
    }
}