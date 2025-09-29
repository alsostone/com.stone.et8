using System;
using MemoryPack;
using TrueSync;

namespace ST.GridBuilder
{
    [MemoryPackable]
    [Serializable]
    public partial class GridMapData
    {
        [MemoryPackInclude] public TSVector position;
        [MemoryPackInclude] public TSVector rotation;
        [MemoryPackInclude] public GridData gridData;
    }
}
