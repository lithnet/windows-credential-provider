using System;

namespace Lithnet.CredentialProvider
{
    public class TraceLoggerFactory : ICredentialProviderLoggerFactory
    {
        public ICredentialProviderLogger CreateLogger(Type type)
        {
            return new TraceLogger();
        }

        public ICredentialProviderLogger CreateLogger<T>()
        {
            return new TraceLogger();
        }

        private static readonly TraceLoggerFactory loggerFactory = new TraceLoggerFactory();

        public static ICredentialProviderLoggerFactory Instance => loggerFactory;
    }
}
