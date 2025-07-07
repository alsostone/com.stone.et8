using TrueSync;

namespace ET.Client
{
    [EntitySystemOf(typeof(LSGridBuilderComponent))]
    [FriendOf(typeof(LSGridBuilderComponent))]
    [FriendOf(typeof(LSViewPlacementComponent))]
    public static partial class LSViewBuilderComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSGridBuilderComponent self)
        {
            // self.gridMapReference = new WeakReference<GridMap>(gridMap);
            // self.gridMap = UnityEngine.Object.FindObjectOfType<GridMap>();
            // self.gridMapIndicator = UnityEngine.Object.FindObjectOfType<GridMapIndicator>();
        }

        [EntitySystem]
        private static void Destroy(this LSGridBuilderComponent self)
        {
        }

        public static void OnPlacementDragStart(this LSGridBuilderComponent self, long targetId)
        {
            // if (!self.dragPlacement)
            // {
            //     if (self.RaycastTarget(touchPosition, out GameObject target))
            //     {
            //         Placement buiding = target.GetComponent<Placement>();
            //         if (!buiding) {
            //             return false;
            //         }
            //
            //         Vector3 position = buiding.GetPosition();
            //         if (self.gridMap.gridData.CanTake(buiding.placementData))
            //         {
            //             self.dragPlacement = buiding;
            //             self.dragPlacement.SetPreviewMaterial();
            //             self.RaycastTerrain(touchPosition, out Vector3 pos);
            //             self.dragOffset = position - pos;
            //         } else {
            //             buiding.DoShake();
            //         }
            //     }
            // }
        }

        public static void OnPlacementDrag(this LSGridBuilderComponent self, TSVector2 position)
        {
            // if (self.dragPlacement)
            // {
            //     if (self.RaycastTerrain(touchPosition, out Vector3 pos))
            //     {
            //         IndexV2 index = self.gridMap.ConvertToIndex(pos + self.dragOffset);
            //         int targetLevel = self.gridMap.gridData.GetShapeLevelCount(index.x, index.z, self.dragPlacement.placementData);
            //         self.dragPlacement.SetMovePosition(self.gridMap.GetLevelPosition(index.x, index.z, targetLevel));
            //         if (self.gridMapIndicator) {
            //             self.gridMapIndicator.GenerateIndicator(index.x, index.z, targetLevel, self.dragPlacement.placementData);
            //         }
            //     }
            // }
        }

        public static void OnPlacementDragEnd(this LSGridBuilderComponent self, TSVector2 position)
        {
            // if (self.dragPlacement)
            // {
            //     self.dragPlacement.ResetPreviewMaterial();
            //     if (self.RaycastTerrain(touchPosition, out Vector3 pos))
            //     {
            //         IndexV2 index = self.gridMap.ConvertToIndex(pos + self.dragOffset);
            //         if (self.gridMap.gridData.CanPut(index.x, index.z, self.dragPlacement.placementData))
            //         {
            //             if (self.isNewBuilding)
            //             {
            //                 self.dragPlacement.placementData.id = self.gridMap.gridData.GetNextGuid();
            //                 self.gridMap.gridData.Put(index.x, index.z, self.dragPlacement.placementData);
            //                 self.gridMap.gridData.ResetFlowField();
            //             }
            //             else if (index.x != self.dragPlacement.placementData.x || index.z != self.dragPlacement.placementData.z)
            //             {
            //                 self.gridMap.gridData.Take(self.dragPlacement.placementData);
            //                 self.gridMap.gridData.Put(index.x, index.z, self.dragPlacement.placementData);
            //                 self.gridMap.gridData.ResetFlowField();
            //             }
            //             self.dragPlacement.SetPutPosition(self.gridMap.GetPutPosition(self.dragPlacement.placementData));
            //         }
            //         else {
            //             if (self.isNewBuilding) {
            //                 self.dragPlacement.Remove();
            //             } else {
            //                 self.dragPlacement.SetPutPosition(self.gridMap.GetPutPosition(self.dragPlacement.placementData));
            //             }
            //         }
            //     } else {
            //         if (self.isNewBuilding) {
            //             self.dragPlacement.Remove();
            //         } else {
            //             self.dragPlacement.SetPutPosition(self.gridMap.GetPutPosition(self.dragPlacement.placementData));
            //         }
            //     }
            //     self.dragPlacement = null;
            //     self.isNewBuilding = false;
            //     self.dragOffset = Vector3.zero;
            //     if (self.gridMapIndicator) {
            //         self.gridMapIndicator.ClearIndicator();
            //     }
            // }
        }

        public static void OnPlacementStart(this LSGridBuilderComponent self, int tableId)
        {
            // if (self.dragPlacement)
            // {
            //     if (self.isNewBuilding) {
            //         self.dragPlacement.Remove();
            //     } else {
            //         self.dragPlacement.SetPutPosition(self.gridMap.GetPutPosition(self.dragPlacement.placementData));
            //     }
            // }
            // if (placement) {
            //     placement.Reset();
            //     self.dragPlacement = placement;
            //     self.dragPlacement.SetPreviewMaterial();
            //     self.isNewBuilding = true;
            //     self.dragOffset = Vector3.zero;
            // }
        }

        public static void OnPlacementRotate(this LSGridBuilderComponent self, int rotation)
        {
            // if (self.dragPlacement)
            // {
            //     if (self.isNewBuilding) {
            //         self.dragPlacement.Rotation(1);
            //     }
            // }
        }

        public static void OnPlacementCancel(this LSGridBuilderComponent self)
        {
            // if (self.dragPlacement)
            // {
            //     if (self.isNewBuilding) {
            //         self.dragPlacement.Remove();
            //     } else {
            //         self.dragPlacement.SetPutPosition(self.gridMap.GetPutPosition(self.dragPlacement.placementData));
            //     }
            //     self.dragPlacement = null;
            //     self.isNewBuilding = false;
            //     self.dragOffset = Vector3.zero;
            // }
        }

    }
}