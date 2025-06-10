#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TrueSync;
using ICSharpCode.SharpZipLib.BZip2;

namespace ProcessLog.Editor
{
    public partial class ProcessLog
    {
        private static readonly Dictionary<ushort, LogSymbolData> LogSymbolMapping = new Dictionary<ushort, LogSymbolData>();
        private static readonly StringBuilder StringBuilder = new StringBuilder(512);
        private static readonly List<long> TempArgs = new List<long>();
        
        private static void LoadSymbolFile(string filename)
        {
            var tempData = MiniJSON.Json.Deserialize(File.ReadAllText(filename));
            var jsonData = tempData as List<object>;
            if (jsonData == null) {
                ET.Log.Error("Symbol table is empty.");
                return;
            }
            
            LogSymbolMapping.Clear();
            for (var i = 0; i < jsonData.Count; i++) {
                var data = jsonData[i] as Dictionary<string, object>;
                if (data == null) {
                    ET.Log.Error("Symbol table is empty.");
                    continue;
                }
                
                var logSymbolData = new LogSymbolData();
                logSymbolData.ID = ushort.Parse(data["ID"].ToString());
                logSymbolData.FileName = data["FileName"] != null ? data["FileName"].ToString() : "";
                logSymbolData.FuncName = data["FuncName"] != null ? data["FuncName"].ToString() : "";
                logSymbolData.ArgTypes = data["ArgTypes"] != null ? data["ArgTypes"].ToString() : "";
                logSymbolData.ArgNames = data["ArgNames"] != null ? data["ArgNames"].ToString() : "";
                logSymbolData.ValidArgNames = data["ValidArgNames"] != null ? data["ValidArgNames"].ToString() : "";
                logSymbolData.ValidArgCount = int.Parse(data["ValidArgCount"].ToString());
                logSymbolData.Comments = data["Comments"] != null ? data["Comments"].ToString() : "";
                LogSymbolMapping.Add(logSymbolData.ID, logSymbolData);
            }
        }

        private static void ConvertToTextFile(string binFilename, string txtFilename)
        {
            var logFrameDatas = new List<LogFrameData>();
            using (var fs = new FileStream(binFilename, FileMode.Open, FileAccess.Read)) {
                var bzipHeader = new byte[2];
                fs.Read(bzipHeader, 0, 2);
                fs.Position = 0;

                BinaryReader binaryReader = null;
                if (bzipHeader[0] == 'B' && bzipHeader[1] == 'Z') {
                    var decompressStream = new MemoryStream();
                    BZip2.Decompress(fs, decompressStream, false);
                    binaryReader = new BinaryReader(decompressStream);
                    decompressStream.Position = 0;
                }
                else {
                    binaryReader = new BinaryReader(fs);
                }

                while (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length) {
                    var logFrameData = new LogFrameData();
                    if (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length) {
                        logFrameData.FrameIndex = binaryReader.ReadInt32();
                    }

                    if (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length) {
                        logFrameData.Hash = binaryReader.ReadInt64();
                    }

                    if (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length) {
                        var idCount = binaryReader.ReadInt32();
                        for (var i = 0; i < idCount; i++) {
                            logFrameData.Ids.Add(binaryReader.ReadUInt16());
                        }
                    }

                    if (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length) {
                        var argCount = binaryReader.ReadInt32();
                        for (var i = 0; i < argCount; i++) {
                            logFrameData.Args.Add(binaryReader.ReadInt64());
                        }
                    }

                    if (binaryReader.BaseStream.Position != binaryReader.BaseStream.Length) {
                        var luaLogCount = binaryReader.ReadInt32();
                        for (var i = 0; i < luaLogCount; i++) {
                            var byteLength = binaryReader.ReadInt32();
                            var luaLog = Encoding.UTF8.GetString(binaryReader.ReadBytes(byteLength));
                            logFrameData.Strings.Add(luaLog);
                        }
                    }
                    logFrameDatas.Add(logFrameData);
                }
            }

            var stringBuilder = new StringBuilder(1024 * 1024 * 10);
            try {
                foreach (var logFrameData in logFrameDatas) {
                    ConvertToTextFrame(logFrameData, stringBuilder);
                }
            }
            catch (Exception e) {
                ET.Log.Error(e.ToString());
            }
            finally {
                if (stringBuilder.Length > 0) {
                    using (var fs = new FileStream(txtFilename, FileMode.Create, FileAccess.Write)) {
                        var bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
            }
        }
        
        private static void ConvertToTextFrame(LogFrameData logFrameData, StringBuilder content)
        {
            content.AppendLine();
            content.Append("Frame:");
            content.Append(logFrameData.FrameIndex);
            content.AppendLine();
            content.Append("Hash:");
            content.Append(logFrameData.Hash);
            content.AppendLine();
            content.Append("ID Count:");
            content.Append(logFrameData.Ids.Count);
            content.AppendLine();
            content.Append("Args Count:");
            content.Append(logFrameData.Args.Count);
            content.AppendLine();
            
            var indexArg = 0; var indexString = 0;
            for (var index = 0; index < logFrameData.Ids.Count; index++) {
                var id = logFrameData.Ids[index];
                if (id == 9999) {
                    content.AppendLine(logFrameData.Strings[indexString]);
                    indexString++;
                }
                else {
                    if (LogSymbolMapping.TryGetValue(id, out var logSymbolData)) {
                        TempArgs.Clear();

                        var count = indexArg + logSymbolData.ValidArgCount;
                        for (; indexArg < count; indexArg++) {
                            if (logFrameData.Args.Count > indexArg) {
                                TempArgs.Add(logFrameData.Args[indexArg]);
                            }
                        }

                        content.AppendLine(ConvertToTextRow(logSymbolData, TempArgs));
                    }
                }
            }
        }

        private static string ConvertToTextRow(LogSymbolData logSymbolData, IReadOnlyList<long> args)
        {
            if (logSymbolData == null) { return string.Empty; }

            StringBuilder.Length = 0;
            StringBuilder.Append("ID:");
            StringBuilder.Append(logSymbolData.ID);
            StringBuilder.Append(" ");
            StringBuilder.Append(logSymbolData.FileName);
            StringBuilder.Append("-->>");
            StringBuilder.Append(logSymbolData.FuncName);
            StringBuilder.Append("(");
            
            if (!string.IsNullOrEmpty(logSymbolData.ArgNames)) {
                var argNames = logSymbolData.ArgNames.Split(',');
                for (var index = 0; index < argNames.Length; index++) {
                    var arg = argNames[index];
                    StringBuilder.Append(arg);
                    if (index != argNames.Length - 1) {
                        StringBuilder.Append(",");
                    }
                }
            }
            StringBuilder.Append(") ");

            if (!string.IsNullOrEmpty(logSymbolData.ValidArgNames)) {
                var validArgNames = logSymbolData.ValidArgNames.Split(',');
                for (var i = 0; i < validArgNames.Length; i++) {
                    if (i >= args.Count) { continue; }

                    StringBuilder.Append(validArgNames[i]);
                    StringBuilder.Append("=");
                    if (validArgNames[i].Contains("FP")) {
                        StringBuilder.Append((float)FP.FromRaw(args[i]));
                    }
                    else {
                        StringBuilder.Append(args[i]);
                    }
                    StringBuilder.Append("; ");
                }
            }

            StringBuilder.Append(logSymbolData.Comments);
            return StringBuilder.ToString();
        }
    }
}
#endif