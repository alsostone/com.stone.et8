#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ProcessLog.Editor
{
    public class LogSymbolFile
    {
        private readonly HashSet<ushort> mSymbolSet = new HashSet<ushort>();
        private readonly Dictionary<ushort, LogSymbolData> mSymbolMapping = new Dictionary<ushort, LogSymbolData>();
        private ushort mSymbolID = 1;

        public bool Contains(ushort id)
        {
            return mSymbolSet.Contains(id);
        }

        public bool CheckExist(ushort id)
        {
            return mSymbolMapping.ContainsKey(id);
        }

        public void AddSymbolData(ushort id, LogFunctionData functionData)
        {
            var data = new LogSymbolData();
            data.ID = id;
            data.FileName = functionData.FileName;
            data.FuncName = functionData.FuncName;
            data.ArgTypes = functionData.ArgTypes;
            data.ArgNames = functionData.ArgNames;
            data.ValidArgNames = functionData.ValidArgNames;
            data.ValidArgCount = functionData.ValidArgCount;
            data.Comments = functionData.Comment;
            mSymbolMapping.Add(data.ID, data);
        }
        
        public ushort AddSymbolData(LogFunctionData functionData)
        {
            while (mSymbolSet.Contains(mSymbolID)) {
                mSymbolID++;
            }

            var info = new LogSymbolData();
            info.ID = mSymbolID;
            info.FileName = functionData.FileName;
            info.FuncName = functionData.FuncName;
            info.ArgTypes = functionData.ArgTypes;
            info.ArgNames = functionData.ArgNames;
            info.ValidArgNames = functionData.ValidArgNames;
            info.ValidArgCount = functionData.ValidArgCount;
            info.Comments = functionData.Comment;
            mSymbolMapping.Add(info.ID, info);
            mSymbolSet.Add(info.ID);
            return mSymbolID;
        }
        
        public void CreateSymbolFile(string fullPath)
        {
            if (mSymbolMapping.Count == 0) {
                return;
            }
            
            var symbols = new List<LogSymbolData>();
            foreach (var pair in mSymbolMapping) {
                symbols.Add(pair.Value);
            }
            
            symbols.Sort((data1, data2) => data1.ID.CompareTo(data2.ID));
            var builder = new StringBuilder(symbols.Count * 64);
            builder.AppendLine("[");
            foreach (var logSymbolData in symbols) {
                builder.AppendLine(MiniJSON.Json.Serialize(logSymbolData.ToDictionary()));
            }
            builder.Append("]");
            File.WriteAllText(fullPath, builder.ToString());
        }

        public void ReadSymbolFile(string fullPath)
        {
            mSymbolSet.Clear();
            if (!File.Exists(fullPath)) { return; }

            var readAllText = File.ReadAllText(fullPath);
            var idMatch = new Regex("\"ID\":\\s*(\\d+)");
            var matchCollection = idMatch.Matches(readAllText);
            for (var i = 0; i < matchCollection.Count; i++) {
                var id = ushort.Parse(matchCollection[i].Groups[1].Value);
                mSymbolSet.Add(id);
            }
        }

        public static void RemoveSymbolFile(string fullPath)
        {
            if (File.Exists(fullPath)) {
                File.Delete(fullPath);
            }
        }
    }
}
#endif