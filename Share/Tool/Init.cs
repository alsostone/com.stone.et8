using System;
using CommandLine;

namespace ET.Server
{
    public class ToolOptions : Singleton<ToolOptions>
    {
        [Option("FuncName", Required = false, Default = "Proto2CS")]
        public string FuncName { get; set; }
    }
    
    internal static class Init
    {
        private static int Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Log.Error(e.ExceptionObject.ToString());
            };
            
            try
            {
                // 命令行参数
                Parser.Default.ParseArguments<ToolOptions>(args)
                    .WithNotParsed(error => throw new Exception($"命令行格式错误! {error}"))
                    .WithParsed((o)=>World.Instance.AddSingleton(o));

                switch (ToolOptions.Instance.FuncName)
                {
                    case "Proto2CS":
                        InnerProto2CS.Proto2CS();
                        Console.WriteLine("proto2cs succeed!");
                        return 0;
                    default:
                        Log.Error($"未找到方法: {ToolOptions.Instance.FuncName}");
                        return 1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return 1;
        }
    }
}