using System;

namespace Lithnet.CredentialProvider
{
    public class TraceLoggerFactory : ILoggerFactory
    {
        public ILogger CreateLogger(Type type)
        {
            return new TraceLogger();
        }

        public ILogger CreateLogger<T>()
        {
            return new TraceLogger();
        }

        private static readonly TraceLoggerFactory loggerFactory = new TraceLoggerFactory();

        public static ILoggerFactory Instance => loggerFactory;
    }
}
