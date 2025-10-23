using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class FindReferences
{
    private const string MetaExtension = ".meta";

    [MenuItem("Assets/Find References %#&f", false, 10)]
    public static void Find()
    {
        var isFolder = false;
        if (Selection.objects.Length > 0) {
            var path = AssetDatabase.GetAssetPath(Selection.objects[0]);
            isFolder = AssetDatabase.IsValidFolder(path);
        }
        else
        {
            isFolder = true;
        }
        
        if (isFolder)
        {
            // 目录
            var gameObjects = Selection.GetFiltered<Object>(SelectionMode.DeepAssets);
            foreach (var go in gameObjects) {
                FindOne(go);
            }
        }
        else
        {
            // 单选or多选文件
            foreach (var item in Selection.objects) {
                FindOne(item);
            }
        }
    }

    private static void FindOne(Object selectedObject)
    {
        bool isMacOS = Application.platform == RuntimePlatform.OSXEditor;
        int totalWaitMilliseconds = isMacOS ? 2 * 1000 : 300 * 1000;
        int cpuCount = Environment.ProcessorCount;
        string appDataPath = Application.dataPath;
        
        string selectedAssetPath = AssetDatabase.GetAssetPath(selectedObject);
        if (AssetDatabase.IsValidFolder(selectedAssetPath))
        {
            return;
        }
        string selectedAssetGUID = AssetDatabase.AssetPathToGUID(selectedAssetPath);
        string selectedAssetMetaPath = selectedAssetPath + MetaExtension;

        var references = new List<string>();
        var output = new System.Text.StringBuilder();

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var psi = new ProcessStartInfo();
        psi.WindowStyle = ProcessWindowStyle.Minimized;

        if (isMacOS) {
            psi.FileName = "/usr/bin/mdfind";
            psi.Arguments = string.Format("-onlyin {0} {1}", appDataPath, selectedAssetGUID);
        } else {
            psi.FileName = Path.Combine(Environment.CurrentDirectory, "Assets/Editor/FindReferences/rg.exe");
            psi.Arguments = string.Format("--case-sensitive --follow --files-with-matches --no-text --fixed-strings " +
                                          "--ignore-file Assets/Editor/FindReferences/ignore.txt " +
                                          "--threads {0} --regexp {1} -- {2}",
                cpuCount, selectedAssetGUID, appDataPath);
        }

        psi.UseShellExecute = false;
        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;

        var process = new Process();
        process.StartInfo = psi;

        process.OutputDataReceived += (sender, e) => {
            if (string.IsNullOrEmpty(e.Data)) { return; }
            string relativePath = e.Data.Replace(appDataPath, "Assets").Replace("\\", "/");
            // skip the meta file of whatever we have selected
            if (relativePath == selectedAssetMetaPath) { return; }
            references.Add(relativePath);
        };
        process.ErrorDataReceived += (sender, e) => {
            if (string.IsNullOrEmpty(e.Data)){ return; }
            output.AppendLine("Error: " + e.Data);
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        while (!process.HasExited) {
            if (stopwatch.ElapsedMilliseconds < totalWaitMilliseconds) {
                float progress = (float)((double)stopwatch.ElapsedMilliseconds / totalWaitMilliseconds);
                string info = string.Format("Finding {0}/{1}s {2:P2}", stopwatch.ElapsedMilliseconds / 1000,
                    totalWaitMilliseconds / 1000, progress);
                bool canceled = EditorUtility.DisplayCancelableProgressBar("Find References in Project", info, progress);
                if (canceled) {
                    process.Kill();
                    break;
                }
                Thread.Sleep(100);
            } else {
                process.Kill();
                break;
            }
        }
        foreach (string file in references) {
            string guid = AssetDatabase.AssetPathToGUID(file);
            output.AppendLine(string.Format("{0} {1}", guid, file));
            string assetPath = file;
            if (file.EndsWith(MetaExtension)) {
                assetPath = file.Substring(0, file.Length - MetaExtension.Length);
            }
            UnityEngine.Debug.Log(string.Format("{0}\n{1}", file, guid), AssetDatabase.LoadMainAssetAtPath(assetPath));
        }

        EditorUtility.ClearProgressBar();
        stopwatch.Stop();

        string content = string.Format(
            "{0} {1} found for object: \"{2}\" path: \"{3}\" guid: \"{4}\" total time: {5}s\n\n{6}",
            references.Count, references.Count > 2 ? "references" : "reference", selectedObject.name, selectedAssetPath,
            selectedAssetGUID, stopwatch.ElapsedMilliseconds / 1000d, output);
        if (references.Count > 0)
        {
            UnityEngine.Debug.LogError(content, selectedObject);
        }
        else
        {
            AssetDatabase.DeleteAsset(selectedAssetPath);
            UnityEngine.Debug.LogWarning(content, selectedObject);
        }
    }
}