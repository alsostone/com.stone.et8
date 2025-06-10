#if UNITY_EDITOR
using System.Collections.Generic;

namespace ProcessLog.Editor
{
    public class LogSymbolData
    {
        public ushort ID { get; set; }              //日志ID
        public string FileName { get; set; }        //子路径
        public string FuncName { get; set; }        //函数名
        public string ArgTypes { get; set; }        //参数类型
        public string ArgNames { get; set; }        //参数名字
        public string ValidArgNames { get; set; }   //有效参数名字
        public int ValidArgCount { get; set; }      //有效参数个数
        public string Comments { get; set; }        //手动插入日志注释

        public Dictionary<string, object> ToDictionary()
        {
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("ID", ID);
            dictionary.Add("FileName", FileName);
            dictionary.Add("FuncName", FuncName);
            dictionary.Add("ArgTypes", ArgTypes);
            dictionary.Add("ArgNames", ArgNames);
            dictionary.Add("ValidArgNames", ValidArgNames);
            dictionary.Add("ValidArgCount", ValidArgCount);
            dictionary.Add("Comments", Comments);
            return dictionary;
        }
    }
}
#endif