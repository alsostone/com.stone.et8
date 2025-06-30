#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ProcessLog.Editor
{
    public class ProcessLogSetting
    {
        //日志DB生成目录
        public static string LogSymbolFile = $"{Application.dataPath}" + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + "process_log_symbol.json";

        /// <summary>
        /// 选择需要自动插入日志代码的文件夹目录
        /// </summary>
        public static string[] SearchPaths =
        {
            "/Scripts/Hotfix/Share/LockStep/Battle"
        };

        //要屏蔽的文件夹路径(路径默认从Assets下开始)
        public static string[] IgnoreFolders =
        {
            "/Scripts/Hotfix/Share/LockStep/Battle/AI/Node",
            "/Scripts/Hotfix/Share/LockStep/Battle/AI/Helper",
            "/Scripts/Hotfix/Share/LockStep/Battle/Utils",
            "/Scripts/Hotfix/Share/LockStep/Battle/Helper",
            "/Scripts/Hotfix/Share/LockStep/Battle/Effect",
        };

        //要屏蔽的方法名称
        public static string[] IgnoreFuncNames =
        {
            "Deserialize",
        };

        //要屏蔽的文件名
        public static string[] IgnoreFiles =
        {
            "TSBezier.cs",
        };

        /// 需要序列化存储的枚举参数
        public static List<string> EnumConfig = new List<string>
        {
        };

        public static bool IgnoreFileName(string fileName)
        {
            foreach (var name in IgnoreFiles) {
                if (name.Equals(fileName)) {
                    return true;
                }
            }
            return false;
        }
        
        public static bool IgnoreFunctionName(string functionName)
        {
            foreach (var name in IgnoreFuncNames) {
                if (name.Equals(functionName)) {
                    return true;
                }
            }
            return false;
        }
    }
}
#endif