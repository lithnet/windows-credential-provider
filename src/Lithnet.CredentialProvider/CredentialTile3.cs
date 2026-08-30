using System;
using Lithnet.CredentialProvider.Interop;

namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Represents a version 3 credential tile that preserves transparency in bitmap controls.
    /// </summary>
    /// <remarks>Inherit from this class when a <see cref="CredentialProviderLogoControl"/> or <see cref="UserTileControl"/> must preserve the image's alpha channel. The <see cref="BitmapControl.BackgroundColor"/> property does not apply to this tile type. Microsoft does not publish documentation for the underlying version 3 credential interfaces, so use <see cref="CredentialTile2"/> unless you need image transparency.</remarks>
    public abstract partial class CredentialTile3 : CredentialTile2
    {
        /// <summary>
        /// Initializes a generic version 3 credential tile.
        /// </summary>
        /// <param name="credentialProvider">The credential provider that owns this tile.</param>
        protected CredentialTile3(CredentialProviderBase credentialProvider) : this(credentialProvider, null) { }

        /// <summary>
        /// Initializes a version 3 credential tile for a user.
        /// </summary>
        /// <param name="credentialProvider">The credential provider that owns this tile.</param>
        /// <param name="user">The user represented by this tile, or <see langword="null"/> for a generic tile.</param>
        protected CredentialTile3(CredentialProviderBase credentialProvider, CredentialProviderUser user) : base(credentialProvider, user) { }
    }
}
