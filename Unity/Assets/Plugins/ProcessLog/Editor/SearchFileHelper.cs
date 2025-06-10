#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ProcessLog.Editor
{
    public static class SearchFileHelper
    {
        /// <summary>
        /// 返回所有配置的需要自动插入代码的文件路径
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllSearchFile()
        {
            var result = new List<string>();
            foreach (var path in ProcessLogSetting.SearchPaths) {
                result.AddRange(SearchAllFilesWithSuffixs(Application.dataPath, path, ".cs", ProcessLogSetting.IgnoreFolders));
            }

            return result;
        }
        
        public static List<string> GetAllFile()
        {
            var result = new List<string>();
            result.AddRange(SearchAllFilesWithSuffixs(Application.dataPath, "/Scripts", ".cs"));
            return result;
        }

        /// <summary>
        /// 在指定路径下搜索所有指定后缀的文件(子文件夹下也会被搜索)
        /// 会剔除被屏蔽的文件夹路径
        /// </summary>
        /// <param name="baseDir">参数1:父路径</param>
        /// <param name="searchPath">参数2:搜索目录</param>
        /// <param name="allSuffixs">参数3:要搜索的后缀(以逗号隔开,如.txt,.cs)</param>
        /// <param name="ignoreFolders">参数4:需要剔除的文件夹</param>
        /// <returns>返回值:所有找到的子路径</returns>
        public static List<string> SearchAllFilesWithSuffixs(string baseDir, string searchPath, string allSuffixs, string[] ignoreFolders = null)
        {
            List<string> subPaths = new List<string>();
            
            // 剔除所有屏蔽文件夹
            var ignoreSign = false;
            if (ignoreFolders != null)
            {
                foreach (var ignoreFolder in ignoreFolders) {
                    if (ignoreFolder == searchPath) {
                        ignoreSign = true;
                        break;
                    }
                }
            }

            if (ignoreSign == false) {
                var info = new DirectoryInfo(baseDir + searchPath);
                var subFiles = info.GetFiles();
                var subDirs = info.GetDirectories();
                
                // 处理子文件
                foreach (FileInfo file in subFiles) {
                    if (!File.Exists(file.FullName))
                        continue;
                    
                    // 提取后缀(带.)
                    var suffixIndex = file.FullName.LastIndexOf('.');
                    if (suffixIndex == -1)
                        continue;
                    
                    // 匹配后缀
                    var suffix = file.FullName.Substring(suffixIndex);
                    foreach (string targetSuffix in allSuffixs.Split(',')) {
                        if (suffix == targetSuffix) {
                            string subPath = file.FullName.Substring(baseDir.Length, file.FullName.Length - baseDir.Length);
                            subPaths.Add(subPath);
                            break;
                        }
                    }
                }

                // 处理子目录
                foreach (var dir in subDirs) {
                    // 直接输入子文件路径会进入这里导致死循环，需要做一层保护
                    if (Directory.Exists(dir.FullName)) {
                        var subFolder = dir.FullName.Substring(baseDir.Length, dir.FullName.Length - baseDir.Length);
                        subFolder = subFolder.Replace("\\", "/");
                        subPaths.AddRange(SearchAllFilesWithSuffixs(baseDir, subFolder, allSuffixs, ignoreFolders));
                    }
                }
            }

            return subPaths;
        }
    }
}
#endif