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

        public void SetLogLevel(BASICLOGGERLEVELS loglevel, IBasicLoggerTag tag)
        {
            return;
        }

        public BASICLOGGERLEVELS GetLogLevel()
        {
            return BASICLOGGERLEVELS.OFF;
        }

        public BASICLOGGERLEVELS GetLogLevel(IBasicLoggerTag tagged)
        {
            return BASICLOGGERLEVELS.OFF;
        }

        public void SetLogLevel(BasicLoggerLogLevels loglevel)
        {
            return;
        }

        public void SetLogLevel(BasicLoggerLogLevels loglevel, IBasicLoggerTag tag)
        {
            return;
        }

        public BasicLoggerLogLevels CurrentLogLevel()
        {
            return BasicLoggerLogLevels.None;
        }

        public BasicLoggerLogLevels CurrentLogLevel(IBasicLoggerTag tagged)
        {
            return BasicLoggerLogLevels.None;
        }

        public Boolean Changefile(String LogFileName)
        {
            return false;
        }

        #region Trace
        public void Trace(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Trace(IBasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Trace(BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Trace(IBasicLoggerTag tag, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }
        #endregion Trace

        #region Debug
        public void Debug(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Debug(IBasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Debug(BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Debug(IBasicLoggerTag tag, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }
        #endregion Debug

        #region Information
        public void Info(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Info(IBasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Information(BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Information(IBasicLoggerTag tag, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }
        #endregion Information

        #region Warning
        public void Warn(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Warn(IBasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Warning(BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Warning(IBasicLoggerTag tag, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }
        #endregion Warning

        #region Error
        public void Error(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Error(IBasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Error(BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Error(IBasicLoggerTag tag, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }
        #endregion Error

        #region Fatal
        public void Fatal(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Fatal(IBasicLoggerTag tag, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Fatal(BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }

        public void Fatal(IBasicLoggerTag tag, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            return;
        }
        #endregion Error

        public void Raw(String logString)
        {
            return;
        }
    }
}
