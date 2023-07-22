using System;

namespace Lithnet.CredentialProvider
{
    public interface ILogger
    {
        void LogError(Exception ex, string v);
        void LogError(string v);
        void LogTrace(string v);
        void LogWarning(string message);
    }
}
