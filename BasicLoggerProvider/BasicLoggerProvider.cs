using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Peamel.BasicLogger.Extensions.Logging
{
    /// <summary>
    /// Heavily used (cloned) https://github.com/tbebekis/AspNetCore-CustomLoggingProvider
    /// </summary>
    [ProviderAlias("BasicLogger")]
    public class BasicLoggerProvider : IDisposable, ILoggerProvider, ISupportExternalScope
    {
        IExternalScopeProvider fScopeProvider;
        protected IDisposable SettingsChangeToken;

        Microsoft.Extensions.Logging.ILogger m_logger;

        String m_basicLoggerFilename = String.Empty;
        int m_maxFileSizeMB = 3;

        private bool m_disposed = false; // To detect redundant calls
        public bool IsDisposed { get; protected set; }

        void ISupportExternalScope.SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            fScopeProvider = scopeProvider;
        }

        /// <summary>
        /// Returns the scope provider. 
        /// <para>Called by logger instances created by this provider.</para>
        /// </summary>
        internal IExternalScopeProvider ScopeProvider
        {
            get
            {
                if (fScopeProvider == null)
                    fScopeProvider = new LoggerExternalScopeProvider();
                return fScopeProvider;
            }
        }

        Microsoft.Extensions.Logging.ILogger ILoggerProvider.CreateLogger(string Category)
        {
            if (null == m_logger)
            {
                // do some work against config to initialize logging

                m_logger = new BasicLoggerNetcore(this, Category, m_basicLoggerFilename, BasicLoggerLogLevels.Information, m_maxFileSizeMB);
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

        internal BasicLoggerOptions Settings { get; private set; }


        public BasicLoggerProvider(IOptionsMonitor<BasicLoggerOptions> Settings)
            : this(Settings.CurrentValue)
        {
            // https://docs.microsoft.com/en-us/aspnet/core/fundamentals/change-tokens
            SettingsChangeToken = Settings.OnChange(settings => {
                this.Settings = settings;
            });
        }

        public BasicLoggerProvider(BasicLoggerOptions settings)
        {
            m_basicLoggerFilename = settings.Filename;
            m_maxFileSizeMB = settings.MaxFileSizeInMB;
            this.Settings = settings;
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
