using System;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Creates loggers for credential provider components.
    /// </summary>
    public interface ICredentialProviderLoggerFactory
    {
        /// <summary>
        /// Creates a logger for the specified component type.
        /// </summary>
        /// <param name="type">The type that will write log messages.</param>
        /// <returns>A logger for the specified type.</returns>
        ICredentialProviderLogger CreateLogger(Type type);

        /// <summary>
        /// Creates a logger for the specified component type.
        /// </summary>
        /// <typeparam name="T">The type that will write log messages.</typeparam>
        /// <returns>A logger for the specified type.</returns>
        ICredentialProviderLogger CreateLogger<T>();
    }
}
