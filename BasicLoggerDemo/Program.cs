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

        public void GenerateLogs(BasicLoggerTag Tag)
        {
            _log.Trace(Tag, "Trace log");
            _log.Debug(Tag, "Debug log");
            _log.Info(Tag, "Info log");
            _log.Warn(Tag, "Warning log");
            _log.Error(Tag, "Error log");
            _log.Fatal(Tag, "Fatal log");
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
        static ILogger _log2;
        static SampleClassToLog classToLog = new SampleClassToLog();

        static void Main(string[] args)
        {
            // Get a logger that has not been initialized.
            // Nothing should be outputed

            // Should not generate any logs, since a logger has not been set
            // However, it should also not generate a null pointer exception
            _log = BasicLoggerFactory.CreateLogger($".\\BasicLogger.log");
            _log = BasicLoggerFactory.GetLogger();   // gets the default logger

            // Create a second logger, different file name
            _log2 = BasicLoggerFactory.CreateLogger("SECONDLOGGER", $".\\BasicLogger2.log");

            _log.Raw("***  GenerateLogs should not generate logs");

            classToLog.GenerateLogs();

            // Now instantiate a real logger

            // Attach the logger to the class
            _log.Raw("***  GenerateLogs attached to the logger, should generate logs");
            classToLog.UpdateLogger(_log);

            // It should now generate logs from info and above
            // Info is the default logging level

            // Create a tag for the logger
            BasicLoggerTag tag = new BasicLoggerTag("TAGGED");

            _log.Raw("***  Showing use of tags, to customize which logs are generated");
            _log.SetLogLevel(BASICLOGGERLEVELS.WARN);
            _log.SetLogLevel(BASICLOGGERLEVELS.TRACE, tag); // Setting the log level also creates a tag if it doesn't exist
            // Could also create the tag
            //_log.CreateTag("TAGGED");

            _log.Raw("***  GenerateLogs with no tag, log level of WARN");
            classToLog.GenerateLogs();
            _log.Raw("***  GenerateLogs with a tag, log level of TRACE");
            classToLog.GenerateLogs(tag);

            _log.Raw("***  GenerateLogs from a different thread");
            classToLog.GenerateLogsDifferentThread().Wait();

            _log = BasicLoggerFactory.GetLogger("SECONDLOGGER");   // gets the second logger
            _log2.Info("This should be in a second log file");

        }
    }
}
