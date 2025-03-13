using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MacroDefineWnd : EditorWindow
{
    private const int WndWidth = 340;
    private const int WndHeight = 300;
    private const int ScrollRegionHeight = 260;
    private const int BtnWidth = 60;
    private const int BtnHeight = 30;
    private const float Space = 5;
    private Vector2 mScrollPos;
    private List<MacroMenuItem> MacroMenuItems = new List<MacroMenuItem>();

    [MenuItem("ET/MacroDefine &M", false, 100)]
    private static void OpenSyncModelWnd()
    {
        MacroDefineWnd wnd = GetWindowWithRect<MacroDefineWnd>
            (new Rect(0, 0, WndWidth, WndHeight), true, "宏设置", true);
        wnd.Init();
        wnd.Show();
    }
    private void OnDestroy()
    {
        Debug.Log("窗口关闭");
    }
    private void Init()
    {
        MacroMenuItems.Clear();
        foreach (var code in Enum.GetValues(typeof(EMacroDefine))) {
            string enumName = code.ToString();
            string enumDescription = ((EMacroDefine)code).ToDescription();
            
            MacroMenuItem item = new MacroMenuItem(enumName, enumDescription);
            MacroMenuItems.Add(item);
        }
    }
    private void OnGUI()
    {
        using (GUILayout.ScrollViewScope scrollScope = new GUILayout.ScrollViewScope(mScrollPos, false, true,
        GUILayout.Width(WndWidth), GUILayout.Height(ScrollRegionHeight))) {
            mScrollPos = scrollScope.scrollPosition;
            GUILayout.BeginVertical();

            foreach (var item in MacroMenuItems) {
                GUILayout.BeginHorizontal();
                GUIStyle toggleStyle = new GUIStyle(GUI.skin.toggle);
                toggleStyle.fontSize = 16;
                toggleStyle.imagePosition = ImagePosition.ImageLeft;
                item.Selected = GUILayout.Toggle(item.Selected, "  " + item.ShowName, toggleStyle, GUILayout.Width(240), GUILayout.Height(20));
                GUIStyle labelStyle = new GUIStyle(GUI.skin.label);
                labelStyle.normal.textColor = Color.cyan;
                labelStyle.fontSize = 16;
                labelStyle.alignment = TextAnchor.MiddleLeft;
                if (item.Enable != item.Selected) {
                    GUILayout.Label("更改", labelStyle, GUILayout.Width(60), GUILayout.Height(20));
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }
        GUILayout.Space(Space);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("全部选中", GUILayout.Width(BtnWidth), GUILayout.Height(BtnHeight))) {
            SetAllSelect(true);
        }
        GUILayout.Space(Space);
        if (GUILayout.Button("取消选中", GUILayout.Width(BtnWidth), GUILayout.Height(BtnHeight))) {
            SetAllSelect(false);
        }
        GUILayout.Space(Space);
        if (GUILayout.Button("全部反选", GUILayout.Width(BtnWidth), GUILayout.Height(BtnHeight))) {
            ReverseSelected();
        }
        GUILayout.Space(Space);
        if (GUILayout.Button("还原修改", GUILayout.Width(BtnWidth), GUILayout.Height(BtnHeight))) {
            Init();
        }
        GUILayout.Space(Space);

        if (GUILayout.Button("保存设置", GUILayout.Width(BtnWidth), GUILayout.Height(BtnHeight))) {
            Save();
        }
        GUILayout.Space(Space);

        GUILayout.EndHorizontal();
    }
    private void SetAllSelect(bool selected)
    {
        foreach (var item in MacroMenuItems) {
            item.Selected = selected;
        }
    }
    private void ReverseSelected()
    {
       foreach (var item in MacroMenuItems) {
            item.Selected = !item.Selected;
       }
    }
    private void Save()
    {
        foreach (var item in MacroMenuItems) {
            item.Save();
        }
        Close();
    }
}

public class MacroMenuItem
{
    public string ShowName;
    public string MacroDefine;
    public bool Selected;
    public bool Enable;

    public MacroMenuItem(string macroDefine, string showName)
    {
        MacroDefine = macroDefine;
        ShowName = showName;
        Init();
    }
    public void Init()
    {
        Enable = MacroUtil.HasDefineSymbol(MacroDefine);
        Selected = Enable;
    }
    public void Save()
    {
        if (Enable != Selected) {
            if (Selected) {
                MacroUtil.AddDefineSymbols(MacroDefine);
            } else {
                MacroUtil.RemoveDefineSymbols(MacroDefine);
            }
        }
    }
}
