using System;
using System.Diagnostics;

namespace Lithnet.CredentialProvider
{
    public class TraceLogger : ILogger
    {
        public void LogError(Exception ex, string v)
        {
            Trace.TraceError(v + "\r\n\r\n" + ex?.ToString());
        }

        public void LogError(string v)
        {
            Trace.TraceError(v);
        }

        public void LogTrace(string v)
        {
            Trace.TraceInformation(v);
        }

        public void LogWarning(string message)
        {
            Trace.TraceWarning(message);
        }
    }
}
