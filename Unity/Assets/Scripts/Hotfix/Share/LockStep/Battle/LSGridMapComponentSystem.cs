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
            byte[] gridBytes = FileComponent.Instance.Load(gridName);
            self.GridData = MemoryPackSerializer.Deserialize<GridData>(gridBytes);
            self.GridData.SetDestination(new FieldV2(0, 0));
        }
        
        [EntitySystem]
        private static void Deserialize(this LSGridMapComponent self)
        {
            // 由于GridData是一个大数据结构，序列化会消耗较多时间且序列化后内存占用也大
            // 优化：由于对于格子数量来说，放入的数量是很小的，反序列化时再放入一遍消耗也不大，所以这里需要一个初始的取地图数据
            // 可以再优化，比如将GridData池化，有瓶颈时再说
            byte[] gridBytes = FileComponent.Instance.Load(self.GridName);
            self.GridData = MemoryPackSerializer.Deserialize<GridData>(gridBytes);
            self.GridData.SetDestination(new FieldV2(0, 0));
        }
        
        [LSEntitySystem]
        private static void LSUpdate(this LSGridMapComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(81, self.LSParent().Id);
            // 每帧更新时重置流场 有脏标记
            self.GridData.ResetFlowField();
        }
        
        public static GridData GetGridData(this LSGridMapComponent self)
        {self.LSRoom()?.ProcessLog.LogFunction(80, self.LSParent().Id);
            return self.GridData;
        }
        
        public static IndexV2 ConvertToIndex(this LSGridMapComponent self, TSVector2 position)
        {self.LSRoom()?.ProcessLog.LogFunction(79, self.LSParent().Id);
            FieldV2 v2 = new FieldV2(position.x, position.y);
            return self.GridData.ConvertToIndex(ref v2);
        }

        public static TSVector GetPutPosition(this LSGridMapComponent self, PlacementData placementData)
        {self.LSRoom()?.ProcessLog.LogFunction(78, self.LSParent().Id);
            int level = self.GridData.GetPointLevelCount(placementData.x, placementData.z, placementData);
            FieldV2 v2 = self.GridData.GetCellPosition(placementData.x, placementData.z);
            return new TSVector(v2.x, level * self.GridData.cellSize, v2.z);
        }

        // 只允许反序列化时调用，战斗逻辑中应使用TryPut
        public static void Put(this LSGridMapComponent self, int x, int z, PlacementData placementData)
        {self.LSRoom()?.ProcessLog.LogFunction(77, self.LSParent().Id, x, z);
            self.GridData.Put(x, z, placementData);
        }
    }
}