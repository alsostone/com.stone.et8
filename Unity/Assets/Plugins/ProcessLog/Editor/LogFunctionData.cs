#if UNITY_EDITOR
namespace ProcessLog.Editor
{
    public class LogFunctionData
    {
        public string LogCode;              //生成的插入函数头的代码
        public int Type;                    //0：自动插入； 1：手动插入
        
        public int ID;                      //日志ID
        public string FileName;             //文件名
        public string FuncName;             //函数名
        public string ArgTypes;             //参数类型 逗号分隔
        public string ArgNames;             //参数名称 逗号分隔
        public int ValidArgCount;           //有效参数个数
        public string ValidArgNames;        //有效参数名称 逗号分隔
        public string Comment;              //手动插入代码注释
    }
}
#endif