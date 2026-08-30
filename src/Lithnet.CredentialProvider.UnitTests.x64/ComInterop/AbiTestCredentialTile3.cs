using System;

namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    internal sealed class AbiTestCredentialTile3 : CredentialTile3
    {
        public AbiTestCredentialTile3(CredentialProviderBase credentialProvider) : base(credentialProvider)
        {
        }

        public bool ThrowOnLoad { get; set; }

        public override void OnLoad()
        {
            if (this.ThrowOnLoad)
            {
                throw new InvalidOperationException("Callback ABI test OnLoad failure");
            }
        }

        protected override CredentialResponseBase GetCredentials()
        {
            return null;
        }
    }
}
