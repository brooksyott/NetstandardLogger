using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Configuration;
using Peamel.BasicLogger;
using Peamel.BasicLogger.Extensions.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;

namespace NetcoreConsoleAppDemo
{
    class Program
    {
        static BasicLoggerOptions _loggerOptions = new BasicLoggerOptions();


        static IHost Host;
        public static IConfigurationRoot Configuration;

        static void Main(string[] args)
        {
            ConfigureServices();
            Task.Run(async () => await MainAsync()).Wait();

            Console.WriteLine("Done");
        }

        static void ConfigureServices()
        {
            // Build configuration
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Create a host, and configure logging
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                })
               .ConfigureLogging( (hostContext, logging) =>
               {
                   logging.ClearProviders();
                   logging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                   logging.AddBasicLogger();
               })
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddTransient<IBurstLogs, BurstLogs>();
               });

            Host = builder.Build();
        }

        public static List<Task> TaskList = new List<Task>();

        static private async Task MainAsync()
        {
            Console.WriteLine("Starting");
            using (var serviceScope = Host.Services.CreateScope())
            {
                var services2 = serviceScope.ServiceProvider;

                try
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    var myService = services2.GetRequiredService<IBurstLogs>();
                    int idx = 0;
                    for (int i = 0; i < 1000; i++)
                    {
                        idx++;
                        Task t = Task.Run(() => myService.GenerateLogs(idx, 200));
                        TaskList.Add(t);
                    }

                    await Task.WhenAll(TaskList.ToArray());
                    sw.Stop();

                    Console.WriteLine("Success: 200k logs in " + sw.ElapsedMilliseconds + " ms");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("==============   Error Occured " + ex.ToString());
                }
            }

            Console.WriteLine("Stopping");
        }
    }

   


    public interface IBurstLogs
    {
        Task GenerateLogs(int t, int numLogs);
    }

    public class BurstLogs : IBurstLogs
    {
        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public BurstLogs(Microsoft.Extensions.Logging.ILogger<BurstLogs> logger)
        {
            _logger = logger;
            _logger.LogInformation($"************ Constructor");
        }

        public async Task GenerateLogs(int t, int numLogs)
        {
            int idx = t;
            _logger.LogInformation($"************ Starting thread, idx {idx}");
            for (int i = 0; i < numLogs; i++)
            {
                _logger.LogInformation($"Log {idx}, {i}");
                await Task.Delay(0);
            }
        }
    }
}
