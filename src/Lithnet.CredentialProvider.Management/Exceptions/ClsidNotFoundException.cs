using System;

namespace Lithnet.CredentialProvider
{
    public class ClsidNotFoundException : NotFoundException
    {
        public ClsidNotFoundException() : base()
        {
        }

        public ClsidNotFoundException(string message) : base(message)
        {
        }

        public ClsidNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
