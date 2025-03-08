using System.Collections.Generic;
using System.Net;

namespace ET
{
    public partial class TbStartScene
    {
        public MultiMap<int, TbStartSceneRow> Gates = new();

        public MultiMap<int, TbStartSceneRow> ProcessScenes = new();

        public Dictionary<long, Dictionary<string, TbStartSceneRow>> ClientScenesByName = new();

        public TbStartSceneRow LocationRow;

        public List<TbStartSceneRow> Realms = new();

        public List<TbStartSceneRow> Routers = new();

        public List<TbStartSceneRow> Maps = new();

        public TbStartSceneRow Match;

        public TbStartSceneRow Benchmark;

        public List<TbStartSceneRow> GetByProcess(int process)
        {
            return this.ProcessScenes[process];
        }

        public TbStartSceneRow GetBySceneName(int zone, string name)
        {
            return this.ClientScenesByName[zone][name];
        }

        partial void PostInit()
        {
            foreach (TbStartSceneRow startSceneConfig in this.DataList)
            {
                this.ProcessScenes.Add(startSceneConfig.Process, startSceneConfig);

                if (!this.ClientScenesByName.ContainsKey(startSceneConfig.Zone))
                {
                    this.ClientScenesByName.Add(startSceneConfig.Zone, new Dictionary<string, TbStartSceneRow>());
                }

                this.ClientScenesByName[startSceneConfig.Zone].Add(startSceneConfig.Name, startSceneConfig);

                switch (startSceneConfig.Type)
                {
                    case SceneType.Realm:
                        this.Realms.Add(startSceneConfig);
                        break;
                    case SceneType.Gate:
                        this.Gates.Add(startSceneConfig.Zone, startSceneConfig);
                        break;
                    case SceneType.Location:
                        this.LocationRow = startSceneConfig;
                        break;
                    case SceneType.Router:
                        this.Routers.Add(startSceneConfig);
                        break;
                    case SceneType.Map:
                        this.Maps.Add(startSceneConfig);
                        break;
                    case SceneType.Match:
                        this.Match = startSceneConfig;
                        break;
                    case SceneType.BenchmarkServer:
                        this.Benchmark = startSceneConfig;
                        break;
                }
            }
        }
    }

    public partial class TbStartSceneRow
    {
        public ActorId ActorId;

        public SceneType Type;

        public TbStartProcessRow TbStartProcessRow
        {
            get
            {
                return TbStartProcess.Instance.Get(this.Process);
            }
        }

        public TbStartZoneRow TbStartZoneRow
        {
            get
            {
                return TbStartZone.Instance.Get(this.Zone);
            }
        }

        // 内网地址外网端口，通过防火墙映射端口过来
        private IPEndPoint innerIPPort;

        public IPEndPoint InnerIPPort
        {
            get
            {
                if (innerIPPort == null)
                {
                    this.innerIPPort = NetworkHelper.ToIPEndPoint($"{this.TbStartProcessRow.InnerIP}:{this.Port}");
                }

                return this.innerIPPort;
            }
        }

        private IPEndPoint outerIPPort;

        // 外网地址外网端口
        public IPEndPoint OuterIPPort
        {
            get
            {
                if (this.outerIPPort == null)
                {
                    this.outerIPPort = NetworkHelper.ToIPEndPoint($"{this.TbStartProcessRow.OuterIP}:{this.Port}");
                }

                return this.outerIPPort;
            }
        }

        partial void PostInit()
        {
            this.ActorId = new ActorId(this.Process, this.Id, 1);
            this.Type    = EnumHelper.FromString<SceneType>(this.SceneType);
        }
    }
}