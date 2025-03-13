# 基于YIUI-ET8.1 
## 包含Luban，YIUI，HybridCLR，YooAsset
## `正在支持帧同步中的技能系统`

## 支持功能
1.  移除ET的UI框架 使用YIUI框架实现所有UI
2.  ET/Build/XXX 一键打各平台包
3.  ET/Excel/XXX 一键导出Luban配置表

## 打包自动流程
1. 切换目标平台
2. 若当前不是Client`GlobalConfig中CodeMode`模式 则强制设置成Client模式
3. 编译热更DLL 并拷贝到YooAsset打包目录
4. HybridCLR生成 并拷贝到YooAsset打包目录
5. 导出Excel配置 并拷贝到YooAsset打包目录
6. YooAsset资源打包 可通过`YooAsset/Assetbundle Builder`修改默认配置
7. BuildPipeline.BuildPlayer 出包

```csharp
private static void AutomationBuild(BuildTarget buildTarget, EPlayMode playMode, BuildOptions buildOptions = BuildOptions.None)
{
    if (EditorUserBuildSettings.activeBuildTarget != buildTarget) {
        BuildTargetGroup targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
        Log.Info($"switch buid target {EditorUserBuildSettings.activeBuildTarget} to {buildTarget}");
        EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroup, buildTarget);
    }
    
    // 强制设置成Client模式 & 指定PlayMode
    var globalConfig = AssetDatabase.LoadAssetAtPath<GlobalConfig>("Assets/Resources/GlobalConfig.asset");
    globalConfig.CodeMode = CodeMode.Client;
    globalConfig.EPlayMode = playMode;
    EditorUtility.SetDirty(globalConfig);
    AssetDatabase.SaveAssets();
    AssemblyTool.EnableUnityClient();

    // 编译热更DLL并拷贝到YooAsset打包目录
    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
    if (!AssemblyTool.CompileDlls())
        return;
    AssemblyTool.CopyHotUpdateDlls();
    BuildHelper.ReGenerateProjectFiles();
    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
    
    // HybridCLR相关生成并拷贝到YooAsset打包目录
    if (Define.EnableIL2CPP)
    {
        PrebuildCommand.GenerateAll();
        HybridCLREditor.CopyAotDll();
    }
    AssetDatabase.Refresh();
    
    // 导出Excel配置并拷贝到YooAsset打包目录
    MenuExportExcelClient();

    // YooAsset Assetbundle打包
    if (!YooAssetScriptableBuild(buildTarget, EBuildMode.IncrementalBuild))
        return;
    AssetDatabase.Refresh();

    string pathName = GetBuildName(buildTarget, buildOptions);
    string[] scenes = { "Assets/Scenes/Init.unity" };

    Log.Info("build start");
    BuildReport report = BuildPipeline.BuildPlayer(scenes, pathName, buildTarget, buildOptions);
    if (report.summary.result != BuildResult.Succeeded)
    {
        Log.Error($"build not success: {report.summary.result}");
        return;
    }
    Log.Info("build success");
}
```

## Luban 配置表
1. 修改表导出类名以Tb为前缀
2. 导表分为4类 client/server/client_server/all 分别对应GlobalConfig中CodeMode的配置
3. 把StartConfig相关的4个平台表，同类名但不同数据，依靠不生成类标签`[Config]`实现（不够优雅，先用着）
```csharp
public static void ExcelExporter(CodeMode codeMode)
{...}

public static void ExcelAllExporter()
{
#if UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
    ShellHelper.Run("sh export_excel_client.sh", "../Tools/Luban/");
    ShellHelper.Run("sh export_excel_server.sh", "../Tools/Luban/");
    ShellHelper.Run("sh export_excel_client_server.sh", "../Tools/Luban/");
#else
    ShellHelper.Run("export_excel_client.bat", "../Tools/Luban/");
    ShellHelper.Run("export_excel_server.bat", "../Tools/Luban/");
    ShellHelper.Run("export_excel_client_server.bat", "../Tools/Luban/");
#endif
}
```
# Reference
1. ET [ET8.1](https://github.com/egametang/ET/tree/release8.1)
2. YIUI [YIUI-ET8.1](https://github.com/LiShengYang-yiyi/YIUI/tree/YIUI-ET8.1)
3. X-ET7 [X-ET7 master](https://github.com/IcePower/X-ET7)
