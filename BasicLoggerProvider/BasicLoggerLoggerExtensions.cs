using System;
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
    static public class BasicLoggerExtensions
    {
        static public ILoggingBuilder AddBasicLogger(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();

            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, BasicLoggerProvider>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<BasicLoggerOptions>, BasicLoggerOptionsSetup>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IOptionsChangeTokenSource<BasicLoggerOptions>, LoggerProviderOptionsChangeTokenSource<BasicLoggerOptions, BasicLoggerProvider>>());
            return builder;
        }

        /// <summary>
        /// Adds the file logger provider, aliased as 'File', in the available services as singleton and binds the file logger options class to the 'File' section of the appsettings.json file.
        /// </summary>
        static public ILoggingBuilder AddBasicLogger(this ILoggingBuilder builder, Action<BasicLoggerOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            builder.AddBasicLogger();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}
