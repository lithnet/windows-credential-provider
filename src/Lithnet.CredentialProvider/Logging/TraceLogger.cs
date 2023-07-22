using System;
using System.Diagnostics;

namespace Lithnet.CredentialProvider
{
    public class TraceLogger : ICredentialProviderLogger
    {
        public void LogError(Exception ex, string v)
        {
            Trace.TraceError($"{v}\r\n\r\n{ex?.ToString()}");
        }

        public void LogError(string v)
        {
            Trace.TraceError(v);
        }

        public void LogInformation(string message)
        {
            Trace.TraceInformation(message);
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
