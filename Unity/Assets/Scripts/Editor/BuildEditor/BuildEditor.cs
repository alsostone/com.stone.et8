using System;
using System.IO;
using System.Reflection;
using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using UnityEditor;
using UnityEngine;
using YooAsset;
using YooAsset.Editor;
using BuildReport = UnityEditor.Build.Reporting.BuildReport;
using BuildResult = UnityEditor.Build.Reporting.BuildResult;

namespace ET
{
    /// <summary>
    /// ET菜单顺序
    /// </summary>
    public static class ETMenuItemPriority
    {
        public const int BuildTool = 1001;
        public const int ChangeDefine = 1002;
        public const int Compile = 1003;
        public const int Reload = 1004;
        public const int NavMesh = 1005;
        public const int ServerTools = 1006;
    }

    public static class BuildEditor
    {
        #region luban export
        private const string ExcelConfigPath = "../Unity/Assets/Bundles/Config";
        [MenuItem("ET/Excel/Exprot Client", false, 101)]
        public static void MenuExportExcelClient()
        {
            ToolsEditor.ExcelExporter(CodeMode.Client);
            if (Directory.Exists(ExcelConfigPath)) {
                FileHelper.CleanDirectory(ExcelConfigPath);
            }
            else {
                Directory.CreateDirectory(ExcelConfigPath);
            }
            FileHelper.CopyDirectory("../Config/Excel/c", ExcelConfigPath);
            AssetDatabase.Refresh();
        }
        [MenuItem("ET/Excel/Exprot Server", false, 102)]
        public static void MenuExportExcelServer()
        {
            ToolsEditor.ExcelExporter(CodeMode.Server);
        }
        [MenuItem("ET/Excel/Exprot ClientServer", false, 103)]
        public static void MenuExportExcelClientServer()
        {
            ToolsEditor.ExcelExporter(CodeMode.ClientServer);
        }
        [MenuItem("ET/Excel/Exprot All", false, 201)]
        public static void MenuExportExcelAll()
        {
            ToolsEditor.ExcelAllExporter();
            if (Directory.Exists(ExcelConfigPath)) {
                FileHelper.CleanDirectory(ExcelConfigPath);
            }
            else {
                Directory.CreateDirectory(ExcelConfigPath);
            }
            FileHelper.CopyDirectory("../Config/Excel/c", ExcelConfigPath);
            AssetDatabase.Refresh();
        }
        #endregion
        
        #region proto export
        [MenuItem("ET/Proto2CS", false, 102)]
        public static void Proto2CS()
        {
            ToolsEditor.Proto2CS();
            AssetDatabase.Refresh();
        }
        #endregion

        #region build package 
        [MenuItem("ET/Build/Android (Offline)", false, 101)]
        public static void MenuAndroidIncrementalBuild()
        {
            AutomationBuild(BuildTarget.Android, EPlayMode.OfflinePlayMode);
        }
        [MenuItem("ET/Build/Android (Host)", false, 102)]
        public static void MenuAndroidIncrementalBuildHost()
        {
            AutomationBuild(BuildTarget.Android, EPlayMode.HostPlayMode);
        }
        [MenuItem("ET/Build/IOS (Offline)", false, 201)]
        public static void MenuIOSIncrementalBuild()
        {
            AutomationBuild(BuildTarget.iOS, EPlayMode.OfflinePlayMode);
        }
        [MenuItem("ET/Build/IOS (Host)", false, 202)]
        public static void MenuIOSIncrementalBuildHost()
        {
            AutomationBuild(BuildTarget.iOS, EPlayMode.HostPlayMode);
        }
        [MenuItem("ET/Build/Windows (Offline)", false, 301)]
        public static void MenuWindowsIncrementalBuild()
        {
            AutomationBuild(BuildTarget.StandaloneWindows64, EPlayMode.OfflinePlayMode);
        }
        [MenuItem("ET/Build/Windows (Host)", false, 302)]
        public static void MenuWindowsIncrementalBuildHost()
        {
            AutomationBuild(BuildTarget.StandaloneWindows64, EPlayMode.HostPlayMode);
        }
        [MenuItem("ET/Build/OSX (Offline)", false, 401)]
        public static void MenuOSXIncrementalBuild()
        {
            AutomationBuild(BuildTarget.StandaloneOSX, EPlayMode.OfflinePlayMode);
        }
        [MenuItem("ET/Build/OSX (Host)", false, 402)]
        public static void MenuOSXIncrementalBuildHost()
        {
            AutomationBuild(BuildTarget.StandaloneOSX, EPlayMode.HostPlayMode);
        }
        #endregion
        
        private const string InitScene = "Assets/Scenes/Init.unity";
        private static void AutomationBuild(BuildTarget buildTarget, EPlayMode playMode)
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(InitScene);
            CheckAndSwitchBuildTarget(buildTarget);
            
            // 1.导出Excel配置并拷贝到YooAsset打包目录
            Log.Info("build step(1/6)[ExportExcel] start");
            MenuExportExcelClient();
            Log.Info("build step(1/6)[ExportExcel] success");
            
            // 2.强制设置成Client模式 & 指定PlayMode
            Log.Info("build step(2/6)[SetClient] start");
            var globalConfig = AssetDatabase.LoadAssetAtPath<GlobalConfig>("Assets/Resources/GlobalConfig.asset");
            globalConfig.CodeMode = CodeMode.Client;
            globalConfig.EPlayMode = playMode;
            EditorUtility.SetDirty(globalConfig);
            AssetDatabase.SaveAssets();
            AssemblyTool.EnableUnityClient();
            Log.Info("build step(2/6)[SetClient] success");
            
            // 3.编译热更DLL并拷贝到YooAsset打包目录
            Log.Info($"build step(3/6)[CompileDlls] start");
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            if (!AssemblyTool.CompileDlls())
                return;
            AssemblyTool.CopyHotUpdateDlls();
            BuildHelper.ReGenerateProjectFiles();
            Log.Info("build step(3/6)[CompileDlls] success");
            
            // 4.HybridCLR相关生成并拷贝到YooAsset打包目录
            Log.Info($"build step(4/6)[HybridCLR] start");
            SettingsUtil.Enable = MacroUtil.HasDefineSymbol(EMacroDefine.ENABLE_IL2CPP);
            if (SettingsUtil.Enable)
            {
                PrebuildCommand.GenerateAll();
                HybridCLREditor.CopyAotDll();
            }
            Log.Info($"build step(4/6)[HybridCLR] success");

            // 5.YooAsset Assetbundle打包
            Log.Info($"build step(5/6)[YooAsset] start");
            if (!YooAssetScriptableBuild(buildTarget, EBuildMode.IncrementalBuild))
                return;
            Log.Info($"build step(5/6)[YooAsset] success");
    
            // 6.1获取HybridCLR的BuildOptions 防止因为BuildOptions不一致导致包运行异常
            Type type = typeof(StripAOTDllCommand);
            MethodInfo method = type.GetMethod("GetBuildPlayerOptions", BindingFlags.Static | BindingFlags.NonPublic);
            BuildOptions buildOptions = (BuildOptions)method.Invoke(null, new object[1] { buildTarget });
            
            // 6.2所有资源就绪开始打包
            Log.Info($"build step(6/6)[BuildPlayer] start");
            string pathName = GetBuildName(buildTarget, buildOptions);
            string[] scenes = { InitScene };
            BuildReport report = BuildPipeline.BuildPlayer(scenes, pathName, buildTarget, buildOptions);
            if (report.summary.result != BuildResult.Succeeded)
            {
                Log.Info($"build step(6/6)[BuildPlayer] fail.");
                return;
            }
            Log.Info($"build step(6/6)[BuildPlayer] success");
        }
        
        private static void CheckAndSwitchBuildTarget(BuildTarget buildTarget)
        {
            Debug.LogFormat($"check and switch build target: {buildTarget}");
            if (EditorUserBuildSettings.activeBuildTarget != buildTarget) {
                BuildTargetGroup targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
                Log.Info($"switch buid target {EditorUserBuildSettings.activeBuildTarget} to {buildTarget}");
                EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroup, buildTarget);
            }
        }

        private static bool YooAssetScriptableBuild(BuildTarget buildTarget, EBuildMode buildMode)
        {
            var packages = AssetBundleCollectorSettingData.Setting.Packages;
            if (packages.Count == 0)
            {
                Log.Info($"build YooAsset Assetbundle failed. no package found.");
                return false;
            }
    
            // 方便起见版本号用默认 做打包管线时可改为传入参数
            int totalMinutes = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
            var packageVersion = DateTime.Now.ToString("yyyy-MM-dd") + "-" + totalMinutes;
            var packageName = packages[0].PackageName;
    
            // 所有打包选项和YooAsset AssetbundleBuilder面板保持一致 以便用面板修改设置
            var fileNameStyle = AssetBundleBuilderSetting.GetPackageFileNameStyle(packageName, EBuildPipeline.ScriptableBuildPipeline);
            var buildinFileCopyOption = AssetBundleBuilderSetting.GetPackageBuildinFileCopyOption(packageName, EBuildPipeline.ScriptableBuildPipeline);
            var buildinFileCopyParams = AssetBundleBuilderSetting.GetPackageBuildinFileCopyParams(packageName, EBuildPipeline.ScriptableBuildPipeline);
            var compressOption = AssetBundleBuilderSetting.GetPackageCompressOption(packageName, EBuildPipeline.ScriptableBuildPipeline);
            var encyptionClassName = AssetBundleBuilderSetting.GetPackageEncyptionClassName(packageName, EBuildPipeline.ScriptableBuildPipeline);
            var encryptionClassTypes = EditorTools.GetAssignableTypes(typeof(IEncryptionServices));
            var classType = encryptionClassTypes.Find(x => x.FullName.Equals(encyptionClassName));
            var encryptionServices = classType != null ?  (IEncryptionServices)Activator.CreateInstance(classType) : null;
    
            ScriptableBuildParameters buildParameters = new ScriptableBuildParameters();
            buildParameters.BuildOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();
            buildParameters.BuildinFileRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot();
            buildParameters.BuildPipeline = EBuildPipeline.ScriptableBuildPipeline.ToString();
            buildParameters.BuildTarget = buildTarget;
            buildParameters.BuildMode = buildMode;
            buildParameters.PackageName = packageName;
            buildParameters.PackageVersion = packageVersion;
            buildParameters.EnableSharePackRule = true;
            buildParameters.VerifyBuildingResult = true;
            buildParameters.FileNameStyle = fileNameStyle;
            buildParameters.BuildinFileCopyOption = buildinFileCopyOption;
            buildParameters.BuildinFileCopyParams = buildinFileCopyParams;
            buildParameters.EncryptionServices = encryptionServices;
            buildParameters.CompressOption = compressOption;
    
            ScriptableBuildPipeline pipeline = new ScriptableBuildPipeline();
            var buildResult = pipeline.Run(buildParameters, true);
            if (buildResult.Success)
            {
                Log.Info($"build YooAsset Assetbundle success. {buildResult.OutputPackageDirectory}");
                return true;
            }
            Log.Error($"build YooAsset Assetbundle failed. {buildResult.ErrorInfo}");
            return false;
        }
    
        private static bool YooAssetBuiltinBuild(BuildTarget buildTarget, EBuildMode buildMode)
        {
            var packages = AssetBundleCollectorSettingData.Setting.Packages;
            if (packages.Count == 0)
            {
                Log.Info($"build YooAsset Assetbundle failed. no package found.");
                return false;
            }
    
            // 方便起见版本号用默认 做打包管线时可改为传入参数
            int totalMinutes = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
            var packageVersion = DateTime.Now.ToString("yyyy-MM-dd") + "-" + totalMinutes;
            var packageName = packages[0].PackageName;
    
            // 所有打包选项和YooAsset AssetbundleBuilder面板保持一致 以便用面板修改设置
            var fileNameStyle = AssetBundleBuilderSetting.GetPackageFileNameStyle(packageName, EBuildPipeline.BuiltinBuildPipeline);
            var buildinFileCopyOption = AssetBundleBuilderSetting.GetPackageBuildinFileCopyOption(packageName, EBuildPipeline.BuiltinBuildPipeline);
            var buildinFileCopyParams = AssetBundleBuilderSetting.GetPackageBuildinFileCopyParams(packageName, EBuildPipeline.BuiltinBuildPipeline);
            var compressOption = AssetBundleBuilderSetting.GetPackageCompressOption(packageName, EBuildPipeline.BuiltinBuildPipeline);
            var encyptionClassName = AssetBundleBuilderSetting.GetPackageEncyptionClassName(packageName, EBuildPipeline.BuiltinBuildPipeline);
            var encryptionClassTypes = EditorTools.GetAssignableTypes(typeof(IEncryptionServices));
            var classType = encryptionClassTypes.Find(x => x.FullName.Equals(encyptionClassName));
            var encryptionServices = classType != null ?  (IEncryptionServices)Activator.CreateInstance(classType) : null;
    
            BuiltinBuildParameters buildParameters = new BuiltinBuildParameters();
            buildParameters.BuildOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();
            buildParameters.BuildinFileRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot();
            buildParameters.BuildPipeline = EBuildPipeline.BuiltinBuildPipeline.ToString();
            buildParameters.BuildTarget = buildTarget;
            buildParameters.BuildMode = buildMode;
            buildParameters.PackageName = packageName;
            buildParameters.PackageVersion = packageVersion;
            buildParameters.EnableSharePackRule = true;
            buildParameters.VerifyBuildingResult = true;
            buildParameters.FileNameStyle = fileNameStyle;
            buildParameters.BuildinFileCopyOption = buildinFileCopyOption;
            buildParameters.BuildinFileCopyParams = buildinFileCopyParams;
            buildParameters.EncryptionServices = encryptionServices;
            buildParameters.CompressOption = compressOption;
    
            BuiltinBuildPipeline pipeline = new BuiltinBuildPipeline();
            var buildResult = pipeline.Run(buildParameters, true);
            if (buildResult.Success)
            {
                Log.Info($"build YooAsset Assetbundle success. {buildResult.OutputPackageDirectory}");
                return true;
            }
            Log.Error($"build YooAsset Assetbundle failed. {buildResult.ErrorInfo}");
            return false;
        }
    
        private static string GetBuildDir(BuildTarget target)
        {
            return $"/Release/{target.ToString()}";
        }
    
        private static string GetBuildName(BuildTarget target, BuildOptions buildOptions)
        {
            switch (target) {
                case BuildTarget.Android:
                    if (!buildOptions.HasFlag(BuildOptions.AcceptExternalModificationsToPlayer))
                        return $"../Release/{target.ToString()}/{PlayerSettings.productName}.apk";
                    break;
                case BuildTarget.StandaloneOSX:
                    return $"../Release/{target.ToString()}/{PlayerSettings.productName}.app";
                case BuildTarget.StandaloneWindows64:
                    return $"../Release/{target.ToString()}/{PlayerSettings.productName}.exe";
            }
            return $"../Release/{target.ToString()}";
        }
        
    }
}