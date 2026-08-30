using System;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Receives log messages from a credential provider and its credential tiles.
    /// </summary>
    public interface ICredentialProviderLogger
    {
        /// <summary>
        /// Logs an error message and its associated exception.
        /// </summary>
        /// <param name="ex">The exception associated with the error.</param>
        /// <param name="message">The error message.</param>
        void LogError(Exception ex, string message);

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The error message.</param>
        void LogError(string message);

        /// <summary>
        /// Logs a trace message.
        /// </summary>
        /// <param name="message">The trace message.</param>
        void LogTrace(string message);

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The informational message.</param>
        void LogInformation(string message);

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message.</param>
        void LogWarning(string message);
    }
}
