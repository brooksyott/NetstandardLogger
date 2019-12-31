using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peamel.BasicLogger
{
    public enum BASICLOGGERLEVELS
    {
        OFF,
        FATAL,
        ERROR,
        WARN,
        INFO,
        DEBUG,
        TRACE
    };

    public enum BasicLoggerLogLevels
    {
        None = 6,
        Fatal = 5,
        Error = 4,
        Warning = 3,
        Information = 2,
        Debug = 1,
        Trace = 0
    };

    internal class LoggerLevels
    {
        internal Boolean TraceOn = true;
        internal Boolean DebugOn = true;
        internal Boolean InfoOn = true;
        internal Boolean WarnOn = false;
        internal Boolean ErrorOn = false;
        internal Boolean FatalOn = false;

        internal void SetLogLevel(BasicLoggerLogLevels logLevel)
        {
            switch (logLevel)
            {
                case BasicLoggerLogLevels.Trace:
                    {
                        TraceOn = true;
                        DebugOn = true;
                        InfoOn = true;
                        WarnOn = true;
                        ErrorOn = true;
                        FatalOn = true;
                        break;
                    }
                case BasicLoggerLogLevels.Debug:
                    {
                        TraceOn = false;
                        DebugOn = true;
                        InfoOn = true;
                        WarnOn = true;
                        ErrorOn = true;
                        FatalOn = true;
                        break;
                    }
                case BasicLoggerLogLevels.Information:
                    {
                        TraceOn = false;
                        DebugOn = false;
                        InfoOn = true;
                        WarnOn = true;
                        ErrorOn = true;
                        FatalOn = true;
                        break;
                    }
                case BasicLoggerLogLevels.Warning:
                    {
                        TraceOn = false;
                        DebugOn = false;
                        InfoOn = false;
                        WarnOn = true;
                        ErrorOn = true;
                        FatalOn = true;
                        break;
                    }
                case BasicLoggerLogLevels.Error:
                    {
                        TraceOn = false;
                        DebugOn = false;
                        InfoOn = false;
                        WarnOn = false;
                        ErrorOn = true;
                        FatalOn = true;
                        break;
                    }
                case BasicLoggerLogLevels.Fatal:
                    {
                        TraceOn = false;
                        DebugOn = false;
                        InfoOn = false;
                        WarnOn = false;
                        ErrorOn = false;
                        FatalOn = true;
                        break;
                    }
                case BasicLoggerLogLevels.None:
                    {
                        TraceOn = false;
                        DebugOn = false;
                        InfoOn = false;
                        WarnOn = false;
                        ErrorOn = false;
                        FatalOn = false;
                        break;
                    }
            }
        }

        internal Boolean SetLogLevel(String logLevel)
        {
            String tLogLevel = logLevel.ToLowerInvariant();

            switch (tLogLevel)
            {
                case "trace":
                    {
                        TraceOn = true;
                        DebugOn = true;
                        InfoOn = true;
                        WarnOn = true;
                        ErrorOn = true;
                        FatalOn = true;
                        return true;
                    }
                case "debug":
                    {
                        TraceOn = false;
                        DebugOn = true;
                        InfoOn = true;
                        WarnOn = true;
                        ErrorOn = true;
                        FatalOn = true;
                        return true;
                    }
                case "information":
                    {
                        TraceOn = false;
                        DebugOn = false;
                        InfoOn = true;
                        WarnOn = true;
                        ErrorOn = true;
                        FatalOn = true;
                        return true;
                    }
                case "warning":
                    {
                        TraceOn = false;
                        DebugOn = false;
                        InfoOn = false;
                        WarnOn = true;
                        ErrorOn = true;
                        FatalOn = true;
                        return true;
                    }
                case "error":
                    {
                        TraceOn = false;
                        DebugOn = false;
                        InfoOn = false;
                        WarnOn = false;
                        ErrorOn = true;
                        FatalOn = true;
                        return true;
                    }
                case "fatal":
                    {
                        TraceOn = false;
                        DebugOn = false;
                        InfoOn = false;
                        WarnOn = false;
                        ErrorOn = false;
                        FatalOn = true;
                        return true;
                    }
                case "none":
                    {
                        TraceOn = false;
                        DebugOn = false;
                        InfoOn = false;
                        WarnOn = false;
                        ErrorOn = false;
                        FatalOn = false;
                        return true;
                    }
                default:
                    {
                        return false;
                    }
            }
        }

        internal BasicLoggerLogLevels CurrentLogLevel()
        {
            if (TraceOn == true) return BasicLoggerLogLevels.Trace;
            if (DebugOn == true) return BasicLoggerLogLevels.Debug;
            if (InfoOn == true) return BasicLoggerLogLevels.Information;
            if (WarnOn == true) return BasicLoggerLogLevels.Warning;
            if (ErrorOn == true) return BasicLoggerLogLevels.Error;
            if (FatalOn == true) return BasicLoggerLogLevels.Fatal;
            return BasicLoggerLogLevels.None;
        }

        internal void SetLogLevel(BASICLOGGERLEVELS loglevel)
        {
            switch (loglevel)
            {
                case BASICLOGGERLEVELS.TRACE:
                    {
                        TraceOn = true;
                        DebugOn = true;
                        InfoOn = true;
                        WarnOn = true;
                        ErrorOn = true;
                        FatalOn = true;
                        break;
                    }
                case BASICLOGGERLEVELS.DEBUG:
                    {
                        TraceOn = false;
                        DebugOn = true;
                        InfoOn = true;
                        WarnOn = true;
                        ErrorOn = true;
                        FatalOn = true;
                        break;
                    }
                case BASICLOGGERLEVELS.INFO:
                    {
                        TraceOn = false;
                        DebugOn = false;
                        InfoOn = true;
                        WarnOn = true;
                        ErrorOn = true;
                        FatalOn = true;
                        break;
                    }
                case BASICLOGGERLEVELS.WARN:
                    {
                        TraceOn = false;
                        DebugOn = false;
                        InfoOn = false;
                        WarnOn = true;
                        ErrorOn = true;
                        FatalOn = true;
                        break;
                    }
                case BASICLOGGERLEVELS.ERROR:
                    {
                        TraceOn = false;
                        DebugOn = false;
                        InfoOn = false;
                        WarnOn = false;
                        ErrorOn = true;
                        FatalOn = true;
                        break;
                    }
                case BASICLOGGERLEVELS.FATAL:
                    {
                        TraceOn = false;
                        DebugOn = false;
                        InfoOn = false;
                        WarnOn = false;
                        ErrorOn = false;
                        FatalOn = true;
                        break;
                    }
                case BASICLOGGERLEVELS.OFF:
                    {
                        TraceOn = false;
                        DebugOn = false;
                        InfoOn = false;
                        WarnOn = false;
                        ErrorOn = false;
                        FatalOn = false;
                        break;
                    }
            }
        }

        internal BASICLOGGERLEVELS GetLogLevel()
        {
            if (TraceOn == true) return BASICLOGGERLEVELS.TRACE;
            if (DebugOn == true) return BASICLOGGERLEVELS.DEBUG;
            if (InfoOn == true) return BASICLOGGERLEVELS.INFO;
            if (WarnOn == true) return BASICLOGGERLEVELS.WARN;
            if (ErrorOn == true) return BASICLOGGERLEVELS.ERROR;
            if (FatalOn == true) return BASICLOGGERLEVELS.FATAL;
            return BASICLOGGERLEVELS.OFF;
        }

    }
}
