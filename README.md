## BasicLogger

### Why another netstandard logger?

This is a very simple netstandard logger, that outputs a file into a tab delimeted format, so it can be imported into excel very easily. It has a few features that loggers I've looked at do not have. The logger works with desktop apps, Xamarin Forms, netcore and UWP.

1. Supports multiple loggers, with a specified path, and a maximum file size for rotation

Using the BasicLoggerFactory.CreateLogger, you can name the logger with a string, and supply the full path to the logger. This is handy for XAMARIN forms apps.
```
// LoggerName is the name of the logger, DEFAULT being the default name
// logFileName is the full path of the log file to be used
// maxFileSize will cause the file to be rotated if it exceeds that size in megabytes
public static ILogger CreateLogger(string loggerName, string logFileName, long maxFileSize = 10);

// Since LoggerName is omitted, DEFAULT is the refence to this logger
// logFileName is the full path of the log file to be used
// maxFileSize will cause the file to be rotated if it exceeds that size in megabytes
public static ILogger CreateLogger(string logFileName, long maxFileSize = 1);
 ```

2. Support for Empty loggers

   If you get a logger than hasn't been created yet, it installs an empty logger so classes using a logger do not need to check for null. The class would just need to provide a way to update the logger once it's been created.

```
// Gets the default logger
BasicLoggerFactory.GetLogger();

// Gets a logger by name
BasicLoggerFactory.GetLogger("SOMELOGGER");

```

3. Logging Tags

   A logger can have tags associated with it. This allows logging levels to be set for a specific tag. If a log is generated using a tag that does not exist, it will using the default logging level.  

```
// Info log using no tag, or more specifically the default tag
_log.Info("Info log");

// Info log, using a tag, which is just a string
_log.Info("Info log", "LOGLEVELTAG");

// Sets the log level for logs not tagged
_log.SetLogLevel(BASICLOGGERLEVELS.WARN);

// Sets the log level for logs with tagged "LOGLEVELTAG"
_log.SetLogLevel(BASICLOGGERLEVELS.TRACE, "LOGLEVELTAG");
```
