using System.Diagnostics;

namespace NPBehave
{
    public static class NPLog
    {
        private const int TraceLevel = 1;
        private const int DebugLevel = 2;
        private const int InfoLevel = 3;
        private const int WarningLevel = 4;
        
        [Conditional("DEBUG")]
        public static void Debug(string msg)
        {
        }
        
        [Conditional("DEBUG")]
        public static void Trace(string msg)
        {

        }

        public static void Info(string msg)
        {

        }

        public static void TraceInfo(string msg)
        {

        }

        public static void Warning(string msg)
        {

        }

        public static void Error(string msg)
        {

        }

        public static void Error(System.Exception e)
        {
        }
        
        public static void Console(string msg)
        {
        }
    }
}
