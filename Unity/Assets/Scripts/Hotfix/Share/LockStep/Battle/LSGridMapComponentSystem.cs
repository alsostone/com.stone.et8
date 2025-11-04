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
        {self.LSRoom()?.ProcessLog.LogFunction(82, self.LSParent().Id);
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
        {self.LSRoom()?.ProcessLog.LogFunction(81, self.LSParent().Id);
            if (self.FlowFieldDirty) {
                self.FlowFieldDirty = false;
                self.ReleaseFlowField(self.FlowFieldDefaultIndex);
                self.FlowFieldDefaultIndex = self.GenerateFlowField(self.FlowFieldDestination);
                EventSystem.Instance.Publish(self.LSWorld(), new LSGridDataReset() { GridData = self.GridData, FlowField = self.GetDefaultFlowField() });
            }
        }

        private static void ResetGridData(this LSGridMapComponent self, byte[] gridBytes)
        {self.LSRoom()?.ProcessLog.LogFunction(90, self.LSParent().Id);
            GridMapData gridMapData = MemoryPackSerializer.Deserialize<GridMapData>(gridBytes);
            self.GridPosition = gridMapData.position;
            self.GridRotation = TSQuaternion.Euler(gridMapData.rotation);
            self.GridData = gridMapData.gridData;
            self.SetDestination(new TSVector(0, 0, 0));
            EventSystem.Instance.Publish(self.LSWorld(), new LSGridDataReset() { GridData = self.GridData, FlowField = self.GetDefaultFlowField() });
        }
        
        public static GridData GetGridData(this LSGridMapComponent self)
        {
            return self.GridData;
        }
        
        public static IndexV2 ConvertToIndex(this LSGridMapComponent self, TSVector position)
        {self.LSRoom()?.ProcessLog.LogFunction(79, self.LSParent().Id);
            position = TSQuaternion.Inverse(self.GridRotation) * (position - self.GridPosition);
            return self.GridData.ConvertToIndex(new FieldV2(position.x, position.z));
        }
        
        private static void SetDestination(this LSGridMapComponent self, TSVector position)
        {
            position = TSQuaternion.Inverse(self.GridRotation) * (position - self.GridPosition);
            self.FlowFieldDestination = new FieldV2(position.x, position.z);
            self.FlowFieldDirty = true;
        }
        
        public static int GenerateFlowField(this LSGridMapComponent self, List<TSVector> positions)
        {
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
        {
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
        {
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
        {
            FP offset = self.GridData.cellSize * FP.Half;    // 偏移0.5能有效避免在边缘的抖动
            position = TSQuaternion.Inverse(self.GridRotation) * (position - self.GridPosition);
            position.x = TSMath.Clamp(position.x, offset, self.GridData.cellSize * self.GridData.xLength - offset);
            position.z = TSMath.Clamp(position.z, offset, self.GridData.cellSize * self.GridData.zLength - offset);
            return self.GridPosition + self.GridRotation * position;
        }

        public static TSVector GetPutPosition(this LSGridMapComponent self, PlacementData placementData)
        {self.LSRoom()?.ProcessLog.LogFunction(78, self.LSParent().Id);
            int level = self.GridData.GetPointLevelCount(placementData.x, placementData.z, placementData);

            int size = self.GridData.cellSize;
            FP x = (placementData.x + FP.Half) * size;
            FP y = level * size;
            FP z = (placementData.z + FP.Half) * size;
            TSVector pos = self.GridPosition + self.GridRotation * new TSVector(x, y, z);
            return pos;
        }
        
        public static bool CanPut(this LSGridMapComponent self, int x, int z, PlacementData placementData)
        {
            return self.GridData.CanPut(x, z, placementData);
        }
        
        public static void Put(this LSGridMapComponent self, int x, int z, PlacementData placementData)
        {self.LSRoom()?.ProcessLog.LogFunction(77, self.LSParent().Id, x, z);
            self.GridData.Put(x, z, placementData);
            self.FlowFieldDirty = true;
        }

        public static bool CanTake(this LSGridMapComponent self, PlacementData placementData)
        {
            return self.GridData.CanTake(placementData);
        }
        
        public static void Take(this LSGridMapComponent self, PlacementData placementData)
        {
            self.GridData.Take(placementData);
            self.FlowFieldDirty = true;
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
        {
            if (index < 0 || index >= self.FlowFields.Count)
                return;
            self.FreeFlowField.Push(index);
        }
        
        private static int GenerateFlowField(this LSGridMapComponent self, FieldV2 destination)
        {
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