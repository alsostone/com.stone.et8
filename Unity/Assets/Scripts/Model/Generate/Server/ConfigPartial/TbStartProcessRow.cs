using System.Net;

namespace ET
{
    public partial class TbStartProcessRow
    {
        public string InnerIP => this.TbStartMachineRow.InnerIP;

        public string OuterIP => this.TbStartMachineRow.OuterIP;
        
        // 内网地址外网端口，通过防火墙映射端口过来
        private IPEndPoint ipEndPoint;

        public IPEndPoint IPEndPoint
        {
            get
            {
                if (ipEndPoint == null)
                {
                    this.ipEndPoint = NetworkHelper.ToIPEndPoint(this.InnerIP, this.Port);
                }

                return this.ipEndPoint;
            }
        }

        public TbStartMachineRow TbStartMachineRow => TbStartMachine.Instance.Get(this.MachineId);

        partial void PostInit()
        {
        }
    }
}