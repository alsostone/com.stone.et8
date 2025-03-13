using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

static class MacroUtil
{
    public static void AddDefineSymbols(params string[] symbols)
    {
        if (symbols == null || symbols.Length == 0) { return; }
        var group = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
        Debug.LogFormat("BuildTagetGroup:{0}, add define symbols:{1}", group, string.Join(";", symbols));
        string defString = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
        List<string> allDefines = defString.Split(';').ToList();
        allDefines.AddRange(symbols.Except(allDefines));
        PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", allDefines.ToArray()));
        AssetDatabase.SaveAssets();
    }
    
    public static void RemoveDefineSymbols(params string[] symbols)
    {
        if (symbols == null || symbols.Length == 0) { return; }
        var group = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
        Debug.LogFormat("BuildTagetGroup:{0}, remove define symbols:{1}", group, string.Join(";", symbols));
        string defString = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
        List<string> allDefines = defString.Split(';').ToList();
        foreach (var symbol in symbols) {
            allDefines.Remove(symbol);
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(group, string.Join(";", allDefines.ToArray()));
        AssetDatabase.SaveAssets();
    }
    
    public static bool HasDefineSymbol(string symbol)
    {
        if (string.IsNullOrEmpty(symbol)) { return false; }
        var group = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
        string defString = PlayerSettings.GetScriptingDefineSymbolsForGroup(group);
        List<string> allDefines = defString.Split(';').ToList();
        return allDefines.Contains(symbol);
    }

    public static void AddDefineSymbols(params EMacroDefine[] macroDefines)
    {
        if (macroDefines == null || macroDefines.Length == 0) { return; }
        string[] symbols = Array.ConvertAll<EMacroDefine, string>(macroDefines, s => s.ToString());
        AddDefineSymbols(symbols);
    }
    
    public static void RemoveDefineSymbols(params EMacroDefine[] macroDefines)
    {
        if (macroDefines == null || macroDefines.Length == 0) { return; }
        string[] symbols = Array.ConvertAll<EMacroDefine, string>(macroDefines, s => s.ToString());
        RemoveDefineSymbols(symbols);
    }
    
    public static bool HasDefineSymbol(EMacroDefine macroDefine)
    {
        return HasDefineSymbol(macroDefine.ToString());
    }
}