using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Runtime.CompilerServices;
using System.IO;

// Requires 
namespace Peamel.BasicLogger
{

    /// <summary>
    /// The logger class writes logs to a file, and will create and .old.log file when rotating based
    /// on the maximum allowed file size.
    /// The TAG allows the caller to setup log levels specific to a tag, but they all go 
    /// to the same log file (for now)
    /// </summary>
    public class Logger : ILogger
    {
        public const String DEFAULT = "DEFAULT";

        private string DatetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        private const string Title = "Date Time\tThread\tLevel\tFile\tMethod\tLine\tLog";
        private string _fileName;
        private Object _logLock = new Object();
        private long _maxFileSize = 0;
        private FileInfo _logFileInfo = null;
        private BasicLoggerTag _defaultTag = new BasicLoggerTag(DEFAULT);

        // Keeps tracks of the log levels based on the tag
        private Dictionary<String, LoggerLevels> _tagLogLevel;

        /// <summary>
        /// Configures the logger
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="LogFileName"></param>
        /// <param name="MaxFileSize">FileSize to rotate in Megabytes</param>
        /// <returns></returns>
        public Boolean ConfigureLogger(String LogFileName, long MaxFileSize = 10)
        {
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
                    using (var f = File.Create(LogFileName))
                    {
                    }
                }
            }

            return loggerSuccess;
        }

        public Logger(String LogFileName, long MaxFileSize = 1)
        {
            Boolean rc = ConfigureLogger(LogFileName, MaxFileSize);

            // Set up the default log tag and log level (INFO LEVEL)
            _tagLogLevel = new Dictionary<string, LoggerLevels>();
            SetLogLevel(BASICLOGGERLEVELS.INFO, _defaultTag);
        }

        public void SetLogLevel(BASICLOGGERLEVELS loglevel)
        {
            SetLogLevel(loglevel, _defaultTag);
        }

        public void CreateTag(String tag)
        {
            if (_tagLogLevel.ContainsKey(tag) == false)
            {
                // If there is no tag, create it
                _tagLogLevel[tag] = new LoggerLevels();
            }
        }

        public void SetLogLevel(BASICLOGGERLEVELS loglevel, BasicLoggerTag tagged)
        {
            String tag = tagged.TagName;

            if (_tagLogLevel.ContainsKey(tag) == false)
            {
                // If there is no tag, create it
                _tagLogLevel[tag] = new LoggerLevels();
            }
            _tagLogLevel[tag].SetLogLevel(loglevel);
        }

        public BASICLOGGERLEVELS GetLogLevel()
        {
            return GetLogLevel(_defaultTag);
        }

        public BASICLOGGERLEVELS GetLogLevel(BasicLoggerTag tagged)
        {
            String tag = tagged.TagName;

            if (_tagLogLevel.ContainsKey(tag) == false)
            {
                // If there is no tag, create it
                return _tagLogLevel[tag].GetLogLevel();
            }
            return BASICLOGGERLEVELS.OFF;
        }

        // This is used if the user specificies a tag to be used
        public void Trace(String logstring, 
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Trace(_defaultTag, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Trace(BasicLoggerTag tagged, String logstring, 
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            String tag = tagged.TagName;

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

            WriteFormattedLog(BASICLOGGERLEVELS.TRACE.ToString(), logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Debug(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Debug(_defaultTag, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Debug(BasicLoggerTag tagged, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            String tag = tagged.TagName;

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

            WriteFormattedLog(BASICLOGGERLEVELS.DEBUG.ToString(), logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Info(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Info(_defaultTag, logstring, memberName, sourceFilePath, sourceLineNumber);
        }


        // This is used if the user specificies a tag to be used
        public void Info(BasicLoggerTag tagged, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            // If the tag does not exist, then we use the default loggin level
            Stopwatch sw = new Stopwatch();
            sw.Start();
            String tag = tagged.TagName;
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
            WriteFormattedLog(BASICLOGGERLEVELS.INFO.ToString(), logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Warn(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Warn(_defaultTag, logstring, memberName, sourceFilePath, sourceLineNumber);
        }


        // This is used if the user specificies a tag to be used
        public void Warn(BasicLoggerTag tagged, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            // If the tag does not exist, then we use the default loggin level
            String tag = tagged.TagName;
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

            WriteFormattedLog(BASICLOGGERLEVELS.WARN.ToString(), logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Error(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Error(_defaultTag, logstring, memberName, sourceFilePath, sourceLineNumber);
        }


        // This is used if the user specificies a tag to be used
        public void Error(BasicLoggerTag tagged, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            // If the tag does not exist, then we use the default loggin level
            String tag = tagged.TagName;
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

            WriteFormattedLog(BASICLOGGERLEVELS.ERROR.ToString(), logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Fatal(String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Fatal(_defaultTag, logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Fatal(BasicLoggerTag tagged, String logstring,
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            // If the tag does not exist, then we use the default loggin level
            String tag = tagged.TagName;
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

            WriteFormattedLog(BASICLOGGERLEVELS.FATAL.ToString(), logstring, memberName, sourceFilePath, sourceLineNumber);
        }

        // This is used if the user specificies a tag to be used
        public void Raw(String logString)
        {
            // Writing to the filesystem, so perform a lock
            lock (_logLock)
            {
                using (StreamWriter sw = File.AppendText(_fileName))
                {
                    sw.WriteLine(logString);
                }
            }

            RotateLogs();
        }

        private void WriteFormattedLog(String level, String logstring,
            String memberName,
            String sourceFilePath,
            int sourceLineNumber)
        {
            if (!File.Exists(_fileName))
            {
                return;
            }

            // Get the current excecuting thread
            int tid = TaskScheduler.Current.Id;
            tid = System.Threading.Thread.CurrentThread.ManagedThreadId;

            // Derive only the filename, not the full path
            String[] filenameArray;
            String sourceFile = String.Empty;

            if (!String.IsNullOrEmpty(sourceFilePath))
            {
                filenameArray = sourceFilePath.Split('\\');
                sourceFile = filenameArray[filenameArray.Length - 1];
            }

            string logString = String.Format("{0}\t{1}\t[{2}]\t{3}\t{4}\t{5}\t{6}",
                DateTime.Now.ToString(DatetimeFormat),
                tid,
                level,
                sourceFile,
                memberName,
                sourceLineNumber,
                logstring);

            // Writing to the filesystem, so perform a lock
            lock (_logLock)
            {
                using (StreamWriter sw = File.AppendText(_fileName))
                {
                    sw.WriteLine(logString);
                }
            }

            RotateLogs();
        }

        /// <summary>
        /// Checks the filesize, and if it's greater that the max allowed
        /// rotate the logs.
        /// This also assumes any call to RotateLogs is done within a lock
        /// </summary>
        private void RotateLogs()
        {
            if (_logFileInfo == null) return;
            // Should check if this works, or if I need to do a new everytime :)
            if (_logFileInfo.Length > _maxFileSize)
            {
                String newFileName = _fileName + ".1";
                try
                {
                    lock (_logLock)
                    {
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
                            _logFileInfo = new FileInfo(_fileName);
                        }
                    }
                } catch
                {

                }
                Raw(Title);
            }
        }
    }
}
