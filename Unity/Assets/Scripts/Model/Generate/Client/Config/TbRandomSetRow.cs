
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
    public sealed partial class TbRandomSetRow : BeanBase
    {
        public TbRandomSetRow(ByteBuf _buf)
        {
            Id = _buf.ReadInt();
            {int __n0 = System.Math.Min(_buf.ReadSize(), _buf.Size);Items = new ItemRandomSet[__n0];for(var __index0 = 0 ; __index0 < __n0 ; __index0++) { ItemRandomSet __e0;__e0 = ItemRandomSet.DeserializeItemRandomSet(_buf); Items[__index0] = __e0;}}
            Xxx = _buf.ReadInt();

            PostInit();
        }

        public static TbRandomSetRow DeserializeTbRandomSetRow(ByteBuf _buf)
        {
            return new TbRandomSetRow(_buf);
        }

        /// <summary>
        /// ID
        /// </summary>
        public readonly int Id;

        public readonly ItemRandomSet[] Items;

        /// <summary>
        /// 占位,合并字段不能最后
        /// </summary>
        public readonly int Xxx;


        public const int __ID__ = 1389919849;
        public override int GetTypeId() => __ID__;

        public  void ResolveRef()
        {
            
            foreach (var _e in Items) { _e?.ResolveRef(); }
            
        }

        public override string ToString()
        {
            return "{ "
            + "Id:" + Id + ","
            + "items:" + Luban.StringUtil.CollectionToString(Items) + ","
            + "xxx:" + Xxx + ","
            + "}";
        }

        partial void PostInit();
    }
}
