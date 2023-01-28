namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Specifies how a generic tile should be dispalyed in LogonUI
    /// </summary>
    public enum GenericTileDisplayMode
    {
        /// <summary>
        /// Specifies that the generic tile should be displayed as a logon provider under the generic 'Other User' tile.
        /// </summary>
        /// <remarks>
        /// This applies only to scenarios involving LogonUI. This value is ignored for CredUI.
        /// This scenario is only supported in Windows 8 and above.
        /// </remarks>
        DisplayUnderOtherUser = 0,

        /// <summary>
        /// Specifies that the generic tile should be displayed as it's own dedicated credential tile. Note this applies to LogonUI scenarios only.
        /// </summary>
        /// <remarks>
        /// This applies only to scenarios involving LogonUI. This value is ignored for CredUI.
        /// Selecting this option emulates the behaviour of a pre-Windows 8 credential provider.
        /// </remarks>
        DisplayAsDedicatedTile = 1
    }
}
