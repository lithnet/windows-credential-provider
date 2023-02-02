using System;
using Lithnet.CredentialProvider.Interop;
using Microsoft.Extensions.Logging;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Represents a user credential tile that implements the functionality of <see cref="CredentialTile"/> and <see cref="CredentialTile2"/>, but includes support for dynamically updating bitmap images.
    /// </summary>
    /// <remarks>This interface is public, but undocumented by Microsoft. It is recommended to use <see cref="CredentialTile2"/> tiles unless this specific functionality is needed</remarks>
    internal abstract partial class CredentialTile3 : CredentialTile2
    {
        protected CredentialTile3(CredentialProviderBase credentialProvider) : this(credentialProvider, null) { }

        protected CredentialTile3(CredentialProviderBase credentialProvider, CredentialProviderUser user) : base(credentialProvider, user) { }
    }
}
