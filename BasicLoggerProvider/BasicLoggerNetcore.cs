using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Peamel.BasicLogger.Extensions.Logging
{
    public class BasicLoggerNetcore : Microsoft.Extensions.Logging.ILogger
    {
        public BasicLoggerProvider Provider { get; private set; }
        String Category;
        Peamel.BasicLogger.ILogger m_plogger;
        Peamel.BasicLogger.IBasicLoggerTag m_ptag;
        int m_maxFileSizeMB = 3;

        public IDisposable BeginScope<TState>(TState state)
        {
            return Provider.ScopeProvider.Push(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel.HasFlag(logLevel))
                return true;
            return false;
        }

        public BasicLoggerNetcore(BasicLoggerProvider Provider, string Category, String filename, String logLevelString, int maxFileSize = 3)
        {
            this.Provider = Provider;
            this.Category = Category;
            if (m_plogger == null)
            {
                // Logger hasn't been created yet, so create one
                m_plogger = BasicLoggerFactory.CreateLogger(filename, maxFileSize);
                m_ptag = new BasicLoggerTag();
                m_maxFileSizeMB = maxFileSize;
                m_plogger.SetLogLevel(logLevelString);
            }
        }

        public BasicLoggerNetcore(BasicLoggerProvider Provider, string Category, String filename, BasicLoggerLogLevels logLevel, int maxFileSize = 3)
        {
            this.Provider = Provider;
            this.Category = Category;
            if (m_plogger == null)
            {
                // Logger hasn't been created yet, so create one
                m_plogger = BasicLoggerFactory.CreateLogger(filename, maxFileSize);
                m_ptag = new BasicLoggerTag();
                m_maxFileSizeMB = maxFileSize;
                m_plogger.SetLogLevel(logLevel);
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
