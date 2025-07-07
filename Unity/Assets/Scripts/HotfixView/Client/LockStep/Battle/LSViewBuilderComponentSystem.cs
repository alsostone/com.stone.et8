using System;
using ST.GridBuilder;
using TrueSync;
using UnityEngine;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSViewBuilderComponent))]
    [FriendOf(typeof(LSViewBuilderComponent))]
    [FriendOf(typeof(LSViewPlacementComponent))]
    public static partial class LSViewBuilderComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSViewBuilderComponent self)
        {
        }

        [EntitySystem]
        private static void Destroy(this LSViewBuilderComponent self)
        {
            self.OnPlacementCancel();
        }
        
        public static void OnPlacementDragStart(this LSViewBuilderComponent self, long targetId)
        {
            if (!self.DragPlacement)
            {
                LSUnitViewComponent unitViewComponent = self.Room().GetComponent<LSUnitViewComponent>();
                LSUnitView lsUnitView = unitViewComponent.GetChild<LSUnitView>(targetId);
                Placement placement = lsUnitView?.GetComponent<LSViewPlacementComponent>()?.Placement;
                if (placement == null)
                    return;
                
                LSViewGridMapComponent gridMapComponent = self.Room().GetComponent<LSViewGridMapComponent>();
                if (gridMapComponent.GridMap.gridData.CanTake(placement.placementData))
                {
                    self.DragUnitView = lsUnitView;
                    lsUnitView.GetComponent<LSViewTransformComponent>().SetTransformEnabled(false);
                    self.DragPlacement = placement;
                    self.DragPlacement.SetPreviewMaterial();
                    self.DragOffset = new Vector3(0, float.MaxValue, 0);
                } else {
                    placement.DoShake();
                }
            }
        }

        public static void OnPlacementDrag(this LSViewBuilderComponent self, TSVector2 position)
        {
            if (self.DragPlacement)
            {
                LSViewGridMapComponent gridMapComponent = self.Room().GetComponent<LSViewGridMapComponent>();
                GridMap gridMap = gridMapComponent.GridMap;
                
                Vector3 pos = new(position.x.AsFloat(), 0, position.y.AsFloat());
                if (Math.Abs(self.DragOffset.y - float.MaxValue) < float.Epsilon) {
                    self.DragOffset = self.DragPlacement.GetPosition() - pos;
                }
                
                IndexV2 index = gridMap.ConvertToIndex(pos + self.DragOffset);
                int targetLevel = gridMap.gridData.GetShapeLevelCount(index.x, index.z, self.DragPlacement.placementData);
                self.DragPlacement.SetMovePosition(gridMap.GetLevelPosition(index.x, index.z, targetLevel));
                
                if (gridMapComponent.GridMapIndicator) {
                    gridMapComponent.GridMapIndicator.GenerateIndicator(index.x, index.z, targetLevel, self.DragPlacement.placementData);
                }
            }
        }

        public static void OnPlacementDragEnd(this LSViewBuilderComponent self, TSVector2 position)
        {
            if (self.DragPlacement)
            {
                // 这里恢复原状即可 放置结果由逻辑层处理并通知给表现层
                self.OnPlacementCancel();
                LSViewGridMapComponent gridMapComponent = self.Room().GetComponent<LSViewGridMapComponent>();
                if (gridMapComponent.GridMapIndicator) {
                    gridMapComponent.GridMapIndicator.ClearIndicator();
                }
            }
        }

        public static void OnPlacementStart(this LSViewBuilderComponent self, EUnitType type, int tableId, int level)
        {
            self.OnPlacementCancel();
            int targetModel = 0;
            switch (type)
            {
                case EUnitType.Block:
                    targetModel = TbBlock.Instance.Get(tableId).Model;
                    break;
                case EUnitType.Building:
                    targetModel = TbBuilding.Instance.Get(tableId, level).Model;
                    break;
            }
            if (targetModel == 0) {
                return;
            }
            
            Scene root = self.Root();
            TbResourceRow resourceRow = TbResource.Instance.Get(targetModel);
            GameObject prefab = root.GetComponent<ResourcesLoaderComponent>().LoadAssetSync<GameObject>(resourceRow.Url);
            GlobalComponent globalComponent = root.GetComponent<GlobalComponent>();
            GameObject go = UnityEngine.Object.Instantiate(prefab, globalComponent.Unit, true);
            Placement placement = go.GetComponent<Placement>();
            if (placement == null)
            {
                UnityEngine.Object.DestroyImmediate(go);
            }
            else
            {
                self.DragPlacement = placement;
                self.DragPlacement.SetPreviewMaterial();
                self.DragOffset = Vector3.zero;
            }
        }

        public static void OnPlacementRotate(this LSViewBuilderComponent self, int rotation)
        {
            if (self.DragPlacement)
            {
                if (self.DragPlacement.placementData.isNew) {
                    self.DragPlacement.Rotation(1);
                }
            }
        }

        public static void OnPlacementCancel(this LSViewBuilderComponent self)
        {
            if (self.DragPlacement)
            {
                self.DragPlacement.ResetPreviewMaterial();
                if (self.DragPlacement.placementData.isNew) {
                    self.DragPlacement.Remove();
                } else {
                    LSUnitView lsUnitView = (LSUnitView)self.DragUnitView;
                    lsUnitView?.GetComponent<LSViewTransformComponent>().SetTransformEnabled(true);
                }
                self.DragPlacement = null;
                self.DragUnitView = null;
            }
        }

    }
}