using System;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Creates <see cref="TraceLogger"/> instances.
    /// </summary>
    public class TraceLoggerFactory : ICredentialProviderLoggerFactory
    {
        /// <summary>
        /// Creates a trace logger for the specified component type.
        /// </summary>
        /// <param name="type">The type that will write log messages.</param>
        /// <returns>A trace logger for the specified type.</returns>
        public ICredentialProviderLogger CreateLogger(Type type)
        {
            return new TraceLogger();
        }

        /// <summary>
        /// Creates a trace logger for the specified component type.
        /// </summary>
        /// <typeparam name="T">The type that will write log messages.</typeparam>
        /// <returns>A trace logger for the specified type.</returns>
        public ICredentialProviderLogger CreateLogger<T>()
        {
            return new TraceLogger();
        }

        private static readonly TraceLoggerFactory loggerFactory = new TraceLoggerFactory();

        /// <summary>
        /// Gets the shared trace logger factory.
        /// </summary>
        public static ICredentialProviderLoggerFactory Instance => loggerFactory;
    }
}
