using System;

namespace Lithnet.CredentialProvider
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(Type type);
        
        ILogger CreateLogger<T>();
    }
}
