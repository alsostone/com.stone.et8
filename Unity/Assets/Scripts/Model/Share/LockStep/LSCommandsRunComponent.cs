using MemoryPack;
using System.Collections.Generic;
using TrueSync;

namespace ET
{
    [ComponentOf(typeof(LSUnit))]
    [MemoryPackable]
    public partial class LSCommandsRunComponent: LSEntity, ILSUpdate, IAwake, ISerializeToEntity
    {
        public TSVector2 MoveAxis { get; set; }
        public List<ulong> Commands { get; set; } = new ();

        public long PlacementTargetId;
        public int PlacementRotation;
        public EUnitType PlacementType;
        public int PlacementTableId;
        public int PlacementLevel;
    }
}