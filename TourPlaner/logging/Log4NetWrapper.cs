using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Media3D;

namespace TourPlaner.logging
{
    /// <summary>
    /// Wraps the log4j2 logger instances by realizing interface ILoggerWrapper.
    /// This avoids direct dependencies to log4j2 package.
    /// </summary>
    public class Log4NetWrapper : ILoggerWrapper
    {
        private readonly log4net.ILog logger;

        public static Log4NetWrapper CreateLogger(string configPath, string caller)
        {
            if (!File.Exists(configPath))
            {
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();
            // Debug.WriteLine($"Current working directory: {currentDirectory}");

                throw new ArgumentException($"Konfigurationsdatei '{configPath}' existiert nicht.", nameof(configPath));
            }

            var logger = log4net.LogManager.GetLogger(caller);  // System.Reflection.MethodBase.GetCurrentMethod().DeclaringType
            log4net.Config.XmlConfigurator.Configure(new FileInfo(configPath));
            return new Log4NetWrapper(logger);
        }

        private Log4NetWrapper(log4net.ILog logger)
        {
            this.logger = logger;
        }

        public void Debug(string message)
        {
            this.logger.Debug(message);
        }
        public void Warn(string message)
        {
            this.logger.Warn(message);
        }

        public void Error(string message)
        {
            this.logger.Error(message);
        }

        public void Fatal(string message)
        {
            this.logger.Fatal(message);
        }
    }
}
