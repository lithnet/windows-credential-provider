namespace Lithnet.CredentialProvider.UnitTests.ComInterop
{
    internal sealed class AbiTestCredentialTile2 : CredentialTile2
    {
        public AbiTestCredentialTile2(CredentialProviderBase credentialProvider) : base(credentialProvider)
        {
        }

        protected override CredentialResponseBase GetCredentials()
        {
            return null;
        }
    }
}
