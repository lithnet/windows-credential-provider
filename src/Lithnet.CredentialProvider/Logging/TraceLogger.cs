using System;
using System.Diagnostics;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Writes credential provider log messages to <see cref="Trace"/>.
    /// </summary>
    public class TraceLogger : ICredentialProviderLogger
    {
        /// <summary>
        /// Writes an error message and its associated exception to <see cref="Trace"/>.
        /// </summary>
        /// <param name="ex">The exception associated with the error.</param>
        /// <param name="v">The error message.</param>
        public void LogError(Exception ex, string v)
        {
            Trace.WriteLine($"{v}\r\n\r\n{ex?.ToString()}");
        }

        /// <summary>
        /// Writes an error message to <see cref="Trace"/>.
        /// </summary>
        /// <param name="v">The error message.</param>
        public void LogError(string v)
        {
            Trace.WriteLine(v);
        }

        /// <summary>
        /// Writes an informational message to <see cref="Trace"/>.
        /// </summary>
        /// <param name="message">The informational message.</param>
        public void LogInformation(string message)
        {
            Trace.WriteLine(message);
        }

        /// <summary>
        /// Writes a trace message to <see cref="Trace"/>.
        /// </summary>
        /// <param name="v">The trace message.</param>
        public void LogTrace(string v)
        {
            Trace.WriteLine(v);
        }

        /// <summary>
        /// Writes a warning message to <see cref="Trace"/>.
        /// </summary>
        /// <param name="message">The warning message.</param>
        public void LogWarning(string message)
        {
            Trace.WriteLine(message);
        }
    }
}
