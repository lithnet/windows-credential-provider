namespace Lithnet.CredentialProvider
{
    /// <summary>
    /// Identifies the type of data supplied to Consent UI for an elevation request.
    /// </summary>
    public enum ConsentUIType
    {
        /// <summary>
        /// The data describes an executable file.
        /// </summary>
        Exe = 0,

        /// <summary>
        /// The data describes an elevated COM object.
        /// </summary>
        Com = 1,

        /// <summary>
        /// The data describes a Windows Installer package.
        /// </summary>
        Msi = 2,

        /// <summary>
        /// The data describes an ActiveX installation.
        /// </summary>
        ActiveX = 3,

        /// <summary>
        /// The data uses the CredCollect structure. The purpose of this structure is not documented.
        /// </summary>
        CredCollect = 4,

        /// <summary>
        /// The data describes a packaged application.
        /// </summary>
        Msix = 5
    }
}
