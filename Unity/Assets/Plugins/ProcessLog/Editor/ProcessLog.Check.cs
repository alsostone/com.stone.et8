#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;

namespace ProcessLog.Editor
{
    public partial class ProcessLog
    {
        private class LogSymbolLine
        {
            public ushort ID;
            public string FileName;
        }

        private static readonly Dictionary<ushort, LogSymbolLine> LogSymbolLines = new Dictionary<ushort, LogSymbolLine>();

        private static void ClearCheckProcessLogAll()
        {
            LogSymbolLines.Clear();
        }
        
        private static void CheckProcessLog(string filename, LogSymbolFile symbolFile)
        {
            var text = File.ReadAllText(filename, Utf8Encoding);
            var matches = ms_regexLogCode.Matches(text);
            for (var i = matches.Count - 1; i >= 0; i--) {
                var match = matches[i];
                var args = match.Groups[3].Value;
                var id = (ushort)int.Parse(args.Contains(",") ? args.Substring(0, args.IndexOf(',')).Trim() : args);
                
                var info = new LogSymbolLine();
                info.ID = id;
                info.FileName = filename;

                if (LogSymbolLines.ContainsKey(id)) {
                    var format = $"LogFunction ID:{id} repeated. files:";
                    format += $"fileName:{LogSymbolLines[id].FileName}";
                    format += $"fileName:{filename}";
                    ET.Log.Error(format);
                    continue;
                }
                LogSymbolLines.Add(id, info);
                            
                if (!symbolFile.Contains(id)) {
                    ET.Log.Error($"ID:{id} not exist in SymbolFile.");
                }
            }
        }
        
    }
}
#endif