using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Runtime.CompilerServices;
using System.IO;
using System.Threading;

// Requires 
namespace Peamel.BasicLogger
{
    public enum LoggerOutputTypes
    {
        File,
        Console,
        Both
    };

    /// <summary>
    /// The logger class writes logs to a file, and will create and .old.log file when rotating based
    /// on the maximum allowed file size.
    /// The TAG allows the caller to setup log levels specific to a tag, but they all go 
    /// to the same log file (for now)
    /// </summary>
    public class Logger : ILogger, IDisposable
    {
        const String InformationLevel = "INFO";
        const String DebugLevel = "DEBUG";
        const String WarningLevel = "WARN";
        const String TraceLevel = "TRACE";
        const String ErrorLevel = "ERROR";
        const String FatalLevel = "FATAL";

        public const String DEFAULT = "DEFAULT";

        static public int LoggerCount { get; set; }

        private string DatetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        private const string Title = "Date Time\tThread\tLevel\tFile\tMethod\tLine\tEvent\tLog";
        private string _fileName;
        private Object _logLock = new Object();
        private long _maxFileSize = 0;
        private FileInfo _logFileInfo = null;
        private IBasicLoggerTag _defaultTag = new BasicLoggerTag(DEFAULT);

        private LoggerOutputTypes _loggerOutputType = LoggerOutputTypes.File;


        Action<DateTime, int?, String, String, String, int, String, String> messageTarget;

        public void RegisterLogHandler(Action<DateTime, int?, String, String, String, int, String, String> handler)
        {
            messageTarget += handler;
        }

        public void UnRegisterLogHandler(Action<DateTime, int?, String, String, String, int, String, String> handler)
        {
            messageTarget -= handler;
        }

        // Keeps tracks of the log levels based on the tag
        private Dictionary<String, LoggerLevels> _tagLogLevel;

        private StreamWriter _logStreamWriter;

        /// <summary>
        /// Configures the logger
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="LogFileName"></param>
        /// <param name="MaxFileSize">FileSize to rotate in Megabytes</param>
        /// <returns></returns>
        public Boolean ConfigureLogger(String LogFileName, LoggerOutputTypes loggerOutputType = LoggerOutputTypes.File)
        {
            _loggerOutputType = loggerOutputType;
            return ConfigureLogger(LogFileName, 10);
        }

        /// <summary>
        /// Configures the logger to type console
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="LogFileName"></param>
        /// <param name="MaxFileSize">FileSize to rotate in Megabytes</param>
        /// <returns></returns>
        public Boolean ConfigureLogger()
        {
            _loggerOutputType = LoggerOutputTypes.Console;
            Raw(Title);
            return true;
        }

        /// <summary>
        /// Configures the logger
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="LogFileName"></param>
        /// <param name="MaxFileSize">FileSize to rotate in Megabytes</param>
        /// <returns></returns>
        public Boolean ConfigureLogger(String LogFileName, long MaxFileSize = 10)
        {
            return ConfigureLogger(LogFileName, MaxFileSize, LoggerOutputTypes.File);
        }

        /// <summary>
        /// Configures the logger
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="LogFileName"></param>
        /// <param name="MaxFileSize">FileSize to rotate in Megabytes</param>
        /// <returns></returns>
        public Boolean ConfigureLogger(String LogFileName, long MaxFileSize = 10, LoggerOutputTypes loggerOutputType = LoggerOutputTypes.File)
        {
            _loggerOutputType = loggerOutputType;

            Boolean loggerSuccess = true;

            if (String.IsNullOrEmpty(LogFileName)) return false;

            _fileName = LogFileName;

            if (!File.Exists(LogFileName))
            {
                lock (_logLock)
                {
                    using (var s = File.Create(_fileName))
                    {
                    }
                }
            }

            _logFileInfo = new FileInfo(_fileName);
            _logStreamWriter = File.AppendText(_fileName);
            _logStreamWriter.AutoFlush = true;

            // Convert Megabytes to Bytes
            _maxFileSize = MaxFileSize * 1024 * 1024;

            if (loggerSuccess == true)
            {
                Raw(Title);
            }

            return loggerSuccess;
        }

        /// <summary>
        /// Configures the logger
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="LogFileName"></param>
        /// <param name="MaxFileSize">FileSize to rotate in Megabytes</param>
        /// <returns></returns>
        public Boolean Changefile(String LogFileName)
        {
            Boolean loggerSuccess = true;

            if (String.IsNullOrEmpty(LogFileName)) return false;

            _fileName = LogFileName;

            if (!File.Exists(LogFileName))
            {
                lock (_logLock)
                {
                    if (_logStreamWriter != null)
                    {
                        _logStreamWriter.Flush();
                        _logStreamWriter.Dispose();
                    }

                    using (var f = File.Create(_fileName))
                    {
                    }

                    _logStreamWriter = File.AppendText(_fileName);
                    _logStreamWriter.AutoFlush = true;
                }
            }

            return loggerSuccess;
        }

        public Logger(String LogFileName, long MaxFileSize = 1, LoggerOutputTypes loggerOutputType = LoggerOutputTypes.File)
        {
            Boolean rc = ConfigureLogger(LogFileName, MaxFileSize, loggerOutputType);

            // Set up the default log tag and log level (INFO LEVEL)
            _tagLogLevel = new Dictionary<string, LoggerLevels>();
            SetLogLevel(BasicLoggerLogLevels.Information, _defaultTag);
        }

        public Logger(String LogFileName, LoggerOutputTypes loggerOutputType = LoggerOutputTypes.File)
        {
            Boolean rc = ConfigureLogger(LogFileName, 10, loggerOutputType);

            // Set up the default log tag and log level (INFO LEVEL)
            _tagLogLevel = new Dictionary<string, LoggerLevels>();
            SetLogLevel(BasicLoggerLogLevels.Information, _defaultTag);
        }

        public Logger()
        {

            Boolean rc = ConfigureLogger();

            LoggerCount++;

            // Set up the default log tag and log level (INFO LEVEL)
            _tagLogLevel = new Dictionary<string, LoggerLevels>();
            SetLogLevel(BasicLoggerLogLevels.Information, _defaultTag);
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        ~Logger()
        {
            if (!this.IsDisposed)
            {
                Dispose(false);
            }
        }

        /// <summary>
        /// Disposes this instance
        /// </summary>
        void IDisposable.Dispose()
        {
            if (!this.IsDisposed)
            {
                try
                {
                    Dispose(true);
                }
                catch
                {
                }

                this.IsDisposed = true;
                GC.SuppressFinalize(this);  // instructs GC not bother to call the destructor                
            }
        }

        /* protected */
        /// <summary>
        /// Disposes the options change toker. IDisposable pattern implementation.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            LoggerCount--;
        }

        public bool IsDisposed { get; protected set; }

        public void CreateTag(String tag)
        {
            if (_tagLogLevel.ContainsKey(tag) == false)
            {
                // If there is no tag, create it
                _tagLogLevel[tag] = new LoggerLevels();
            }
        }

        public void SetLogLevel(BASICLOGGERLEVELS loglevel)
        {
            SetLogLevel(loglevel, _defaultTag);
        }

        public void SetLogLevel(BASICLOGGERLEVELS loglevel, IBasicLoggerTag tagged)
        {
            String tag = tagged.GetName();

            if (_tagLogLevel.ContainsKey(tag) == false)
            {
                // If there is no tag, create it
                _tagLogLevel[tag] = new LoggerLevels();
            }
            _tagLogLevel[tag].SetLogLevel(loglevel);
        }

        public void SetLogLevel(BasicLoggerLogLevels loglevel)
        {
            SetLogLevel(loglevel, _defaultTag);
        }

        public BASICLOGGERLEVELS GetLogLevel()
        {
            return GetLogLevel(_defaultTag);
        }

        public void SetLogLevel(BasicLoggerLogLevels loglevel, IBasicLoggerTag tagged)
        {
            String tag = tagged.GetName();

            if (_tagLogLevel.ContainsKey(tag) == false)
            {
                // If there is no tag, create it
                _tagLogLevel[tag] = new LoggerLevels();
            }
            _tagLogLevel[tag].SetLogLevel(loglevel);
        }

        public Boolean SetLogLevel(String loglevel)
        {
            return SetLogLevel(loglevel, _defaultTag);
        }

        public Boolean SetLogLevel(String loglevel, IBasicLoggerTag tagged)
        {
            String tag = tagged.GetName();

            if (_tagLogLevel.ContainsKey(tag) == false)
            {
                // If there is no tag, create it
                _tagLogLevel[tag] = new LoggerLevels();
            }
            return _tagLogLevel[tag].SetLogLevel(loglevel);
        }

        public BasicLoggerLogLevels CurrentLogLevel()
        {
            String ttag = _defaultTag.GetName();
            return _tagLogLevel[ttag].CurrentLogLevel();
        }

        public BasicLoggerLogLevels CurrentLogLevel(IBasicLoggerTag tag)
        {
            String ttag = tag.GetName();

            if (_tagLogLevel.ContainsKey(ttag) == false)
            {
                ttag = _defaultTag.GetName();
            }

            return _tagLogLevel[ttag].CurrentLogLevel();
        }

        public BASICLOGGERLEVELS GetLogLevel(IBasicLoggerTag tagged)
        {
            String tag = tagged.GetName();

            if (_tagLogLevel.ContainsKey(tag) == false)
            {
                // If there is no tag, create it
                return _tagLogLevel[tag].GetLogLevel();
            }
            return BASICLOGGERLEVELS.OFF;
        }

        #region IgnoreLogLevel
        public void Force(BasicLoggerLogLevels logLevel, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Force(logLevel, _defaultTag, eventId, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Force(BasicLoggerLogLevels logLevel, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Force(logLevel, _defaultTag, null, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Force(BasicLoggerLogLevels logLevel, IBasicLoggerTag tagged, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Force(logLevel, tagged, null, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Force(BasicLoggerLogLevels logLevel, IBasicLoggerTag tagged, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            String logLevelString = String.Empty;
            switch(logLevel)
            {
                case BasicLoggerLogLevels.Debug:
                    {
                        logLevelString = DebugLevel;
                        break;
                    }
                case BasicLoggerLogLevels.Error:
                    {
                        logLevelString = ErrorLevel;
                        break;
                    }
                case BasicLoggerLogLevels.Fatal:
                    {
                        logLevelString = FatalLevel;
                        break;
                    }
                case BasicLoggerLogLevels.Information:
                    {
                        logLevelString = InformationLevel;
                        break;
                    }
                case BasicLoggerLogLevels.Trace:
                    {
                        logLevelString = TraceLevel;
                        break;
                    }
                case BasicLoggerLogLevels.Warning:
                    {
                        logLevelString = WarningLevel;
                        break;
                    }
                default:
                    {
                        logLevelString = "N/A";
                        break;
                    }
            }

            WriteFormattedLog(logLevelString, logstring, eventId, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion IgnoreLogLevel

        #region Trace
        // This is used if the user specificies a tag to be used
        public void Trace(BasicLoggerEventId eventId, String logstring, 
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Trace(_defaultTag, eventId, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Trace(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Trace(_defaultTag, null, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Trace(IBasicLoggerTag tagged, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Trace(tagged, null, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Trace(IBasicLoggerTag tagged, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            String tag = tagged.GetName();

            // If the tag does not exist, then we use the default loggin level
            if (_tagLogLevel.ContainsKey(tag) == false)
            {
                if (_tagLogLevel[DEFAULT].TraceOn == false)
                    return;
            }
            else
            {
                // The tag exists, check if the desired level is on
                if (_tagLogLevel[tag].TraceOn == false)
                {
                    return;
                }
            }

            WriteFormattedLog(TraceLevel, logstring, eventId, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion Trace

        #region Debug
        // This is used if the user specificies a tag to be used
        public void Debug(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Debug(_defaultTag, null, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Debug(IBasicLoggerTag tagged, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Debug(tagged, null, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Debug(BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Debug(_defaultTag, eventId, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Debug(IBasicLoggerTag tagged, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            String tag = tagged.GetName();

            // If the tag does not exist, then we use the default loggin level
            if (_tagLogLevel.ContainsKey(tag) == false)
            {
                if (_tagLogLevel[DEFAULT].DebugOn == false)
                    return;
            }
            else
            {
                // The tag exists, check if the desired level is on
                if (_tagLogLevel[tag].DebugOn == false)
                {
                    return;
                }
            }

            WriteFormattedLog(DebugLevel, logstring, eventId, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion Debug

        #region Information
        public void Info(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Information(_defaultTag, null, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Info(IBasicLoggerTag tagged, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Information(tagged, null, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        public void Information(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Information(_defaultTag, null, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Information(IBasicLoggerTag tagged, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Information(tagged, null, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Information(BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Information(_defaultTag, eventId, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Information(IBasicLoggerTag tagged, BasicLoggerEventId eventId, String logstring, 
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            // If the tag does not exist, then we use the default loggin level
            Stopwatch sw = new Stopwatch();
            sw.Start();
            String tag = tagged.GetName();
            if (_tagLogLevel.ContainsKey(tag) == false)
            {
                if (_tagLogLevel[DEFAULT].InfoOn == false)
                    return;
            }
            else
            {
                // The tag exists, check if the desired level is on
                if (_tagLogLevel[tag].InfoOn == false)
                {
                    return;
                }
            }
            WriteFormattedLog(InformationLevel, logstring, eventId, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion Information

        #region Warning
        public void Warn(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Warning(_defaultTag, null, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Warn(IBasicLoggerTag tagged, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Warning(tagged, null, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        public void Warning(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Warning(_defaultTag, null, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Warning(IBasicLoggerTag tagged, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Warning(tagged, null, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Warning(BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Warning(_defaultTag, eventId, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        public void Warning(IBasicLoggerTag tagged, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            // If the tag does not exist, then we use the default loggin level
            String tag = tagged.GetName();
            if (_tagLogLevel.ContainsKey(tag) == false)
            {
                if (_tagLogLevel[DEFAULT].WarnOn == false)
                    return;
            }
            else
            {
                // The tag exists, check if the desired level is on
                if (_tagLogLevel[tag].WarnOn == false)
                {
                    return;
                }
            }

            WriteFormattedLog(WarningLevel, logstring, eventId, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion Warning

        #region Error
        public void Error(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Error(_defaultTag, null, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Error(IBasicLoggerTag tagged, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Error(tagged, null, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Error(BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Error(_defaultTag, eventId, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        public void Error(IBasicLoggerTag tagged, BasicLoggerEventId eventId, String logstring,
               [CallerMemberName] string memberName = "",
               [CallerFilePath] string sourceFilePath = "",
               [CallerLineNumber] int sourceLineNumber = 0)
        {
            // If the tag does not exist, then we use the default loggin level
            String tag = tagged.GetName();
            if (_tagLogLevel.ContainsKey(tag) == false)
            {
                if (_tagLogLevel[DEFAULT].ErrorOn == false)
                    return;
            }
            else
            {
                // The tag exists, check if the desired level is on
                if (_tagLogLevel[tag].ErrorOn == false)
                {
                    return;
                }
            }

            WriteFormattedLog(ErrorLevel, logstring, eventId, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion Error

        #region Fatal
        public void Fatal(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Fatal(_defaultTag, null, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Fatal(IBasicLoggerTag tagged, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Fatal(tagged, null, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Fatal(BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Fatal(_defaultTag, eventId, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        public void Fatal(IBasicLoggerTag tagged, BasicLoggerEventId eventId, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            // If the tag does not exist, then we use the default loggin level
            String tag = tagged.GetName();
            if (_tagLogLevel.ContainsKey(tag) == false)
            {
                if (_tagLogLevel[DEFAULT].FatalOn == false)
                    return;
            }
            else
            {
                // The tag exists, check if the desired level is on
                if (_tagLogLevel[tag].FatalOn == false)
                {
                    return;
                }
            }

            WriteFormattedLog(FatalLevel, logstring, eventId, memberName, sourceFilePath, sourceLineNumber);
        }
        #endregion Fatal


        // This is used if the user specificies a tag to be used
        public void Raw(String logString)
        {
            if ((_loggerOutputType == LoggerOutputTypes.Console) || (_loggerOutputType == LoggerOutputTypes.Both))
            {
                Console.WriteLine(logString);
            }

            if ((_loggerOutputType == LoggerOutputTypes.File) || (_loggerOutputType == LoggerOutputTypes.Both))
            {
                // Writing to the filesystem, so perform a lock
                lock (_logLock)
                {
                    if (_logStreamWriter != null)
                    {
                        _logStreamWriter.WriteLine(logString);
                    }
                }
            }
        }

        private void WriteFormattedLog(String level, String logstring, 
            BasicLoggerEventId eventId,
            String memberName,
            String sourceFilePath,
            int sourceLineNumber)
        {
            if ((!File.Exists(_fileName) && (_loggerOutputType != LoggerOutputTypes.Console)))
            {
                return;
            }

            // Get the current excecuting thread
            //int? tid = TaskScheduler.Current.Id;
            //tid = System.Threading.Thread.CurrentThread.ManagedThreadId;
            int? tid = Task.CurrentId.HasValue ? Task.CurrentId : 0;

            // Derive only the filename, not the full path
            // String[] filenameArray;
            String sourceFile = String.Empty;

            if (!String.IsNullOrEmpty(sourceFilePath))
            {
                //filenameArray = sourceFilePath.Split('\\');
                //sourceFile = filenameArray[filenameArray.Length - 1];
                sourceFile = Path.GetFileName(sourceFilePath);
            }

            String eventString;

            if (eventId == null)
            {
                eventString = "-";
            }
            else
            {
                if (String.IsNullOrEmpty(eventId?.Name))
                {
                    eventString = $"{eventId.Id}";
                }
                else
                {
                    eventString = eventId.Name;
                }
            }

            DateTime logTime = DateTime.Now;

            string logString = String.Format("{0}\t{1}\t[{2}]\t{3}\t{4}\t{5}\t{6}\t{7}",
                logTime.ToString(DatetimeFormat),
                tid,
                level,
                sourceFile,
                memberName,
                sourceLineNumber,
                eventString,
                logstring);

            if ((_loggerOutputType == LoggerOutputTypes.Console) || (_loggerOutputType == LoggerOutputTypes.Both))
            {
                Console.WriteLine(logString);
            }

            if ((_loggerOutputType == LoggerOutputTypes.File) || (_loggerOutputType == LoggerOutputTypes.Both))
            {
                // Writing to the filesystem, so perform a lock
                lock (_logLock)
                {
                    if (_logStreamWriter != null)
                    {
                        _logStreamWriter.WriteLine(logString);
                    }

                    if (messageTarget != null)
                        messageTarget(logTime, tid, level, sourceFile, memberName, sourceLineNumber, eventString, logstring);
                }

                RotateLogs();
            }
        }

        private long _logCount = 0;
        private const long _checkRotationLogCount = 5000;

        /// <summary>
        /// Checks the filesize, and if it's greater that the max allowed
        /// rotate the logs.
        /// This also assumes any call to RotateLogs is done within a lock
        /// </summary>
        private void RotateLogs()
        {

            if (_logFileInfo == null)
                return;

            Interlocked.Increment(ref _logCount);

            if (_logCount < _checkRotationLogCount)
                return;

            Interlocked.Exchange(ref _logCount, 0);

            Task.Run(() =>
            {
                _logFileInfo.Refresh();
               if (_logFileInfo.Length > _maxFileSize)
               {
                   String newFileName = _fileName + ".1";
                   try
                   {
                        lock (_logLock)
                        {
                            _logStreamWriter.Flush();
                            _logStreamWriter.Dispose();

                            if (File.Exists(newFileName))
                            {
                                File.Delete(newFileName);
                            }

                            File.Move(_fileName, newFileName);
                            if (!File.Exists(_fileName))
                           {
                               using (var s = File.Create(_fileName))
                               {
                               }
                           }
                            _logStreamWriter = File.AppendText(_fileName);
                            _logStreamWriter.AutoFlush = true;
                            _logFileInfo = new FileInfo(_fileName);
                        }
                    }
                   catch
                   {

                   }
                   Raw(Title);
               }
            });
        }
    }
}
