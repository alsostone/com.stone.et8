using System;

namespace ET
{
    public static class ToolsEditor
    {
        public static void ExcelExporter(CodeMode codeMode)
        {
#if UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
            switch (codeMode)
            {
                case CodeMode.Client:
                    ShellHelper.Run("sh export_excel_client.sh", "../Tools/Luban");
                    break;
                case CodeMode.Server:
                    ShellHelper.Run("sh export_excel_server.sh", "../Tools/Luban");
                    break;
                case CodeMode.ClientServer:
                    ShellHelper.Run("sh export_excel_client_server.sh", "../Tools/Luban");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(codeMode), codeMode, null);
            }
#else
            switch (codeMode)
            {
                case CodeMode.Client:
                    ShellHelper.Run("export_excel_client.bat", "../Tools/Luban");
                    break;
                case CodeMode.Server:
                    ShellHelper.Run("export_excel_server.bat", "../Tools/Luban");
                    break;
                case CodeMode.ClientServer:
                    ShellHelper.Run("export_excel_client_server.bat", "../Tools/Luban");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(codeMode), codeMode, null);
            }
#endif
        }

        public static void ExcelAllExporter()
        {
#if UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
            ShellHelper.Run("sh export_excel_client.sh", "../Tools/Luban");
            ShellHelper.Run("sh export_excel_server.sh", "../Tools/Luban");
            ShellHelper.Run("sh export_excel_client_server.sh", "../Tools/Luban");
#else
            ShellHelper.Run("export_excel_client.bat", "../Tools/Luban");
            ShellHelper.Run("export_excel_server.bat", "../Tools/Luban");
            ShellHelper.Run("export_excel_client_server.bat", "../Tools/Luban");
#endif
        }
        
        public static void Proto2CS()
        {
#if UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
            const string tools = "./Tool";
#else
            const string tools = ".\\Tool.exe";
#endif
            ShellHelper.Run($"{tools} --AppType=Proto2CS --Console=1", "../Bin/");
        }
    }
}