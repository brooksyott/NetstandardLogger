using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Peamel.BasicLogger.Provider
{
    public class BasicLoggerNetcore : Microsoft.Extensions.Logging.ILogger
    {
        public BasicLoggerProvider Provider { get; private set; }
        String Category;
        Peamel.BasicLogger.ILogger m_plogger;
        Peamel.BasicLogger.IBasicLoggerTag m_ptag;

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel.HasFlag(logLevel))
                return true;
            return false;
        }

        public BasicLoggerNetcore(BasicLoggerProvider Provider, string Category, String filename, String logLevelString)
        {
            this.Provider = Provider;
            this.Category = Category;
            if (m_plogger == null)
            {
                // Logger hasn't been created yet, so create one
                m_plogger = BasicLoggerFactory.CreateLogger(filename, 10);
                m_ptag = new BasicLoggerTag();
                m_plogger.SetLogLevel(logLevelString);
            }
        }

        public BasicLoggerNetcore() { }


        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter
        )
        {
            var msg = formatter(state, exception);
            Peamel.BasicLogger.BasicLoggerLogLevels pLogLevel = BasicLoggerLogLevels.None;

            switch (logLevel)
            {
                case LogLevel.Information:
                    {
                        pLogLevel = BasicLoggerLogLevels.Information;
                        break;
                    }
                case LogLevel.Error:
                    {
                        pLogLevel = BasicLoggerLogLevels.Error;
                        break;
                    }
                case LogLevel.Warning:
                    {
                        pLogLevel = BasicLoggerLogLevels.Warning;
                        break;
                    }
                case LogLevel.Critical:
                    {
                        pLogLevel = BasicLoggerLogLevels.Fatal;
                        break;
                    }
                case LogLevel.Debug:
                    {
                        pLogLevel = BasicLoggerLogLevels.Debug;
                        break;
                    }
                case LogLevel.Trace:
                    {
                        pLogLevel = BasicLoggerLogLevels.Trace;
                        break;
                    }
            }

            BasicLoggerEventId pEventId = null;

            if (eventId != null)
            {
               pEventId = new BasicLoggerEventId(eventId.Id, eventId.Name);
            }

            m_plogger.Force(pLogLevel, m_ptag, pEventId, msg, "CoreLogging", "CoreLogging", 0);
        }
    }

}
