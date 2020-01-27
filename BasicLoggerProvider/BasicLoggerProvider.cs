using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace Peamel.BasicLogger.Extensions.Logging
{
    public class BasicLoggerProvider : IDisposable, ILoggerProvider, ISupportExternalScope
    {
        IExternalScopeProvider fScopeProvider;
        protected IDisposable SettingsChangeToken;

        Microsoft.Extensions.Logging.ILogger m_logger;
        IConfiguration m_config;

        String m_basicLoggerFilename = String.Empty;
        String m_logLevelString = String.Empty;

        private bool m_disposed = false; // To detect redundant calls
        public bool IsDisposed { get; protected set; }

        void ISupportExternalScope.SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            fScopeProvider = scopeProvider;
        }

        Microsoft.Extensions.Logging.ILogger ILoggerProvider.CreateLogger(string Category)
        {
            if (null == m_logger)
            {
                // do some work against config to initialize logging

                m_logger = new BasicLoggerNetcore(this, Category, m_basicLoggerFilename, m_logLevelString);
            }

            return m_logger;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                    m_logger = null;
                }

                m_disposed = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }

        public BasicLoggerProvider(IConfiguration configuration)
        {
            m_config = configuration;
            m_basicLoggerFilename = configuration["Logging:BasicLogger:Filename"];
            if (String.IsNullOrEmpty(m_basicLoggerFilename))
                m_basicLoggerFilename = ".\\BasicLogger.log";
            m_logLevelString = configuration["Logging:LogLevel:BasicLogger"];
            if (String.IsNullOrEmpty(m_logLevelString))
                m_logLevelString = "Error";
        }

        ~BasicLoggerProvider()
        {
            if (!this.IsDisposed)
            {
                Dispose(false);
            }
        }
    }
}
