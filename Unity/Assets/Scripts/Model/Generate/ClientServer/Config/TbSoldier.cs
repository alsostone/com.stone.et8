
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Luban;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 士兵表
    /// </summary>
    [Config]
    public partial class TbSoldier : Singleton<TbSoldier>, IConfig
    {
        private readonly List<TbSoldierRow> _dataList;

        private Dictionary<(int, int), TbSoldierRow> _dataMapUnion;

        public TbSoldier(ByteBuf _buf)
        {
            _dataList = new List<TbSoldierRow>();

            for(int n = _buf.ReadSize() ; n > 0 ; --n)
            {
                TbSoldierRow _v;
                _v = TbSoldierRow.DeserializeTbSoldierRow(_buf);
                _dataList.Add(_v);
            }
            _dataMapUnion = new Dictionary<(int, int), TbSoldierRow>();
            foreach(var _v in _dataList)
            {
                _dataMapUnion.Add((_v.Id, _v.Level), _v);
            }
        }

        public List<TbSoldierRow> DataList => _dataList;

        public TbSoldierRow GetOrDefault(int Id, int level)
        {
            return _dataMapUnion.GetValueOrDefault((Id, level));
        }
    
        public TbSoldierRow Get(int Id, int level)
        {
            if (_dataMapUnion.TryGetValue((Id, level), out var v))
            {
                return v;
            }
            ConfigLog.Error(this, (Id, level));
            return null;
        }

        public void ResolveRef()
        {
            foreach(var _v in _dataList)
            {
                _v.ResolveRef();
            }
        }

        partial void PostInit();
    }
}
