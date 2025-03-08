using System;
using System.Collections;
using System.Diagnostics;

namespace ET.Server
{
    public static partial class WatcherHelper
    {
        public static TbStartMachineRow GetThisMachineConfig()
        {
            string[] localIP = NetworkHelper.GetAddressIPs();
            TbStartMachineRow tbStartMachineRow = null;
            foreach (TbStartMachineRow config in TbStartMachine.Instance.DataList)
            {
                if (!WatcherHelper.IsThisMachine(config.InnerIP, localIP))
                {
                    continue;
                }
                tbStartMachineRow = config;
                break;
            }

            if (tbStartMachineRow == null)
            {
                throw new Exception("not found this machine ip config!");
            }

            return tbStartMachineRow;
        }
        
        public static bool IsThisMachine(string ip, string[] localIPs)
        {
            if (ip != "127.0.0.1" && ip != "0.0.0.0" && !((IList) localIPs).Contains(ip))
            {
                return false;
            }
            return true;
        }
        
        public static System.Diagnostics.Process StartProcess(int processId, int createScenes = 0)
        {
            TbStartProcessRow tbStartProcessRow = TbStartProcess.Instance.Get(processId);
            const string exe = "dotnet";
            string arguments = $"App.dll" + 
                    $" --Process={tbStartProcessRow.Id}" +
                    $" --AppType=Server" +  
                    $" --StartConfig={Options.Instance.StartConfig}" +
                    $" --Develop={Options.Instance.Develop}" +
                    $" --LogLevel={Options.Instance.LogLevel}" +
                    $" --Console={Options.Instance.Console}";
            Log.Debug($"{exe} {arguments}");
            System.Diagnostics.Process process = ProcessHelper.Run(exe, arguments);
            return process;
        }
    }
}