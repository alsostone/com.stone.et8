#if UNITY_EDITOR
using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ProcessLog.Editor
{
    public partial class ProcessLog
    {
        private static readonly string RootDir = Application.dataPath;
        
        [MenuItem("ET/ProcessLog/ProcessLog Gen Auto Code", false, 200)]
        private static void GenAutoCodeSymbol()
        {
            var symbolFile = new LogSymbolFile();
            symbolFile.ReadSymbolFile(ProcessLogSetting.LogSymbolFile);
            
            var allSearchFile = SearchFileHelper.GetAllSearchFile();
            for (var i = 0; i < allSearchFile.Count; i++) {
                var filename = allSearchFile[i];
                var path = RootDir + filename;
                EditorUtility.DisplayProgressBar("Generate Auto Code & Symbol", filename, i / (float) allSearchFile.Count);
                ResolveManualLog(path, symbolFile);
                ResolveAutoLog(path, symbolFile);
            }

            symbolFile.CreateSymbolFile(ProcessLogSetting.LogSymbolFile);
            AssetDatabase.Refresh();
            
            ET.Log.Info("ProcessLog Gen Auto Code Done.");
            EditorUtility.ClearProgressBar();
        }

        [MenuItem("ET/ProcessLog/ProcessLog Re Gen Auto Code", false, 201)]
        private static void RegenAutoCodeSymbol()
        {
            // 1 移除所有文件(防止被拷贝到非战斗系统路径)中旧的日志
            var allSearchFile = SearchFileHelper.GetAllFile();
            for (var i = 0; i < allSearchFile.Count; i++) {
                var filename = allSearchFile[i];
                EditorUtility.DisplayProgressBar("Remove Auto Code & Symbol", filename, i / (float) allSearchFile.Count * 2);
                RemoveAutoCode(RootDir + filename);
            }
            LogSymbolFile.RemoveSymbolFile(ProcessLogSetting.LogSymbolFile);

            // 2 生成全新日志
            var symbolFile = new LogSymbolFile();
            symbolFile.ReadSymbolFile(ProcessLogSetting.LogSymbolFile);
            allSearchFile = SearchFileHelper.GetAllSearchFile();
            for (var i = 0; i < allSearchFile.Count; i++) {
                var filename = allSearchFile[i];
                var path = RootDir + filename;
                EditorUtility.DisplayProgressBar("Generate Auto Code & Symbol", filename, i + allSearchFile.Count / (float) allSearchFile.Count * 2);
                ResolveManualLog(path, symbolFile);
                ResolveAutoLog(path, symbolFile);
            }

            symbolFile.CreateSymbolFile(ProcessLogSetting.LogSymbolFile);
            AssetDatabase.Refresh();
            
            ET.Log.Info("ProcessLog Regen Auto Code Done.");
            EditorUtility.ClearProgressBar();
        }

        [MenuItem("ET/ProcessLog/ProcessLog Remove Auto Code", false, 202)]
        private static void RemoveAllAutoCode()
        {
            // 获取所有文件(防止被拷贝到非战斗系统路径)
            var allSearchFile = SearchFileHelper.GetAllFile();
            for (var i = 0; i < allSearchFile.Count; i++) {
                var filename = allSearchFile[i];
                EditorUtility.DisplayProgressBar("Remove Auto Code & Symbol", filename, i / (float) allSearchFile.Count);
                RemoveAutoCode(RootDir + filename);
            }

            LogSymbolFile.RemoveSymbolFile(ProcessLogSetting.LogSymbolFile);
            AssetDatabase.Refresh();
            
            ET.Log.Info("ProcessLog Remove Auto Code Done.");
            EditorUtility.ClearProgressBar();
        }

        [MenuItem("ET/ProcessLog/ProcessLog Check", false, 203)]
        private static void CheckAutoCodeSymbol()
        {
            var symbolFile = new LogSymbolFile();
            symbolFile.ReadSymbolFile(ProcessLogSetting.LogSymbolFile);
            
            ClearCheckProcessLogAll();
            var allSearchFile = SearchFileHelper.GetAllFile();
            for (var i = 0; i < allSearchFile.Count; i++) {
                var filename = allSearchFile[i];
                var path = RootDir + filename;
                EditorUtility.DisplayProgressBar("Check Auto Code & Symbol", filename, i / (float) allSearchFile.Count);
                CheckProcessLog(path, symbolFile);
            }

            ET.Log.Info("ProcessLog Check Done.");
            EditorUtility.ClearProgressBar();
        }
        
        [MenuItem("ET/ProcessLog/ProcessLog Decompress Folders", false, 204)]
        private static void DecompressLogFolders()
        {
            LoadSymbolFile(ProcessLogSetting.LogSymbolFile);
            DecompressLogFile($"{Application.persistentDataPath}/process_log");
            DecompressLogFile($"{Application.persistentDataPath}/process_log_server");
            
            ET.Log.Info("ProcessLog Decompress Log Folder Done.");
            EditorUtility.ClearProgressBar();
            Execute(Application.persistentDataPath);
        }
        
        [MenuItem("ET/ProcessLog/ProcessLog Decompress Select Folder", false, 205)]
        private static void DecompressLogFolder()
        {
            var folder = EditorUtility.OpenFolderPanel("Bin Log Folder Select", Application.persistentDataPath, "");
            
            LoadSymbolFile(ProcessLogSetting.LogSymbolFile);
            DecompressLogFile(folder);
            
            ET.Log.Info("ProcessLog Decompress Log Folder Done.");
            EditorUtility.ClearProgressBar();
            Execute(folder);
        }

        private static void DecompressLogFile(string folder)
        {
            if (!Directory.Exists(folder)) { return; }
            try {
                var allSearchFile = Directory.GetFiles(folder, "*.bin");
                for (var i = 0; i < allSearchFile.Length; i++) {
                    
                    var file = allSearchFile[i];
                    
                    if (file.EndsWith(".bin")) {
                        var binFilename = Path.Combine(folder, file);
                        var filename = Path.GetFileNameWithoutExtension(file);
                        var txtFilename = Path.Combine(folder, filename + ".txt");
                        EditorUtility.DisplayProgressBar("ProcessLog Decompress Log Folder", filename, i / (float) allSearchFile.Length);
                        
                        ET.Log.Info($"Decompress Bin Log File Begin:{filename}");
                        ConvertToTextFile(binFilename, txtFilename);
                        ET.Log.Info($"Decompress Bin Log File Done:{filename}");
                    }
                }
            }
            catch (Exception e) {
                ET.Log.Error(e.ToString());
            }
        }

        private static void Execute(string folder)
        {
            folder = $"\"{folder}\"";
            switch (Application.platform) {
                case RuntimePlatform.WindowsEditor:
                    Process.Start("Explorer.exe", folder.Replace('/', '\\'));
                    break;
                case RuntimePlatform.OSXEditor:
                    Process.Start("open", folder);
                    break;
            }
        }
    }
}
#endif