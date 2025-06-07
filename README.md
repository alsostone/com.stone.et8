## 帧同步战斗系统
### 目标
原子化设计、配套齐全（如调试工具、AI工具、各种插件）、客户端服务器代码共用、打通匹配到结算流程，以流行技术栈搭建。
#### 参考《帧同步战斗架构设计Ver.2》来设计。 [文档地址](https://zhuanlan.zhihu.com/p/1911184476500897969)
预期2025年底能完工。

### 如何运行
1. 切换到Init场景
2. ET/Excel/XXX导表
3. Lockstep模式 AWSD移动 J普攻 K技能。

### 战斗单位
单位配置表位置 `com.stone.et8/Unity/Assets/Config/Excel/Datas/Unit`
1. 英雄 皮肤、名字、配置初始属性、初始技能
2. 建筑 等级、名字、模型、初始属性、初始技能
3. 小兵 等级、名字、模型、初始属性、初始技能
4. 掉落物 逐步拓展中，后续再补充；
### 技能系统
技能配置表位置 `com.stone.et8/Unity/Assets/Config/Excel/Datas/Skill`
1. 技能配置 释放条件检测、CD、初始CD、消耗、前摇动作、持续动作、后摇动作、效果触发点、效果组、是否实时索敌；
2. 效果实现 伤害、添加BUFF、属性改变、添加子弹、重新索敌(炮弹落地炸一圈)、召唤士兵、随机召唤(基于随机掉落包)；
3. BUFF系统 支持持续时间、BUFF生效效果、Dot伤害效果、BUFF结束效果；
4. 索敌系统 支持指定范围、队伍、类型、优先级；
5. 移动轨迹 轨迹基于贝塞尔曲线，当前支持1个控制点(配置控制点趋近起终点比例和控制点高度偏移)，支持炸弹抛射；
### AI
正在开发中...


## 基于YIUI-ET8.1拓展 包含Luban，YIUI，HybridCLR，YooAsset
## 支持功能
1.  移除ET的UI框架 使用YIUI框架实现所有UI
2.  移除ET的打包流程 使用ET/Build/XXX 一键打各平台包
3.  移除ET的导表流程 ET/Excel/XXX 一键导出Luban配置表

## 打包自动流程
ps: 首次打包需手动安装HybridCLR HybridCLR/Installer
    如果打包失败，多试几次，依旧报错再尝试解决。`一键打包只是流程整合，便于了解打包流程，并不能避免流程中某节点报错`。
1. 切换目标平台
2. 若当前不是Client`GlobalConfig中CodeMode`模式 则强制设置成Client模式
3. 编译热更DLL 并拷贝到YooAsset打包目录
4. HybridCLR生成 并拷贝到YooAsset打包目录
5. 导出Excel配置 并拷贝到YooAsset打包目录
6. YooAsset资源打包 可通过`YooAsset/Assetbundle Builder`修改默认配置
7. BuildPipeline.BuildPlayer 出包

```csharp
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
## 插件目录
项目中用到了以下插件，部分为付费插件，如商用请在Unity商店购买。此处只做学习研究使用，如有侵权请联系删除。

1. 伤害飘字使用DamageNumbersPro [Unity商店地址](https://assetstore.unity.com/packages/2d/gui/damage-numbers-pro-186447)
2. 血条HUD是用com.stone.hud [GitHub地址](https://github.com/alsostone/com.stone.hud)
3. 行为树用NPBehave [GitHub地址](https://github.com/alsostone/NPBehave)

# Reference
1. ET [ET8.1](https://github.com/egametang/ET/tree/release8.1)
2. YIUI [YIUI-ET8.1](https://github.com/LiShengYang-yiyi/YIUI/tree/YIUI-ET8.1)
3. X-ET7 [X-ET7 master](https://github.com/IcePower/X-ET7)
