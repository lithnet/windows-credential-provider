namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Represents a 'v2' user credential tile that implements the functionality of <see cref="CredentialTile"/>, and includes support for personalized tile, where a single user tile is shown, with multiple logon options grouped within it.
    /// </summary>
    /// <remarks>Inheriting from this class enables you to provide a v2 credential tile. V2 credential tiles were introduced in Windows 8. See the Microsoft documentation on ICredentialProviderCredential2 for more information</remarks>
    /// <inheritdoc/>
    public abstract partial class CredentialTile2 : CredentialTile
    {
        /// <summary>
        /// Gets the user represented by this credential tile
        /// </summary>
        public CredentialProviderUser User { get; }

        /// <summary>
        /// Gets a value indicating if this is a personalized or generic tile
        /// </summary>
        public override bool IsGenericTile => this.User == null;

        /// <summary>
        /// Gets or sets a value that controls how the generic tile is displayed to the end user.
        /// </summary>
        /// <remarks>This does not apply in scenarios where a personalized tile is provided</remarks>
        public GenericTileDisplayMode GenericTileDisplayMode { get; set; }

        protected CredentialTile2(CredentialProviderBase credentialProvider) : this(credentialProvider, null) { }

        protected CredentialTile2(CredentialProviderBase credentialProvider, CredentialProviderUser user) : base(credentialProvider)
        {
            this.User = user;
        }
    }
}
