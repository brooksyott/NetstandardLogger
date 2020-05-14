﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;

using System.Collections.Concurrent;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Peamel.BasicLogger.Extensions.Logging
{
    /// <summary>
    /// Configures a FileLoggerOptions instance by using ConfigurationBinder.Bind against an IConfiguration.
    /// <para>This class essentially binds a FileLoggerOptions instance with a section in the appsettings.json file.</para>
    /// </summary>
    internal class BasicLoggerOptionsSetup : ConfigureFromConfigurationOptions<BasicLoggerOptions>
    {
        /// <summary>
        /// Constructor that takes the IConfiguration instance to bind against.
        /// </summary>
        public BasicLoggerOptionsSetup(ILoggerProviderConfiguration<BasicLoggerProvider> providerConfiguration)
            : base(providerConfiguration.Configuration)
        {
        }
    }
}
