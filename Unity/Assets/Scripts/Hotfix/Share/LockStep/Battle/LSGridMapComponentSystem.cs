using System.Collections.Generic;
using MemoryPack;
using ST.GridBuilder;
using TrueSync;

namespace ET
{
    [LSEntitySystemOf(typeof(LSGridMapComponent))]
    [EntitySystemOf(typeof(LSGridMapComponent))]
    [FriendOf(typeof(LSGridMapComponent))]
    public static partial class LSGridMapComponentSystem
    {
        [EntitySystem]
        private static void Awake(this LSGridMapComponent self, string gridName)
        {self.LSRoom()?.ProcessLog.LogFunction(15, self.LSParent().Id);
            self.GridName = gridName;
            
            self.FlowFields = new List<FlowFieldNode[]>();
            self.FreeFlowField = new Stack<int>();
            self.FlowFieldIndexRef = new Dictionary<int, int>();

            self.FlowFieldDefaultIndex = -1;
            self.FlowFieldDirty = false;
            self.FlowFieldDestination = new FieldV2(0, 0);
                    
            self.ResetGridData(FileComponent.Instance.Load(gridName));
            self.PathPoints = new List<IndexV2>();
            self.FlowFieldDestinations = new List<FieldV2>();
        }
        
        [EntitySystem]
        private static void Deserialize(this LSGridMapComponent self)
        {
            // 由于GridData是一个大数据结构，序列化会消耗较多时间且序列化后内存占用也大
            // 优化：由于对于格子数量来说，放入的数量是很小的，反序列化时再放入一遍消耗也不大，所以这里需要一个初始的取地图数据
            // 可以再优化，比如将GridData池化，有瓶颈时再说
            self.ResetGridData(FileComponent.Instance.Load(self.GridName));
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this LSGridMapComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(14, self.LSParent().Id);
            if (self.FlowFieldDirty) {
                self.FlowFieldDirty = false;
                self.ReleaseFlowField(self.FlowFieldDefaultIndex);
                self.FlowFieldDefaultIndex = self.GenerateFlowField(self.FlowFieldDestination);
                EventSystem.Instance.Publish(self.LSWorld(), new LSGridDataReset() { GridData = self.GridData, FlowField = self.GetDefaultFlowField() });
            }
        }

        private static void ResetGridData(this LSGridMapComponent self, byte[] gridBytes)
        {self.LSRoom()?.ProcessLog.LogFunction(13, self.LSParent().Id);
            GridMapData gridMapData = MemoryPackSerializer.Deserialize<GridMapData>(gridBytes);
            self.GridPosition = gridMapData.position;
            self.GridRotation = TSQuaternion.Euler(gridMapData.rotation);
            self.GridData = gridMapData.gridData;
            self.SetDestination(new TSVector(0, 0, 0));
            self.SetObstaclesFromGridData();
            EventSystem.Instance.Publish(self.LSWorld(), new LSGridDataReset() { GridData = self.GridData, FlowField = self.GetDefaultFlowField() });
        }
        
        public static GridData GetGridData(this LSGridMapComponent self)
        {
            return self.GridData;
        }
        
        public static IndexV2 ConvertToIndex(this LSGridMapComponent self, TSVector position)
        {self.LSRoom()?.ProcessLog.LogFunction(12, self.LSParent().Id);
            position = TSQuaternion.Inverse(self.GridRotation) * (position - self.GridPosition);
            return self.GridData.ConvertToIndex(new FieldV2(position.x, position.z));
        }
        
        private static void SetDestination(this LSGridMapComponent self, TSVector position)
        {self.LSRoom()?.ProcessLog.LogFunction(11, self.LSParent().Id);
            position = TSQuaternion.Inverse(self.GridRotation) * (position - self.GridPosition);
            self.FlowFieldDestination = new FieldV2(position.x, position.z);
            self.FlowFieldDirty = true;
        }
        
        private static void SetObstaclesFromGridData(this LSGridMapComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(10, self.LSParent().Id);
            LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
            
            List<int> edges = ObjectPool.Instance.Fetch<List<int>>();
            List<IndexV2> contours = ObjectPool.Instance.Fetch<List<IndexV2>>();
            List<TSVector2> vertices = ObjectPool.Instance.Fetch<List<TSVector2>>();

            self.GridData.GetObstaclesFirstEdge(edges);
            for (int index = 0; index < edges.Count; index++)
            {
                Utils.GenLocalContours(self.GridData.cells, self.GridData.xLength, self.GridData.zLength, edges[index], contours);
                foreach (IndexV2 v2 in contours)
                {
                    FP x = v2.x * self.GridData.cellSize;
                    FP z = v2.z * self.GridData.cellSize;
                    TSVector position = self.GridPosition + self.GridRotation * new TSVector(x, 0, z);
                    vertices.Add(new TSVector2(position.x, position.z));
                }
    
                rvo2Component.AddObstacle(-(index + 100), vertices);
                vertices.Clear();
                contours.Clear();
            }

            ObjectPool.Instance.Recycle(vertices);
            ObjectPool.Instance.Recycle(contours);
            edges.Clear();
            ObjectPool.Instance.Recycle(edges);
        }
        
        public static int GenerateFlowField(this LSGridMapComponent self, List<TSVector> positions)
        {self.LSRoom()?.ProcessLog.LogFunction(9, self.LSParent().Id);
            self.FlowFieldDestinations.Clear();
            foreach (TSVector position in positions) {
                TSVector localPos = TSQuaternion.Inverse(self.GridRotation) * (position - self.GridPosition);
                self.FlowFieldDestinations.Add(new FieldV2(localPos.x, localPos.z));
            }
            
            FlowFieldNode[] flowField;
            if (self.FreeFlowField.TryPop(out int index)) {
                flowField = self.FlowFields[index];
            } else {
                flowField = new FlowFieldNode[self.GridData.xLength * self.GridData.zLength];
                self.FlowFields.Add(flowField);
                index = self.FlowFields.Count - 1;
            }
            
            self.GridData.ResetDijkstraData(flowField, self.FlowFieldDestinations);
            self.GridData.GenerateDijkstraData(flowField);
            self.FlowFieldIndexRef.Add(index, positions.Count);
            
            EventSystem.Instance.Publish(self.LSWorld(), new LSGridDataReset() { GridData = self.GridData, FlowField = self.GetFlowField(index) });
            return index;
        }
        
        public static void RemoveFlowFieldReference(this LSGridMapComponent self, int flowFieldIndex)
        {self.LSRoom()?.ProcessLog.LogFunction(8, self.LSParent().Id, flowFieldIndex);
            if (self.FlowFieldIndexRef.TryGetValue(flowFieldIndex, out int referenceCount))
            {
                if (referenceCount == 1)
                {
                    self.ReleaseFlowField(flowFieldIndex);
                    self.FlowFieldIndexRef.Remove(flowFieldIndex);
                }
                else
                {
                    self.FlowFieldIndexRef[flowFieldIndex] = referenceCount - 1;
                }
            }
        }
        
        public static TSVector GetFieldVector(this LSGridMapComponent self, int flowFieldIndex, TSVector position)
        {
            FlowFieldNode[] flowField = self.GetFlowField(flowFieldIndex);
            if (flowField == null) {
                return new TSVector(0, 0, 0);
            }
            position = TSQuaternion.Inverse(self.GridRotation) * (position - self.GridPosition);
            FieldV2 v2 = self.GridData.GetFieldVector(flowField, new FieldV2(position.x, position.z));
            return self.GridRotation * new TSVector(v2.x, 0, v2.z);
        }
        
        public static bool Pathfinding(this LSGridMapComponent self, TSVector start, TSVector to, List<TSVector> results)
        {self.LSRoom()?.ProcessLog.LogFunction(7, self.LSParent().Id);
            results.Clear();
            
            start = TSQuaternion.Inverse(self.GridRotation) * (start - self.GridPosition);
            to = TSQuaternion.Inverse(self.GridRotation) * (to - self.GridPosition);
            
            // 寻路成功后，将路径点转换为世界坐标
            if (self.GridData.Pathfinding(new FieldV2(start.x, start.z), new FieldV2(to.x, to.z), self.PathPoints))
            {
                int size = self.GridData.cellSize;
                foreach (IndexV2 indexV2 in self.PathPoints)
                {
                    FP x = (indexV2.x + FP.Half) * size;
                    FP z = (indexV2.z + FP.Half) * size;
                    TSVector pos = self.GridPosition + self.GridRotation * new TSVector(x, start.y, z);
                    results.Add(pos);
                }
                return true;
            }
            
            return false;
        }

        // 限制位置在网格范围内
        // 使用时机：为确保移动到目标点，寻路完成后把最后一个点替换成目标点（此时需要限制目标点在网格内，不然就走出界外了）
        public static TSVector ClampPosition(this LSGridMapComponent self, TSVector position)
        {self.LSRoom()?.ProcessLog.LogFunction(6, self.LSParent().Id);
            FP offset = self.GridData.cellSize * FP.Half;    // 偏移0.5能有效避免在边缘的抖动
            position = TSQuaternion.Inverse(self.GridRotation) * (position - self.GridPosition);
            position.x = TSMath.Clamp(position.x, offset, self.GridData.cellSize * self.GridData.xLength - offset);
            position.z = TSMath.Clamp(position.z, offset, self.GridData.cellSize * self.GridData.zLength - offset);
            return self.GridPosition + self.GridRotation * position;
        }

        public static TSVector GetPutPosition(this LSGridMapComponent self, PlacementData placementData)
        {
            int level = self.GridData.GetPointLevelCount(placementData.x, placementData.z, placementData);

            int size = self.GridData.cellSize;
            FP x = (placementData.x + FP.Half) * size;
            FP y = level * size;
            FP z = (placementData.z + FP.Half) * size;
            TSVector pos = self.GridPosition + self.GridRotation * new TSVector(x, y, z);
            return pos;
        }

        private static void GenPlacementBoundary(this LSGridMapComponent self, PlacementData placementData, List<TSVector2> vertices)
        {self.LSRoom()?.ProcessLog.LogFunction(5, self.LSParent().Id);
            List<IndexV2> contours = ObjectPool.Instance.Fetch<List<IndexV2>>();
            Utils.GenLocalContours(placementData.points, PlacementData.width, PlacementData.height, placementData.GetFirstEdge(), contours);
            
            vertices.Clear();
            foreach (IndexV2 v2 in contours)
            {
                FP x = (placementData.x + v2.x - PlacementData.xOffset) * self.GridData.cellSize;
                FP z = (placementData.z + v2.z - PlacementData.zOffset) * self.GridData.cellSize;
                TSVector position = self.GridPosition + self.GridRotation * new TSVector(x, 0, z);
                vertices.Add(new TSVector2(position.x, position.z));
            }

            contours.Clear();
            ObjectPool.Instance.Recycle(contours);
        }
        
        public static bool CanPut(this LSGridMapComponent self, int x, int z, PlacementData placementData)
        {
            return self.GridData.CanPut(x, z, placementData);
        }
        
        public static void Put(this LSGridMapComponent self, int x, int z, PlacementData placementData)
        {self.LSRoom()?.ProcessLog.LogFunction(4, self.LSParent().Id, x, z);
            self.GridData.Put(x, z, placementData);
            self.FlowFieldDirty = true;
            
            switch (placementData.placementType)
            {
                case PlacedLayer.Block:
                {
                    LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
                    List<TSVector2> vertices = ObjectPool.Instance.Fetch<List<TSVector2>>();
                    self.GenPlacementBoundary(placementData, vertices);
                    rvo2Component.AddObstacle(self.LSUnit(placementData.id), vertices);
                    vertices.Clear();
                    ObjectPool.Instance.Recycle(vertices);
                    break;
                }
                case PlacedLayer.Building:
                {
                    LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
                    rvo2Component.AddStaticAgent(self.LSUnit(placementData.id));
                    break;
                }
            }
        }

        public static bool CanTake(this LSGridMapComponent self, PlacementData placementData)
        {
            return self.GridData.CanTake(placementData);
        }
        
        public static void Take(this LSGridMapComponent self, PlacementData placementData)
        {self.LSRoom()?.ProcessLog.LogFunction(3, self.LSParent().Id);
            self.GridData.Take(placementData);
            self.FlowFieldDirty = true;
            
            switch (placementData.placementType)
            {
                case PlacedLayer.Block: {
                    LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
                    rvo2Component.RemoveObstacle(self.LSUnit(placementData.id));
                    break;
                }
                case PlacedLayer.Building: {
                    LSRVO2Component rvo2Component = self.LSWorld().GetComponent<LSRVO2Component>();
                    rvo2Component.RemoveAgent(self.LSUnit(placementData.id));
                    break;
                }
            }
        }
        
        public static FlowFieldNode[] GetDefaultFlowField(this LSGridMapComponent self)
        {
            return self.GetFlowField(self.FlowFieldDefaultIndex);
        }
        
        private static FlowFieldNode[] GetFlowField(this LSGridMapComponent self, int index)
        {
            if (index < 0 || index >= self.FlowFields.Count)
                return null;
            return self.FlowFields[index];
        }

        private static void ReleaseFlowField(this LSGridMapComponent self, int index)
        {self.LSRoom()?.ProcessLog.LogFunction(2, self.LSParent().Id, index);
            if (index < 0 || index >= self.FlowFields.Count)
                return;
            self.FreeFlowField.Push(index);
        }
        
        private static int GenerateFlowField(this LSGridMapComponent self, FieldV2 destination)
        {self.LSRoom()?.ProcessLog.LogFunction(1, self.LSParent().Id);
            FlowFieldNode[] flowField;
            if (self.FreeFlowField.TryPop(out int index)) {
                flowField = self.FlowFields[index];
            } else {
                flowField = new FlowFieldNode[self.GridData.xLength * self.GridData.zLength];
                self.FlowFields.Add(flowField);
                index = self.FlowFields.Count - 1;
            }
            
            self.GridData.ResetDijkstraData(flowField, destination);
            self.GridData.GenerateDijkstraData(flowField);
            return index;
        }

    }
}