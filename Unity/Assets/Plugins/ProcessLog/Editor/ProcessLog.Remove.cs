#if UNITY_EDITOR
using System.IO;

namespace ProcessLog.Editor
{
    public partial class ProcessLog
    {
        private static void RemoveAutoCode(string fullSubPath)
        {
            bool hasModify = false;
            var text = File.ReadAllText(fullSubPath, Utf8Encoding);
            var matches = ms_regexFuncAll.Matches(text);
            for (var i = matches.Count - 1; i >= 0; i--)
            {
                var matchFuncAll = matches[i];
                
                // 匹配函数的左大括号
                var matchLeftBrace = ms_regexLeftBrace.Match(text, matchFuncAll.Index, matchFuncAll.Length);
                if (matchLeftBrace.Success) {
                    int len = matchFuncAll.Index + matchFuncAll.Length - (matchLeftBrace.Index + matchLeftBrace.Length);
                    
                    // 找到第一行代码
                    var matchFirstCode = ms_regexFirstCode.Match(text, matchLeftBrace.Index, len);
                    if (matchFirstCode.Success) {
                        if (ms_regexIgnoreCode.IsMatch(matchFirstCode.Value) || ms_regexManualCode.IsMatch(matchFirstCode.Value))
                            continue;
                        
                        // 首行代码为日志代码，删掉
                        var logTrackCodeMatch = ms_regexLogCode.Match(matchFirstCode.Value);
                        if (logTrackCodeMatch.Success) {
                            text = text.Remove(matchFirstCode.Index + logTrackCodeMatch.Index, logTrackCodeMatch.Length);
                            hasModify = true;
                        }
                    }
                }
            }

            if (hasModify)
                File.WriteAllText(fullSubPath, text, Utf8Encoding);
        }
    }
}
#endif