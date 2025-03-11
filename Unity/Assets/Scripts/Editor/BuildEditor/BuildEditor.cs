using System;
using System.IO;
using HybridCLR.Editor.Commands;
using UnityEditor;
using UnityEngine;
using YooAsset;
using YooAsset.Editor;
using BuildReport = UnityEditor.Build.Reporting.BuildReport;
using BuildResult = UnityEditor.Build.Reporting.BuildResult;

namespace ET
{
    public enum PlatformType
    {
        None,
        Android,
        IOS,
        Windows,
        MacOS,
        Linux
    }

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

    public class BuildEditor : EditorWindow
    {
        private PlatformType activePlatform;
        private PlatformType platformType;
        private BuildOptions buildOptions;

        private GlobalConfig globalConfig;

        [MenuItem("ET/Build Tool", false, ETMenuItemPriority.BuildTool)]
        public static void ShowWindow()
        {
            GetWindow<BuildEditor>(DockDefine.Types);
        }

        private void OnEnable()
        {
            globalConfig = AssetDatabase.LoadAssetAtPath<GlobalConfig>("Assets/Resources/GlobalConfig.asset");

#if UNITY_ANDROID
            activePlatform = PlatformType.Android;
#elif UNITY_IOS
            activePlatform = PlatformType.IOS;
#elif UNITY_STANDALONE_WIN
            activePlatform = PlatformType.Windows;
#elif UNITY_STANDALONE_OSX
            activePlatform = PlatformType.MacOS;
#elif UNITY_STANDALONE_LINUX
            activePlatform = PlatformType.Linux;
#else
            activePlatform = PlatformType.None;
#endif
            platformType = activePlatform;
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("PlatformType ");
            this.platformType = (PlatformType)EditorGUILayout.EnumPopup(platformType);

            EditorGUILayout.LabelField("BuildOptions ");
            this.buildOptions = (BuildOptions)EditorGUILayout.EnumFlagsField(this.buildOptions);

            GUILayout.Space(5);

            if (GUILayout.Button("BuildPackage"))
            {
                if (this.platformType == PlatformType.None)
                {
                    Log.Error("please select platform!");
                    return;
                }

                if (this.globalConfig.CodeMode != CodeMode.Client)
                {
                    Log.Error("build package CodeMode must be CodeMode.Client, please select Client");
                    return;
                }

                if (this.globalConfig.EPlayMode == EPlayMode.EditorSimulateMode)
                {
                    Log.Error("build package EPlayMode must not be EPlayMode.EditorSimulateMode, please select HostPlayMode");
                    return;
                }

                if (platformType != activePlatform)
                {
                    switch (EditorUtility.DisplayDialogComplex("Warning!", $"current platform is {activePlatform}, if change to {platformType}, may be take a long time", "change", "cancel", "no change"))
                    {
                        case 0:
                            activePlatform = platformType;
                            break;
                        case 1:
                            return;
                        case 2:
                            platformType = activePlatform;
                            break;
                    }
                }

                BuildHelper.Build(this.platformType, this.buildOptions);
                return;
            }

            if (GUILayout.Button("ExcelExporter"))
            {
                ToolsEditor.ExcelExporter();
                return;
            }

            if (GUILayout.Button("Proto2CS"))
            {
                ToolsEditor.Proto2CS();
                return;
            }

            GUILayout.Space(5);
        }
    
        #region build package 
        [MenuItem("ET/Build/Android", false, 101)]
        public static void MenuAndroidIncrementalBuild()
        {
            AutomationBuild(BuildTarget.Android, true);
        }
        [MenuItem("ET/Build/Android(Release)", false, 102)]
        public static void MenuAndroidIncrementalBuildRelease()
        {
            AutomationBuild(BuildTarget.Android, false);
        }
        [MenuItem("ET/Build/IOS", false, 201)]
        public static void MenuIOSIncrementalBuild()
        {
            AutomationBuild(BuildTarget.iOS, true);
        }
        [MenuItem("ET/Build/IOS(Release)", false, 202)]
        public static void MenuIOSIncrementalBuildRelease()
        {
            AutomationBuild(BuildTarget.iOS, false);
        }
        
        [MenuItem("ET/Build/Windows", false, 301)]
        public static void MenuWindowsIncrementalBuild()
        {
            AutomationBuild(BuildTarget.StandaloneWindows64, true);
        }
        [MenuItem("ET/Build/Windows(Release)", false, 302)]
        public static void MenuWindowsIncrementalBuildRelease()
        {
            AutomationBuild(BuildTarget.StandaloneWindows64, false);
        }
        [MenuItem("ET/Build/OSX", false, 401)]
        public static void MenuOSXIncrementalBuild()
        {
            AutomationBuild(BuildTarget.StandaloneOSX, true);
        }
        [MenuItem("ET/Build/OSX(Release)", false, 402)]
        public static void MenuOSXIncrementalBuildRelease()
        {
            AutomationBuild(BuildTarget.StandaloneOSX, false);
        }
        #endregion
    
        private static void AutomationBuild(BuildTarget buildTarget, bool development = false, BuildOptions buildOptions = BuildOptions.None)
        {
            var globalConfig = AssetDatabase.LoadAssetAtPath<GlobalConfig>("Assets/Resources/GlobalConfig.asset");
            if (globalConfig.CodeMode != CodeMode.Client) {
                Log.Error("build package CodeMode must be CodeMode.Client, please select Client");
                return;
            }
            
            if (EditorUserBuildSettings.activeBuildTarget != buildTarget) {
                BuildTargetGroup targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
                Log.Info($"switch buid target {EditorUserBuildSettings.activeBuildTarget} to {buildTarget}");
                EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroup, buildTarget);
            }
            
            EditorUserBuildSettings.development = development;
    
            // 强行设置成Client模式 并拷贝热更DLL到YooAsset打包目录
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            AssemblyTool.RefreshCodeMode();
            if (!AssemblyTool.CompileDlls())
                return;
            AssemblyTool.CopyHotUpdateDlls();
            BuildHelper.ReGenerateProjectFiles();
            AssetDatabase.Refresh();
            
            // HybridCLR的前置编译和AOT拷贝
            if (Define.EnableIL2CPP)
            {
                PrebuildCommand.GenerateAll();
                HybridCLREditor.CopyAotDll();
            }
            AssetDatabase.Refresh();
    
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