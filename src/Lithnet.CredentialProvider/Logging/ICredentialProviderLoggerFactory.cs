using System;

namespace Lithnet.CredentialProvider
{
    public interface ICredentialProviderLoggerFactory
    {
        ICredentialProviderLogger CreateLogger(Type type);

        ICredentialProviderLogger CreateLogger<T>();
    }
}
