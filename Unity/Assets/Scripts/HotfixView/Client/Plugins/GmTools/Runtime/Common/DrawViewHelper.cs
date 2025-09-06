#if ENABLE_DEBUG
using UnityEngine;

namespace ST.GmTools
{
    internal static class DrawViewHelper
    {
        internal static void DrawEmpty(string text, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(text);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
        }
        
        internal static int DrawDropDown(int selected, string[] texts, ref bool isOpen)
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(texts[selected])) {
                isOpen = !isOpen;
            }
            if (GUILayout.Button(isOpen ? "∧" : "∨", GUILayout.Width(20))) {
                isOpen = !isOpen;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            if (isOpen) {
                GUILayout.Space(2);
                for (var index = 0; index < texts.Length; index++) {
                    if (GUILayout.Button(texts[index])) {
                        selected = index;
                        isOpen = false;
                    }
                }
                GUILayout.Space(5);
            }
            GUILayout.EndVertical();
            GUILayout.Space(24);
            GUILayout.EndHorizontal();
            
            GUILayout.EndHorizontal();
            return selected;
        }

    }
}
#endif