
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
    /// 资源配置
    /// </summary>
    [Config]
    public partial class ResourceConfigCategory : Singleton<ResourceConfigCategory>, IConfig
    {
        private readonly Dictionary<int, ResourceConfig> _dataMap;
        private readonly List<ResourceConfig> _dataList;

        public ResourceConfigCategory(ByteBuf _buf)
        {
            _dataMap = new Dictionary<int, ResourceConfig>();
            _dataList = new List<ResourceConfig>();

            for (int n = _buf.ReadSize(); n > 0; --n)
            {
                ResourceConfig _v;
                _v = ResourceConfig.DeserializeResourceConfig(_buf);
                _dataList.Add(_v);
                _dataMap.Add(_v.Id, _v);
            }

            PostInit();
        }

        public Dictionary<int, ResourceConfig> DataMap => _dataMap;
        public List<ResourceConfig> DataList => _dataList;

        public ResourceConfig GetOrDefault(int key) => _dataMap.GetValueOrDefault(key);
        public ResourceConfig Get(int key)
        {
            if (_dataMap.TryGetValue(key,out var v))
            {
                return v;
            }
            ConfigLog.Error(this,key);
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
