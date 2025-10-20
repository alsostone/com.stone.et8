using System.Collections.Generic;
using MemoryPack;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class MovePathFindingComponent : LSEntity, IAwake, ILSUpdate, ISerializeToEntity
    {
        public List<TSVector> PathPoints;
        public int CurrentPathIndex = 0;
    }
}