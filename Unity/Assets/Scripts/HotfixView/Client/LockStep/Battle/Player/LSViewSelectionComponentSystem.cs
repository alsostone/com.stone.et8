using System.Collections.Generic;

namespace ET.Client
{
    [LSEntitySystemOf(typeof(LSViewSelectionComponent))]
    [EntitySystemOf(typeof(LSViewSelectionComponent))]
    [FriendOf(typeof(LSViewSelectionComponent))]
    public static partial class LSViewSelectionComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSViewSelectionComponent self)
        {
        }
        
        [LSEntitySystem]
        private static void LSRollback(this LSViewSelectionComponent self)
        {
            SelectionComponent selectionComponent = self.LSViewOwner().GetUnit().GetComponent<SelectionComponent>();
            self.ResetSelection(selectionComponent.SelectedUnitIds);
        }
        
        public static void ResetSelection(this LSViewSelectionComponent self, List<long> selectedIds)
        {
            LSUnitViewComponent lsUnitViewComponent = self.Room().GetComponent<LSUnitViewComponent>();
            foreach (long selectId in selectedIds)
            {
                if (!self.SelectedIdSet.Remove(selectId))
                {
                    LSUnitView lsUnitView = lsUnitViewComponent.GetChild<LSUnitView>(selectId);
                    // TODO: 新增选中效果
                }
            }
            foreach (long unselectId in self.SelectedIdSet)
            {
                LSUnitView lsUnitView = lsUnitViewComponent.GetChild<LSUnitView>(unselectId);
                // TODO: 取消选中效果
            }
            
            self.SelectedIdSet.Clear();
            foreach (long selectId in selectedIds)
            {
                self.SelectedIdSet.Add(selectId);
            }
        }
        
    }
}