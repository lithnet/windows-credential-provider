using System;

namespace Lithnet.CredentialProvider
{
    public interface ICredentialProviderLogger
    {
        void LogError(Exception ex, string message);

        void LogError(string message);

        void LogTrace(string message);

        void LogInformation(string message);

        void LogWarning(string message);
    }
}
