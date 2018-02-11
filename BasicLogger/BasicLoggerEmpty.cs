using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Peamel.BasicLogger
{
    /// <summary>
    /// The intent of this class, is so that when a class will be passed a pointer to a logger
    /// it will likely in have all kinds of logging. It should use this Empty class first
    /// So that it doesn't need to do a bunch of checks if a lgger has been created for it yet
    /// </summary>
    public class EmptyLogger : ILogger
    {
        public Boolean ConfigureLogger(String LogFileName, long MaxFileSize = 10)
        {
            return false;
        }

        public void CreateTag(String tag)
        {
            return;
        }


        public void SetLogLevel(BASICLOGGERLEVELS loglevel)
        {
            return;
        }

        public void SetLogLevel(BASICLOGGERLEVELS loglevel, String tag)
        {
            return;
        }

        public BASICLOGGERLEVELS GetLogLevel(String tag = "DEFAULT")
        {
            return BASICLOGGERLEVELS.OFF;
        }

        public Boolean Changefile(String LogFileName)
        {
            return false;
        }

        public void Trace(String logstring, String tag,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Debug(String logstring, String tag ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Info(String logstring, String tag,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Warn(String logstring, String tag,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Error(String logstring, String tag,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Fatal(String logstring, String tag,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Raw(String logString)
        {
            return;
        }
    }
}
