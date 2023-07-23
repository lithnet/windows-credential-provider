using System;
using System.Diagnostics;

namespace Lithnet.CredentialProvider
{
    public class TraceLogger : ICredentialProviderLogger
    {
        public void LogError(Exception ex, string v)
        {
            Trace.WriteLine($"{v}\r\n\r\n{ex?.ToString()}");
        }

        public void LogError(string v)
        {
            Trace.WriteLine(v);
        }

        public void LogInformation(string message)
        {
            Trace.WriteLine(message);
        }

        public void LogTrace(string v)
        {
            Trace.WriteLine(v);
        }

        public void LogWarning(string message)
        {
            Trace.WriteLine(message);
        }
    }
}
