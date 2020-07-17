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
        Boolean ConfigureLogger(String LogFileName, long MaxFileSize = 10, LoggerOutputTypes loggerOutputType = LoggerOutputTypes.File);
        Boolean ConfigureLogger(String LogFileName, LoggerOutputTypes loggerOutputType = LoggerOutputTypes.File);
        Boolean ConfigureLogger();
        Boolean Changefile(String LogFileName);
        void CreateTag(String tag);
        void RegisterLogHandler(Action<DateTime, int?, String, String, String, int, String, String> handler);
        void UnRegisterLogHandler(Action<DateTime, int?, String, String, String, int, String, String> handler);

        // Legacy, backwards compatable
        void SetLogLevel(BASICLOGGERLEVELS logLevel);
        void SetLogLevel(BASICLOGGERLEVELS logLevel, IBasicLoggerTag tag);
        BASICLOGGERLEVELS GetLogLevel();
        BASICLOGGERLEVELS GetLogLevel(IBasicLoggerTag tagged);

        // New Format
        void SetLogLevel(BasicLoggerLogLevels logLevel);
        void SetLogLevel(BasicLoggerLogLevels logLevel, IBasicLoggerTag tag);
        Boolean SetLogLevel(String loglevel);
        Boolean SetLogLevel(String loglevel, IBasicLoggerTag tagged);
        BasicLoggerLogLevels CurrentLogLevel();
        BasicLoggerLogLevels CurrentLogLevel(IBasicLoggerTag tagged);

        // Force functions
        void Force(BasicLoggerLogLevels logLevel, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Force(BasicLoggerLogLevels logLevel, IBasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Force(BasicLoggerLogLevels logLevel, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Force(BasicLoggerLogLevels logLevel, IBasicLoggerTag tag, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        // Trace functions
        void Trace(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Trace(IBasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Trace(BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Trace(IBasicLoggerTag tag, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);


        // Debug functions
        void Debug(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Debug(IBasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Debug(BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Debug(IBasicLoggerTag tag, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        // Information functions
        void Info(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Info(IBasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Information(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Information(IBasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Information(BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Information(IBasicLoggerTag tag, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        // Warn functions
        void Warn(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Warn(IBasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Warning(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Warning(IBasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Warning(BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Warning(IBasicLoggerTag tag, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        // Error functions
        void Error(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Error(IBasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Error(BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Error(IBasicLoggerTag tag, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        // Fatal functions
        void Fatal(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Fatal(IBasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Fatal(BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Fatal(IBasicLoggerTag tag, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0);

        void Raw(String logString);
    }
}
