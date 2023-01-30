using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace Lithnet.CredentialProvider.Samples
{
    internal static class InternalLogger
    {
        internal static ILoggerFactory LoggerFactory { get; }

        static InternalLogger()
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
            InternalLogger.LoggerFactory = new NLogLoggerFactory(new NLogLoggerProvider(new NLogProviderOptions() { ReplaceLoggerFactory = true }, LogManager.LogFactory));
        }
    }
}
