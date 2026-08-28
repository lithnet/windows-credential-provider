using System;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace Lithnet.CredentialProvider.Samples
{
    internal class InternalLoggerFactory : ICredentialProviderLoggerFactory
    {
        internal static ICredentialProviderLoggerFactory Instance { get; }

        private ILoggerFactory loggerFactory;

        public InternalLoggerFactory(ILoggerFactory factory)
        {
            this.loggerFactory = factory;
        }

        static InternalLoggerFactory()
        {
            /*
                This sample uses NLog to capture trace events from the provider, but you can use any
                logging system compatible with Microsoft.Extensions.Logging;
            */

            var config = new NLog.Config.LoggingConfiguration();
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            /*

                Add file based logging if required

            */

            //var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "c:\\file.txt" };
            //config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, logfile);

            config.AddRule(NLog.LogLevel.Trace, NLog.LogLevel.Fatal, logconsole);

            LogManager.Configuration = config;
            var loggerFactory = new NLogLoggerFactory(new NLogLoggerProvider(new NLogProviderOptions() { ReplaceLoggerFactory = true }, LogManager.LogFactory));
            InternalLoggerFactory.Instance = new InternalLoggerFactory(loggerFactory);
        }

        public ICredentialProviderLogger CreateLogger(Type type)
        {
           return new CredentialProviderLogger(loggerFactory.CreateLogger(type));
        }

        public ICredentialProviderLogger CreateLogger<T>()
        {
            return new CredentialProviderLogger(loggerFactory.CreateLogger<T>());
        }
    }

    public class CredentialProviderLogger : ICredentialProviderLogger
    {
        private readonly Microsoft.Extensions.Logging.ILogger logger;

        public CredentialProviderLogger(Microsoft.Extensions.Logging.ILogger logger)
        {
            this.logger = logger;
        }

        public void LogError(Exception ex, string message)
        {
            this.logger.LogError(ex, message);
        }

        public void LogError(string message)
        {
            this.logger.LogError(message);
        }

        public void LogTrace(string message)
        {
            this.logger.LogTrace(message);
        }

        public void LogInformation(string message)
        {
            this.logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            this.logger.LogWarning(message);
        }
    }
}
