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
        void SetLogLevel(BASICLOGGERLEVELS loglevel, String tag);
        BASICLOGGERLEVELS GetLogLevel(String tag = "DEFAULT");

        void Trace(String logstring, String tag = "DEFAULT",
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void Debug(String logstring, String tag = "DEFAULT" ,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void Info(String logstring, String tag = "DEFAULT",
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void Warn(String logstring, String tag = "DEFAULT",
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void Error(String logstring, String tag = "DEFAULT",
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void Fatal(String logstring, String tag = "DEFAULT",
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);
        void Raw(String logString);

    }
}
