using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peamel.BasicLogger
{
    public class BasicLoggerFactory
    {
        private static Dictionary<String, ILogger> _loggers = new Dictionary<string, ILogger>();
        private const String _defaultLoggerName = Logger.DEFAULT;
        static private Object lockObject = new Object();

        /// <summary>
        /// Gets a logger by name.
        /// </summary>
        /// <param name="loggerName"></param>
        /// <returns></returns>
        public static ILogger GetLogger(String loggerName = _defaultLoggerName)
        {
            // Make any updates to a logger thread safe
            lock ( lockObject )
            {
                if (_loggers.ContainsKey(loggerName))
                {
                    return _loggers[loggerName];
                }

                // If the logger doesn't exist, create an empty logger
                // This allows classes to instantiate a logger even if a logger doesn't exist
                // and not have a null pointer exception
                _loggers[loggerName] = new EmptyLogger();

                return _loggers[loggerName];
            }
        }

        /// <summary>
        /// Gets a logger by name.
        /// </summary>
        /// <param name="loggerName"></param>
        /// <returns></returns>
        public static ILogger CreateLogger(String loggerName, String logFileName, long maxFileSize = 10)
        {
            // Make any updates to a logger thread safe
            lock ( lockObject)
            {
                if (String.IsNullOrEmpty(logFileName)) return null;

                if (_loggers.ContainsKey(loggerName))
                {
                    if (_loggers[loggerName].GetType() != typeof(EmptyLogger))
                    {
                        // If the logger already exists, just return it
                        return _loggers[loggerName];
                    }
                }

                // The logger was either empty, or didn't exist, so create it
                _loggers[loggerName] = new Logger(logFileName, maxFileSize);

                return _loggers[loggerName];
            }
        }

        /// <summary>
        /// Creates the default logger.
        /// </summary>
        /// <param name="loggerName"></param>
        /// <returns></returns>
        public static ILogger CreateLogger(String logFileName, long maxFileSize = 1)
        {
            return CreateLogger(_defaultLoggerName, logFileName, maxFileSize);
        }

        /// <summary>
        /// Updates the logger. This should be used if the logger configuration has changed
        /// </summary>
        /// <param name="loggerName"></param>
        /// <returns></returns>
        public static Boolean UpdateLogger(String loggerName, Logger logger)
        {
            // Make any updates to a logger thread safe
            lock( lockObject)
            {
                if (!_loggers.ContainsKey(loggerName))
                {
                    return false;
                }
                _loggers[loggerName] = logger;

                return true;
            }
        }
    }
}
