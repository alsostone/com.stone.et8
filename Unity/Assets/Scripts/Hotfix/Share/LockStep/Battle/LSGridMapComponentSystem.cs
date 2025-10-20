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
            self.ResetGridData(FileComponent.Instance.Load(gridName));
            self.PathPoints = new List<IndexV2>();
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
            // 每帧更新时重置流场 有脏标记
            self.GridData.ResetFlowField();
        }

        private static void ResetGridData(this LSGridMapComponent self, byte[] gridBytes)
        {self.LSRoom()?.ProcessLog.LogFunction(90, self.LSParent().Id);
            GridMapData gridMapData = MemoryPackSerializer.Deserialize<GridMapData>(gridBytes);
            self.GridPosition = gridMapData.position;
            self.GridRotation = TSQuaternion.Euler(gridMapData.rotation);
            self.GridData = gridMapData.gridData;
            self.SetDestination(new TSVector(0, 0, 0));
            EventSystem.Instance.Publish(self.LSWorld(), new LSGridDataReset() { GridData = self.GridData });
        }
        
        public static GridData GetGridData(this LSGridMapComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(80, self.LSParent().Id);
            return self.GridData;
        }
        
        public static IndexV2 ConvertToIndex(this LSGridMapComponent self, TSVector position)
        {self.LSRoom()?.ProcessLog.LogFunction(79, self.LSParent().Id);
            position = TSQuaternion.Inverse(self.GridRotation) * (position - self.GridPosition);
            return self.GridData.ConvertToIndex(new FieldV2(position.x, position.z));
        }
        
        public static void SetDestination(this LSGridMapComponent self, TSVector position)
        {
            position = TSQuaternion.Inverse(self.GridRotation) * (position - self.GridPosition);
            self.GridData.SetDestination(new FieldV2(position.x, position.z));
        }
        
        public static TSVector GetFieldVector(this LSGridMapComponent self, TSVector position)
        {
            position = TSQuaternion.Inverse(self.GridRotation) * (position - self.GridPosition);
            FieldV2 v2 = self.GridData.GetFieldVector(new FieldV2(position.x, position.z));
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

        // 只允许反序列化时调用，战斗逻辑中应使用TryPut
        public static void Put(this LSGridMapComponent self, int x, int z, PlacementData placementData)
        {self.LSRoom()?.ProcessLog.LogFunction(77, self.LSParent().Id, x, z);
            self.GridData.Put(x, z, placementData);
        }
    }
}