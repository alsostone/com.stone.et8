//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Diagnostics;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 打开文件夹相关的实用函数。
/// </summary>
static class OpenFolder
{
    /// <summary>
    /// 打开 Data Path 文件夹。
    /// </summary>
    [MenuItem("ET/Open Folder/Data Path", false, 10)]
    private static void OpenFolderDataPath()
    {
        Execute(Application.dataPath);
    }
    /// <summary>
    /// 打开 Persistent Data Path 文件夹。
    /// </summary>
    [MenuItem("ET/Open Folder/Persistent Data Path", false, 11)]
    private static void OpenFolderPersistentDataPath()
    {
        Execute(Application.persistentDataPath);
    }
    /// <summary>
    /// 打开 Streaming Assets Path 文件夹。
    /// </summary>
    [MenuItem("ET/Open Folder/Streaming Assets Path", false, 12)]
    private static void OpenFolderStreamingAssetsPath()
    {
        Execute(Application.streamingAssetsPath);
    }
    /// <summary>
    /// 打开 Temporary Cache Path 文件夹。
    /// </summary>
    [MenuItem("ET/Open Folder/Temporary Cache Path", false, 13)]
    private static void OpenFolderTemporaryCachePath()
    {
        Execute(Application.temporaryCachePath);
    }
    /// <summary>
    /// 打开指定路径的文件夹。
    /// </summary>
    /// <param name="folder">要打开的文件夹的路径。</param>
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