using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;

using System.Collections.Concurrent;
using System.Reflection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Peamel.BasicLogger.Extensions.Logging
{
    public class BasicLoggerOptions 
    {
        string m_fileName;
        int m_faxFileSizeInMB;
        Boolean m_logToConsole = false;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public BasicLoggerOptions()
        {
        }

        /* properties */
        /// <summary>
        /// The active log level. Defaults to LogLevel.Information
        /// </summary>
        //public LogLevel LogLevel { get; set; } = Microsoft.Extensions.Logging.LogLevel.Information;
            
        /// <summary>
        /// The folder where log files should be placed. Defaults to this Assembly location
        /// </summary>
        public string Filename
        {
            //get { return !string.IsNullOrWhiteSpace(m_fileName) ? m_fileName : System.IO.Path.GetDirectoryName(this.GetType().Assembly.Location); }
            get { return m_fileName; }
            set { m_fileName = value; }
        }

        /// <summary>
        /// The maximum number in MB of a single log file. Defaults to 3.
        /// </summary>
        public int MaxFileSizeInMB
        {
            get { return m_faxFileSizeInMB > 0 ? m_faxFileSizeInMB : 3; }
            set { m_faxFileSizeInMB = value; }
        }

        /// <summary>
        /// The maximum number in MB of a single log file. Defaults to 3.
        /// </summary>
        public Boolean LogToConsole
        {
            get { return m_logToConsole; }
            set { m_logToConsole = value; }
        }
    }
}
