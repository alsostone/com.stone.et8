using MemoryPack;

namespace ET
{
    [ComponentOf(typeof(LSWorld))]
    [MemoryPackable]
    public partial class LSGameOverComponent: LSEntity, IAwake<TbStageRow>, ILSUpdate, ISerializeToEntity
    {
        public TeamType WinTeam;
    }
}