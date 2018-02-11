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

    internal class LoggerLevels
    {
        internal Boolean TraceOn = true;
        internal Boolean DebugOn = true;
        internal Boolean InfoOn = true;
        internal Boolean WarnOn = false;
        internal Boolean ErrorOn = false;
        internal Boolean FatalOn = false;

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
