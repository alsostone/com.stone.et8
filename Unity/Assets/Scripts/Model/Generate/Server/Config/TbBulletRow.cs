
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;

namespace ET
{
    [EnableClass]
    public sealed partial class TbBulletRow : BeanBase
    {
        public TbBulletRow(ByteBuf _buf)
        {
            Id = _buf.ReadInt();
            Name = _buf.ReadString();
            Life = _buf.ReadInt();
            EffectGroupId = _buf.ReadInt();
            TrackId = _buf.ReadInt();
            TrackId_Ref = null;
            FinishEffectId = _buf.ReadInt();

            PostInit();
        }

        public static TbBulletRow DeserializeTbBulletRow(ByteBuf _buf)
        {
            return new TbBulletRow(_buf);
        }

        /// <summary>
        /// 编号
        /// </summary>
        public readonly int Id;

        /// <summary>
        /// 名称
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// 生存时间
        /// </summary>
        public readonly int Life;

        /// <summary>
        /// 效果组ID
        /// </summary>
        public readonly int EffectGroupId;

        /// <summary>
        /// 轨迹ID
        /// </summary>
        public readonly int TrackId;

        public TbTrackRow TrackId_Ref { get; private set; }

        /// <summary>
        /// 结束特效ID
        /// </summary>
        public readonly int FinishEffectId;


        public const int __ID__ = 568834698;
        public override int GetTypeId() => __ID__;

        public  void ResolveRef()
        {
            
            
            
            
            TrackId_Ref = TbTrack.Instance.GetOrDefault(TrackId);
            
        }

        public override string ToString()
        {
            return "{ "
            + "Id:" + Id + ","
            + "name:" + Name + ","
            + "life:" + Life + ","
            + "effectGroupId:" + EffectGroupId + ","
            + "trackId:" + TrackId + ","
            + "finishEffectId:" + FinishEffectId + ","
            + "}";
        }

        partial void PostInit();
    }
}
