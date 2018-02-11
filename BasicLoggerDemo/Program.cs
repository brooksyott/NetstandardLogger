using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Peamel.BasicLogger;

namespace BasicLoggerDemo
{
    public class SampleClassToLog
    {
        ILogger _log = null;

        public SampleClassToLog()
        {
            // Instantiate a class, that sets the logger internally;
            _log = BasicLoggerFactory.GetLogger();
        }

        public SampleClassToLog(ILogger logger)
        {
            // Instantiate a class, that sets the logger internally;
            _log = logger;
        }

        public void UpdateLogger(ILogger logger)
        {
            _log = logger;
        }

        public void GenerateLogs()
        {
            _log.Trace("Trace log");
            _log.Debug("Debug log");
            _log.Info("Info log");
            _log.Warn("Warning log");
            _log.Error("Error log");
            _log.Fatal("Fatal log");
        }

        public void GenerateLogs(String Tag)
        {
            _log.Trace("Trace log", "TAGGED");
            _log.Debug("Debug log", "TAGGED");
            _log.Info("Info log", "TAGGED");
            _log.Warn("Warning log", "TAGGED");
            _log.Error("Error log", "TAGGED");
            _log.Fatal("Fatal log", "TAGGED");
        }

        // Logging should show a different thread ID
        public async Task GenerateLogsDifferentThread()
        {
           Task t =  Task.Factory.StartNew(() =>
           {
               _log.Trace("Trace log");
               _log.Debug("Debug log");
               _log.Info("Info log");
               _log.Warn("Warning log");
               _log.Error("Error log");
               _log.Fatal("Fatal log");


           });

           await Task.WhenAll(t);
        }

    }

    class Program
    {
        static ILogger _log;
        static SampleClassToLog classToLog = new SampleClassToLog();

        static void Main(string[] args)
        {
            // Get a logger that has not been initialized.
            // Nothing should be outputed

            // Should not generate any logs, since a logger has not been set
            // However, it should also not generate a null pointer exception
            _log = BasicLoggerFactory.CreateLogger("TAGGED", $"c:\\temp\\BasicLogger.log");

            _log.Raw("***  classToLog.GenerateLogs should not generate logs");

            classToLog.GenerateLogs();

            // Now instantiate a real logger

            // Attach the logger to the class
            _log.Raw("***  classToLog.GenerateLogs attached to the logger, should generate logs");
            classToLog.UpdateLogger(_log);

            // It should now generate logs from info and above
            // Info is the default logging level

            _log.Raw("***  Showing use of tags, to customize which logs are generated");
            _log.SetLogLevel(BASICLOGGERLEVELS.WARN);
            _log.SetLogLevel(BASICLOGGERLEVELS.TRACE, "TAGGED");

            _log.Raw("***  GenerateLogs with no tag, log level of WARN");
            classToLog.GenerateLogs();
            _log.Raw("***  GenerateLogs with a tag, log level of TRACE");
            classToLog.GenerateLogs("TAGGED");

            _log.Raw("***  GenerateLogs from a different thread");
            classToLog.GenerateLogsDifferentThread().Wait();

            //Console.WriteLine("Done");
            //Console.ReadLine();
        }
    }
}
