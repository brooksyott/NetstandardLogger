using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Peamel.BasicLogger
{
    public interface ILogger
    {
        Boolean ConfigureLogger(String LogFileName, long MaxFileSize = 10);
        Boolean Changefile(String LogFileName);
        void CreateTag(String tag);

        void SetLogLevel(BASICLOGGERLEVELS loglevel);
        void SetLogLevel(BASICLOGGERLEVELS loglevel, BasicLoggerTag tag);
        BASICLOGGERLEVELS GetLogLevel();
        BASICLOGGERLEVELS GetLogLevel(BasicLoggerTag tagged);

        void Trace(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void Debug(String logstring,
                [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void Info(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void Warn(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void Error(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void Fatal(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Trace(BasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void Debug(BasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void Info(BasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void Warn(BasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void Error(BasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void Fatal(BasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void Raw(String logString);

    }
}
