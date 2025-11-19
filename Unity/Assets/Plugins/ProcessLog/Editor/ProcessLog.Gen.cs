#if UNITY_EDITOR
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ProcessLog.Editor
{
    public partial class ProcessLog
    {
        private static UTF8Encoding Utf8Encoding = new UTF8Encoding(false, true);

        //匹配类名
        private static Regex ms_regexClassName = new Regex(@"(?<=class)\s+\w+(?=[\s\n]*{)");

        //整个函数的匹配模式串
        private static Regex ms_regexFuncAll = new Regex(@"(public|private|protected|internal)((\s+(static|override|virtual)*\s+)|\s+)\w+(<\w+>)*(\[\])*\s+\w+(<\w+>)*\s*\(([^\)]+\s*)?\)\s*\{[^\{\}]*(((?'Open'\{)[^\{\}]*)+((?'-Open'\})[^\{\}]*)+)*(?(Open)(?!))\}");

        //函数头的匹配模式串
        private static Regex ms_regexFuncHead = new Regex(@"(public|private|protected|internal)((\s+(static|override|virtual)*\s+)|\s+)\w+(<\w+>)*(\[\])*\s+\w+(<\w+>)*\s*\(([^\)]+\s*)?\)");

        //匹配函数头之后的第一个大括号模式串
        private static Regex ms_regexLeftBrace = new Regex(@"\{");

        //第一句代码模式串
        private static Regex ms_regexFirstCode = new Regex(@"(\{\s*[^;]+;)(\s*\/\*(.)*\*\/)?");

        //插入的日志代码模式串
        private static Regex ms_regexLogCode = new Regex(@"\s*(self.LSRoom\(\)\?.ProcessLog.)(LogFunction)\(([^;]+\s*)?\)(\})?\s*;");

        //匹配日志代码的id,
        private static Regex ms_regexLogCodeId = new Regex(@"(?<=\(\s*)\s*(\d+)\s*");

        //不需要日志代码模式串
        private static Regex ms_regexIgnoreCode = new Regex(@"\{\s*(self.LSRoom\(\)\?.ProcessLog.)(LogIgnore)\(([^\)]+\s*)?\)(\})?");

        //匹配手动插入的日志代码
        //self.LSRoom()?.ProcessLog.LogFunction(0, ...);   /* comment */
        private static Regex ms_regexManualCode = new Regex(@"(?:\s*.LSRoom\(\)\?.ProcessLog.)(?:LogFunction)\(([^;]+\s*)?\)(?:\})?\s*;\s*\/\*(.)*\*\/");

        //匹配自动插入的日志代码
        private static Regex ms_regexAutoCode = new Regex(@"\s*(?:self.LSRoom\(\)\?.ProcessLog.)(?:LogFunction)\((?:[^\)]+\s*)?\)(\})?\s*;\s*\#(\w*)\#");
        
        //匹配函数的名称
        private static Regex ms_regexFuncName = new Regex(@"(\w+(<\w+>)*\s*)\(");
        
        //匹配所有的函数参数类型以及名称
        private static Regex ms_regexAllArgTypeAndName = new Regex(@"((?:\w|\d|_|<[^<]*>|(?:out\s*)|\[|\]|\,)+)\s*([\w|_|\d]+)\s*(?:=\s*[\w|\d]+)*(?:,|\))");
        
        private static void ResolveAutoLog(string fullPath, LogSymbolFile symbolFile)
        {
            var fileName = Path.GetFileName(fullPath);
            if (ProcessLogSetting.IgnoreFileName(fileName)) {
                return;
            }

            var text = File.ReadAllText(fullPath, Utf8Encoding);
            var matches = ms_regexFuncAll.Matches(text);
            for (var i = matches.Count - 1; i >= 0; i--) {
                var matchFuncAll = matches[i];

                var matchFuncHead = ms_regexFuncHead.Match(text, matchFuncAll.Index, matchFuncAll.Length);
                var funcHeadInfo = GetFunctionData(matchFuncHead.Value);
                if (ProcessLogSetting.IgnoreFunctionName(funcHeadInfo.FuncName)) {
                    continue;
                }
                funcHeadInfo.FileName = fileName;

                //匹配到函数的左大括号
                var matchLeftBrace = ms_regexLeftBrace.Match(text, matchFuncAll.Index, matchFuncAll.Length);
                if (matchLeftBrace.Success) {
                    int len = matchFuncAll.Index + matchFuncAll.Length - (matchLeftBrace.Index + matchLeftBrace.Length);
                    
                    //找到第一行代码
                    var matchFirstCode = ms_regexFirstCode.Match(text, matchLeftBrace.Index, len);
                    if (matchFirstCode.Success) {
                        if (ms_regexIgnoreCode.IsMatch(matchFirstCode.Value) || ms_regexManualCode.IsMatch(matchFirstCode.Value)) {
                            continue;
                        }
                        
                        // 首行代码为日志代码
                        var codeMatch = ms_regexLogCode.Match(matchFirstCode.Value);
                        if (codeMatch.Success) {
                            text = text.Remove(matchFirstCode.Index + codeMatch.Index, codeMatch.Length);
                            var args = codeMatch.Groups[3].Value;
                            var id = (ushort)int.Parse(args.Contains(",") ? args.Substring(0, args.IndexOf(',')).Trim() : args);
                            
                            if (id > 0 && !symbolFile.CheckExist(id)) {
                                symbolFile.AddSymbolData(id, funcHeadInfo);
                                funcHeadInfo.LogCode = ReplaceID(funcHeadInfo.LogCode, id);
                                text = text.Insert(matchLeftBrace.Index + matchLeftBrace.Length, funcHeadInfo.LogCode);
                            }
                            else {
                                var logID = symbolFile.AddSymbolData(funcHeadInfo);
                                funcHeadInfo.LogCode = ReplaceID(funcHeadInfo.LogCode, logID);
                                text = text.Insert(matchLeftBrace.Index + matchLeftBrace.Length, funcHeadInfo.LogCode);
                            }
                        }
                        else {
                            var logID = symbolFile.AddSymbolData(funcHeadInfo);
                            funcHeadInfo.LogCode = ReplaceID(funcHeadInfo.LogCode, logID);
                            text = text.Insert(matchLeftBrace.Index + matchLeftBrace.Length, funcHeadInfo.LogCode);
                        }
                    }
                }
            }
            File.WriteAllText(fullPath, text, Utf8Encoding);
        }
        
        private static void ResolveManualLog(string fullPath, LogSymbolFile symbolFile)
        {
            var fileName = Path.GetFileName(fullPath);

            var hasModify = false;
            var fullText = File.ReadAllText(fullPath, Utf8Encoding);
            var matchCollection = ms_regexFuncAll.Matches(fullText);
            for (var i = 0; i < matchCollection.Count; i++) {
                var funcMatch = matchCollection[i];
                
                var matchFuncHead = ms_regexFuncHead.Match(fullText, funcMatch.Index, funcMatch.Length);
                var functionData = GetFunctionData(matchFuncHead.Value);
                functionData.FileName = fileName;

                // 匹配手动插入的日志
                var manualMatches = ms_regexManualCode.Matches(funcMatch.Value);
                for (var j = 0; j < manualMatches.Count; j++) {
                    var handInsertCodeMatch = manualMatches[j];
                    
                    var args = handInsertCodeMatch.Groups[1].Value;
                    var comment = handInsertCodeMatch.Groups[2].Value;
                    var id = (ushort) 0;
                    
                    if (args.Contains(",")) {
                        id = (ushort)int.Parse(args.Substring(0, args.IndexOf(',')).Trim());
                        args = args.Remove(0, args.IndexOf(',') + 1).Trim();
                        functionData.ValidArgNames = args;
                        functionData.ValidArgCount = args.Split(',').Length;
                    }
                    else {
                        id = (ushort)int.Parse(args);
                        functionData.ValidArgNames = null;
                        functionData.ValidArgCount = 0;
                    }

                    functionData.Comment = comment;
                    functionData.Type = 1;
                    if (id > 0 && !symbolFile.CheckExist(id)) {
                        symbolFile.AddSymbolData(id, functionData);
                    }
                    else {
                        var logID = symbolFile.AddSymbolData(functionData);
                        fullText = ReplaceID(fullText, funcMatch.Index + manualMatches[j].Index, manualMatches[j].Length, logID);
                        hasModify = true;
                    }
                }
            }
            if (hasModify)
                File.WriteAllText(fullPath, fullText, Utf8Encoding);
        }

        /// <summary>
        /// 根据函数头 自动生成日志代码
        /// 只提取int 和 Fix类型的参数
        /// </summary>
        /// <param name="funcHead"></param>
        private static LogFunctionData GetFunctionData(string funcHead)
        {
            var codeText = "self.LSRoom()?.ProcessLog.LogFunction(0, self.LSParent().Id);";
            var functionData = new LogFunctionData();
            
            functionData.ValidArgNames += "Owner,";
            functionData.ValidArgCount++;
            
            //1. 提取函数名
            var funcNameMatch = ms_regexFuncName.Match(funcHead);
            if (funcNameMatch.Success) {
                functionData.FuncName = funcNameMatch.Groups[1].Value;
            }

            //2. 提取所有的参数类型以及参数名称, 同时顺便生成日志代码
            var allArgMatch = ms_regexAllArgTypeAndName.Matches(funcHead);
            for (var i = 0; i < allArgMatch.Count; i++) {
                var match = allArgMatch[i];
                var type = match.Groups[1].Value;
                var name = match.Groups[2].Value;
                
                functionData.ArgTypes += type;
                functionData.ArgNames += name;
                if (i < allArgMatch.Count - 1) {
                    functionData.ArgTypes += ",";
                    functionData.ArgNames += ",";
                }

                if (type.Equals("int")) {
                    codeText = codeText.Insert(codeText.Length - 2, $", {name}");
                    functionData.ValidArgNames += name + ",";
                    functionData.ValidArgCount++;
                }
                else if (type.Equals("FP")) {
                    codeText = codeText.Insert(codeText.Length - 2, $", {name}.V");
                    functionData.ValidArgNames += "(FP)" + name + ",";
                    functionData.ValidArgCount++;
                }
                else if (type.Equals("bool")) {
                    codeText = codeText.Insert(codeText.Length - 2, $", {name} ? 1 : 0");
                    functionData.ValidArgNames += name + ",";
                    functionData.ValidArgCount++;
                }
                else if (type.Equals("TSVector2")) {
                    codeText = codeText.Insert(codeText.Length - 2, $", {name}.x.V, {name}.y.V");
                    functionData.ValidArgNames += $"(FP){name}.X,(FP){name}.Y,";
                    functionData.ValidArgCount += 2;
                }
                else if (type.Equals("TSVector")) {
                    codeText = codeText.Insert(codeText.Length - 2, $", {name}.x.V, {name}.y.V, {name}.z.V");
                    functionData.ValidArgNames += $"(FP){name}.X, (FP){name}.Y,(FP){name}.Z,";
                    functionData.ValidArgCount += 3;
                }
                else if (type.Equals("LSUnit")) {
                    codeText = codeText.Insert(codeText.Length - 2, $", {name}.Id");
                    functionData.ValidArgNames += $"{name}.Id,";
                    functionData.ValidArgCount++;
                }
                else if (ProcessLogSetting.EnumConfig.Contains(type)) {
                    codeText = codeText.Insert(codeText.Length - 2, $", (int){name}");
                    functionData.ValidArgNames += name + ",";
                    functionData.ValidArgCount++;
                }
            }

            if (!string.IsNullOrEmpty(functionData.ValidArgNames)) {
                functionData.ValidArgNames = functionData.ValidArgNames.Remove(functionData.ValidArgNames.Length - 1, 1);
            }

            functionData.LogCode = codeText;
            return functionData;
        }

        /// <summary>
        /// 替换掉日志代码中的id字段
        /// </summary>
        /// <param name="logTrackCode"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static string ReplaceID(string logTrackCode, ushort id)
        {
            var idMatch = ms_regexLogCodeId.Match(logTrackCode);
            if (idMatch.Success) {
                logTrackCode = logTrackCode.Remove(idMatch.Index, idMatch.Length);
                logTrackCode = logTrackCode.Insert(idMatch.Index, id.ToString());
            }

            return logTrackCode;
        }

        /// <summary>
        /// 替换日志代码中的id
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="start">匹配起始位置</param>
        /// <param name="end">匹配结束位置</param>
        /// <param name="id"></param>
        /// <returns></returns>
        private static string ReplaceID(string text, int start, int end, ushort id)
        {
            var idMatch = ms_regexLogCodeId.Match(text, start, end);
            if (idMatch.Success) {
                text = text.Remove(idMatch.Index, idMatch.Length);
                text = text.Insert(idMatch.Index, id.ToString());
            }

            return text;
        }
    }
}
#endif