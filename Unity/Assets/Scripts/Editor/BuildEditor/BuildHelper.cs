using UnityEditor;

namespace ET
{
    public static class BuildHelper
    {
        public static string BuildFolder = "../Release/{0}/StreamingAssets/";

        [InitializeOnLoadMethod]
        public static void ReGenerateProjectFiles()
        {
            Unity.CodeEditor.CodeEditor.CurrentEditor.SyncAll();
        }
    }
}