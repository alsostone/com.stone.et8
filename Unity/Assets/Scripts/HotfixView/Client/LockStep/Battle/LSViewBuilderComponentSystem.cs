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
            if (self.DragPlacement != null)
            {
                self.DragPlacement.Remove();
                self.DragPlacement = null;
            }
        }

        public static void OnPlacementDragStart(this LSViewBuilderComponent self, long targetId)
        {
            if (!self.DragPlacement)
            {
                LSUnitViewComponent unitViewComponent = self.Room().GetComponent<LSUnitViewComponent>();
                LSUnitView unitView = unitViewComponent.GetChild<LSUnitView>(targetId);
                Placement placement = unitView?.GetComponent<LSViewPlacementComponent>()?.Placement;
                if (placement == null)
                    return;
                
                LSCameraComponent lsCameraComponent = self.Room().GetComponent<LSCameraComponent>();
                GridMap gridMap = lsCameraComponent.GetGridMap();
                if (gridMap.gridData.CanTake(placement.placementData))
                {
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
                LSCameraComponent lsCameraComponent = self.Room().GetComponent<LSCameraComponent>();
                GridMap gridMap = lsCameraComponent.GetGridMap();
                GridMapIndicator gridMapIndicator = lsCameraComponent.GetGridMapIndicator();
                
                Vector3 pos = new(position.x.AsFloat(), 0, position.y.AsFloat());
                if (Math.Abs(self.DragOffset.y - float.MaxValue) < float.Epsilon) {
                    self.DragOffset = self.DragPlacement.GetPosition() - pos;
                }
                
                IndexV2 index = gridMap.ConvertToIndex(pos + self.DragOffset);
                int targetLevel = gridMap.gridData.GetShapeLevelCount(index.x, index.z, self.DragPlacement.placementData);
                self.DragPlacement.SetMovePosition(gridMap.GetLevelPosition(index.x, index.z, targetLevel));
                
                if (gridMapIndicator) {
                    gridMapIndicator.GenerateIndicator(index.x, index.z, targetLevel, self.DragPlacement.placementData);
                }
            }
        }

        public static void OnPlacementDragEnd(this LSViewBuilderComponent self, TSVector2 position)
        {
            if (self.DragPlacement)
            {
                LSCameraComponent lsCameraComponent = self.Room().GetComponent<LSCameraComponent>();
                GridMap gridMap = lsCameraComponent.GetGridMap();
                GridMapIndicator gridMapIndicator = lsCameraComponent.GetGridMapIndicator();
                
                Vector3 pos = new(position.x.AsFloat(), 0, position.y.AsFloat());
                IndexV2 index = gridMap.ConvertToIndex(pos + self.DragOffset);
                if (gridMap.gridData.CanPut(index.x, index.z, self.DragPlacement.placementData))
                {
                    if (self.DragPlacement.placementData.isNew)
                    {
                        gridMap.gridData.Put(index.x, index.z, self.DragPlacement.placementData);
                        gridMap.gridData.ResetFlowField();
                    }
                    else if (index.x != self.DragPlacement.placementData.x || index.z != self.DragPlacement.placementData.z)
                    {
                        gridMap.gridData.Take(self.DragPlacement.placementData);
                        gridMap.gridData.Put(index.x, index.z, self.DragPlacement.placementData);
                        gridMap.gridData.ResetFlowField();
                    }
                    self.DragPlacement.SetPutPosition(gridMap.GetPutPosition(self.DragPlacement.placementData));
                }
                else {
                    if (self.DragPlacement.placementData.isNew) {
                        self.DragPlacement.Remove();
                    } else {
                        self.DragPlacement.SetPutPosition(gridMap.GetPutPosition(self.DragPlacement.placementData));
                    }
                }

                self.DragPlacement.ResetPreviewMaterial();
                self.DragPlacement = null;
                
                if (gridMapIndicator) {
                    gridMapIndicator.ClearIndicator();
                }
            }
        }

        public static void OnPlacementStart(this LSViewBuilderComponent self, EUnitType type, int tableId, int level)
        {
            if (self.DragPlacement)
            {
                if (self.DragPlacement.placementData.isNew) {
                    self.DragPlacement.Remove();
                } else {
                    GridMap gridMap = self.Room().GetComponent<LSCameraComponent>().GetGridMap();
                    self.DragPlacement.SetPutPosition(gridMap.GetPutPosition(self.DragPlacement.placementData));
                }
            }

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
            if (placement == null) {
                UnityEngine.Object.DestroyImmediate(go);
            } else {
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
                if (self.DragPlacement.placementData.isNew) {
                    self.DragPlacement.Remove();
                } else {
                    GridMap gridMap = self.Room().GetComponent<LSCameraComponent>().GetGridMap();
                    self.DragPlacement.SetPutPosition(gridMap.GetPutPosition(self.DragPlacement.placementData));
                }
                self.DragPlacement = null;
            }
        }

    }
}