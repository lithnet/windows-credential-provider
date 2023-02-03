using System;

namespace Lithnet.CredentialProvider
{
    public class ProgIdNotFoundException : NotFoundException
    {
        public ProgIdNotFoundException() : base()
        {
        }

        public ProgIdNotFoundException(string message) : base(message)
        {
        }

        public ProgIdNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
